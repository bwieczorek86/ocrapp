using OcrPlugin.App.BlazorClient.Client.Common;

namespace OcrPlugin.App.BlazorClient.Client.DTOs
{
    public class GeneralType : ITemplateType
    {
        public string DebtorName { get; set; }
        public string ContractId { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string PublicId { get; set; }
    }
}