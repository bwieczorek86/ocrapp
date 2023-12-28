using System.Collections.Generic;

namespace OcrPlugin.App.Core.Models
{
    public class DebtorInCase
    {
        public string DebtorName { get; set; }
        public string PublicId { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string Email { get; set; }
        public ICollection<DebtorAddress> Addresses { get; set; }
        public ICollection<Case> Cases { get; set; }
        public string PublicIdType { get; set; }
    }
}