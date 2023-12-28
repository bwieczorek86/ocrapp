namespace OcrPlugin.App.Azure.Storage.Debtors.MergeObjects
{
    public abstract class BaseDebtorEntity : CustomTableEntity
    {
        public string ContractId { get; set; }
        public string DebtorName { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string PublicId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
    }
}