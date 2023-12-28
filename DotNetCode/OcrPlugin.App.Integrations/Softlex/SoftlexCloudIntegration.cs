using OcrPlugin.App.Azure.Storage.AppBlazor;
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Debtors;
using OcrPlugin.App.Core.Integrations.Models;
using OcrPlugin.App.Core.Models;
using ServiceReference1;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Case = OcrPlugin.App.Core.Models.Case;
using DebtorAddress = OcrPlugin.App.Core.Models.DebtorAddress;
using DebtorAddressSoftlex = ServiceReference1.DebtorAddress;
using DebtorSoftlex = ServiceReference1.Debtor;

namespace OcrPlugin.App.Integrations.Softlex
{
    internal class SoftlexCloudIntegration : ISoftlexCloudIntegration
    {
        private readonly IAppBlazorStorage _blazorStorage;
        private readonly IDebtorManager _debtorManager;
        private readonly ISoftlexClientProvider _softlexClientProvider;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SoftlexCloudIntegration(
            IAppBlazorStorage blazorStorage,
            IDebtorManager debtorManager,
            ISoftlexClientProvider softlexClientProvider,
            IDateTimeProvider dateTimeProvider)
        {
            _blazorStorage = blazorStorage;
            _debtorManager = debtorManager;
            _softlexClientProvider = softlexClientProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task UpdateCase(DebtLetter debtLetter)
        {
            await Task.CompletedTask;
        }

        public async Task SendFileToFtp(DebtLetter debtLetter)
        {
            await Task.CompletedTask;
        }

        public async Task InitData(string companyName)
        {
            var config = (await _blazorStorage.GetSoftlexIntegrations()).FirstOrDefault(c => c.CompanyName == companyName);

            await Integrate(config);
        }

        public async Task IncrementData()
        {
            var integrations = await _blazorStorage.GetSoftlexIntegrations();
            foreach (var config in integrations)
            {
                await Integrate(config);
            }
        }

        private async Task Integrate(SoftlexIntegrationConfig config)
        {
            var client = _softlexClientProvider.SoftlexApiClient(config);
            var token = await client.LoginWithUserCredentialsAsync(config.FirmIdentifier, config.Login, config.Password);

            var cases = await client.GetCasesByIncrementAsync(token.Token, config.LastIntegrationDate);
            foreach (var caseIncrement in cases)
            {
                var isValid = Validate(caseIncrement);
                if (!isValid)
                {
                    continue;
                }

                caseIncrement.CaseNo = Sanitize(caseIncrement);
                var debtorCase = ToDebtorCase(caseIncrement);
                await _debtorManager.Upsert(debtorCase, config.CompanyName);

                var debtorIdentifiers = ToDebtorIdentifiers(caseIncrement);
                await _debtorManager.Upsert(debtorIdentifiers, config.CompanyName);
            }

            var dateTimeNow = _dateTimeProvider.GetUtcNow();
            await _blazorStorage.SetSoftlexLastIntegrationDate(config.CompanyName, dateTimeNow);
        }

        private string Sanitize(CaseIncrement caseIncrement)
        {
            return caseIncrement.CaseNo.Replace('/', '_').Replace('\\', '_').Replace('&', '_');
        }

        private bool Validate(CaseIncrement caseIncrement)
        {
            if (string.IsNullOrWhiteSpace(caseIncrement.CaseNo))
            {
                return false;
            }

            return true;
        }

        private DebtorCase ToDebtorCase(CaseIncrement caseIncrement)
        {
            return new()
            {
                ContractId = caseIncrement.CaseNo,
                Debtors = caseIncrement.Debtors.Select(ToDebtor).ToList(),
            };
        }

        private IEnumerable<DebtorIdentifierEntity> ToDebtorIdentifiers(CaseIncrement caseIncrement)
        {
            var listLength = caseIncrement.Debtors.Length * 3;
            var debtorsToReturn = new List<DebtorIdentifierEntity>(listLength);
            foreach (var debtor in caseIncrement.Debtors)
            {
                var debtors = new List<DebtorIdentifierEntity>
                {
                    GetDebtor(debtor, debtor.NIP, DebtorPublicIdType.Nip, caseIncrement.CaseNo),
                    GetDebtor(debtor, debtor.PESEL, DebtorPublicIdType.Pesel, caseIncrement.CaseNo),
                    GetDebtor(debtor, debtor.REGON, DebtorPublicIdType.Regon, caseIncrement.CaseNo),
                };

                debtorsToReturn.AddRange(debtors);
            }

            return debtorsToReturn.Where(c => c != null).ToList();
        }

        private DebtorIdentifierEntity GetDebtor(DebtorSoftlex debtor, string publicId, string publicIdType, string caseNumber)
        {
            var cases = new HashSet<string>();
            cases.Add(caseNumber.Replace("_", "/"));

            if (string.IsNullOrWhiteSpace(publicId))
            {
                return null;
            }

            return new(publicId, publicIdType)
            {
                Email = debtor.Email,
                DebtorName = debtor.Name,
                Nip = debtor.NIP,
                Pesel = debtor.PESEL,
                Regon = debtor.REGON,
                Addresses = debtor.DebtorAddresses.Select(ToAddressEntityObject).ToList(),
                Cases = cases
            };
        }

        private DebtorInCase ToDebtor(DebtorSoftlex debtor)
        {
            var cases = new List<Case>();

            return new()
            {
                Email = debtor.Email,
                DebtorName = debtor.Name,
                Nip = debtor.NIP,
                Pesel = debtor.PESEL,
                Regon = debtor.REGON,

                // TODO Proposition: store all ids in the PublicId (maybe List<PublicId>?)
                PublicId = GetPublicId(debtor),
                Addresses = debtor.DebtorAddresses.Select(ToAddress).ToList(),
                Cases = cases
            };
        }

        private string GetPublicId(DebtorSoftlex debtor)
        {
            // TODO how to counter case where NIP and PESEL exists here and OCR only finds PESEL on the document?
            return debtor.PESEL ?? debtor.NIP ?? debtor.REGON;
        }

        private DebtorAddress ToAddress(DebtorAddressSoftlex debtorAddress)
        {
            return new()
            {
                City = debtorAddress.City,
                Street = debtorAddress.Street,
                PostalCode = debtorAddress.Postcode
            };
        }

        private DebtorAddressEntityObject ToAddressEntityObject(DebtorAddressSoftlex debtorAddress)
        {
            return new()
            {
                City = debtorAddress.City,
                Street = debtorAddress.Street,
                PostalCode = debtorAddress.Postcode,
            };
        }

        private string CreateDebtorTableName(string company)
        {
            return $"{company}Debtors";
        }
    }
}