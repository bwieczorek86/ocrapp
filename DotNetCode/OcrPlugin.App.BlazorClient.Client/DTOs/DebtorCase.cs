namespace OcrPlugin.App.BlazorClient.Client.DTOs
{
    public class DebtorCase
    {
        public string ContractId { get; set; }
        public ICollection<DebtorInCase> Debtors { get; set; }
    }
}