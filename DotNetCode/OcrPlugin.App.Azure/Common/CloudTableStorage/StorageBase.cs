using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Common.CloudTableStorage
{
    public abstract class StorageBase
    {
        protected readonly ILogger Logger;

        private readonly ICloudTableClientFactory _cloudTableClientFactory;

        protected StorageBase(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactory cloudTableClientFactory)
        {
            _cloudTableClientFactory = cloudTableClientFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected async Task<TTableEntity> RetrieveEntity<TTableEntity>(string partitionKey, string rowKey, string tableName)
            where TTableEntity : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var retrieveOperation = TableOperation.Retrieve<TTableEntity>(partitionKey, rowKey);
                var result = await table.ExecuteAsync(retrieveOperation);
                var tableEntity = (TTableEntity)result.Result;

                return tableEntity;
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while retrieving the entity");
                throw;
            }
        }

        protected async Task<bool> Exists<TTableEntity>(string partitionKey, string rowKey, string tableName)
            where TTableEntity : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var retrieveOperation = TableOperation.Retrieve<TTableEntity>(partitionKey, rowKey);
                var result = await table.ExecuteAsync(retrieveOperation);

                return result.Result != null;
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while retrieving the entity");
                throw;
            }
        }

        protected async Task<IReadOnlyCollection<TTableEntity>> RetrieveEntities<TTableEntity>(string partitionKey, string tableName)
            where TTableEntity : ITableEntity, new()
        {
            try
            {
                var table = GetTable(tableName);
                var query = new TableQuery<TTableEntity>().Where(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey",
                        QueryComparisons.Equal,
                        partitionKey));

                var result = new List<TTableEntity>();
                TableContinuationToken token = null;

                do
                {
                    var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                    result.AddRange(segment.Results);
                    token = segment.ContinuationToken;
                }
                while (token != null);

                return result;
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while retrieving entities");
                throw;
            }
        }

        protected async Task<IReadOnlyCollection<TTableEntity>> RetrieveRangeQuery<TTableEntity>(string partitionKey, string rowKey, string tableName)
            where TTableEntity : ITableEntity, new()
        {
            try
            {
                var table = GetTable(tableName);
                var query = GetRangeQuery<TTableEntity>(partitionKey, rowKey);

                var result = new List<TTableEntity>();
                TableContinuationToken token = null;

                do
                {
                    var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                    result.AddRange(segment.Results);
                    token = segment.ContinuationToken;
                }
                while (token != null);

                return result;
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while retrieving entities");
                throw;
            }
        }

        protected async Task Upsert<TTableEntity>(TTableEntity entity, string tableName)
            where TTableEntity : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var upsertOperation = TableOperation.InsertOrReplace(entity);
                await table.ExecuteAsync(upsertOperation);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while upserting the entity");
                throw;
            }
        }

        protected async Task Upsert<TTableEntity>(IEnumerable<TTableEntity> tableEntities, string tableName)
            where TTableEntity : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                await UpsertBatch(table, tableEntities);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while upserting the entity");
                throw;
            }
        }

        protected async Task Delete<TTableEntity>(TTableEntity entity, string tableName)
            where TTableEntity : TableEntity
        {
            try
            {
                entity.ETag = "*";
                var table = GetTable(tableName);
                var deleteOperation = TableOperation.Delete(entity);
                await table.ExecuteAsync(deleteOperation);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while deleting the entity");
                throw;
            }
        }

        protected async Task Delete<TTableEntity>(IEnumerable<TTableEntity> tableEntities, string tableName)
            where TTableEntity : TableEntity
        {
            try
            {
                var table = GetTable(tableName);

                var batchOperation = new TableBatchOperation();
                foreach (var entity in tableEntities)
                {
                    batchOperation.Add(TableOperation.Delete(entity));
                }

                await ExecuteInBatch(batchOperation, table);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while deleting the entity");
                throw;
            }
        }

        protected async Task Merge<TUpdateObject>(TUpdateObject entity, string tableName)
            where TUpdateObject : TableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var upsertOperation = TableOperation.Merge(entity);

                await table.ExecuteAsync(upsertOperation);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while merging the entity");
                throw;
            }
        }

        protected async Task Merge<TUpdateObject>(IEnumerable<TUpdateObject> entities, string tableName)
            where TUpdateObject : TableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var batchOperation = new TableBatchOperation();
                foreach (var entity in entities)
                {
                    batchOperation.Add(TableOperation.Merge(entity));
                }

                await ExecuteInBatch(batchOperation, table);
            }
            catch (StorageException storageException)
            {
                Logger.LogError(storageException, "Error occured while merging the entity");
                throw;
            }
        }

        protected CloudTable GetTable(string tableName)
        {
            var tableClient = _cloudTableClientFactory.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);

            return table;
        }

        protected TableQuery<TTableEntity> GetRangeQuery<TTableEntity>(string partitionKey, string startsWithPattern)
            where TTableEntity : ITableEntity
        {
            var length = startsWithPattern.Length - 1;
            var lastChar = startsWithPattern[length];
            var nextLastChar = (char)(lastChar + 1);
            var startsWithEndPattern = startsWithPattern.Substring(0, length) + nextLastChar;

            var prefixCondition = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(
                    "RowKey",
                    QueryComparisons.GreaterThanOrEqual,
                    startsWithPattern),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(
                    "RowKey",
                    QueryComparisons.LessThan,
                    startsWithEndPattern));

            var filterString = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(
                    "PartitionKey",
                    QueryComparisons.Equal,
                    partitionKey),
                TableOperators.And,
                prefixCondition);

            return new TableQuery<TTableEntity>().Where(filterString);
        }

        private async Task ExecuteInBatch(TableBatchOperation batchOperation, CloudTable table)
        {
            var batches = Split(batchOperation, 100);

            var result = new TableBatchResult();
            foreach (var batch in batches)
            {
                result.AddRange(await table.ExecuteBatchAsync(batch));
            }
        }

        private async Task UpsertBatch<T>(CloudTable cloudTable, IEnumerable<T> entities)
            where T : ITableEntity
        {
            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.Add(TableOperation.InsertOrReplace(entity));
            }

            var batches = Split(batchOperation, 100);
            foreach (var batch in batches)
            {
                await cloudTable.ExecuteBatchAsync(batch);
            }
        }

        private IEnumerable<TableBatchOperation> Split(TableBatchOperation source, int size)
        {
            TableBatchOperation bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                bucket ??= new TableBatchOperation();

                bucket.Add(item);
                count++;

                if (count != size)
                {
                    continue;
                }

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                var tableOperations = bucket.ToArray();
                Array.Resize(ref tableOperations, count);
                yield return bucket;
            }
        }
    }
}