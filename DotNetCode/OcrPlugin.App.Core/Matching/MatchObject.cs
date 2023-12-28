namespace OcrPlugin.App.Core.Matching
{
    public class MatchObject
    {
        public string ContractId { get; set; }
        public long? Pesel { get; set; }
        public string DebtorName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public long? Nip { get; set; }
        public long? PublicId { get; set; }
    }
}