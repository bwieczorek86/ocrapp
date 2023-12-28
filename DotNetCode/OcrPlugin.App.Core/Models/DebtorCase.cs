using System.Collections.Generic;

namespace OcrPlugin.App.Core.Models
{
    public class DebtorCase
    {
        public string ContractId { get; set; }
        public ICollection<DebtorInCase> Debtors { get; set; }
    }
}