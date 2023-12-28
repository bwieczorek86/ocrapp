using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OcrPlugin.App.Azure.Storage
{
    public abstract class CustomTableEntity : TableEntity
    {
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = base.WriteEntity(operationContext);

            foreach (var property in GetType().GetProperties().Where(property =>
                !property.GetCustomAttributes<IgnorePropertyAttribute>(true).Any() &&
                !properties.ContainsKey(property.Name) &&
                typeof(TableEntity).GetProperties().All(p => p.Name != property.Name)))
            {
                var value = property.GetValue(this);
                if (value != null)
                {
                    properties.Add(property.Name, new EntityProperty(JsonConvert.SerializeObject(value)));
                }
            }

            return properties;
        }

        public override void ReadEntity(
            IDictionary<string, EntityProperty> properties,
            OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            foreach (var property in GetType().GetProperties().Where(property =>
                !property.GetCustomAttributes<IgnorePropertyAttribute>(true).Any() &&
                properties.ContainsKey(property.Name) &&
                property.PropertyType != typeof(string) &&
                properties[property.Name].PropertyType == EdmType.String))
            {
                var jToken = TryParseJson(properties[property.Name].StringValue);
                if (jToken != null)
                {
                    var toObjectMethod = jToken.GetType().GetMethod("ToObject", new[] { typeof(Type) });
                    var value = toObjectMethod.Invoke(jToken, new object[] {property.PropertyType});

                    property.SetValue(this, value);
                }
            }
        }

        private static JToken TryParseJson(string s)
        {
            try
            {
                return JToken.Parse(s);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }
    }
}