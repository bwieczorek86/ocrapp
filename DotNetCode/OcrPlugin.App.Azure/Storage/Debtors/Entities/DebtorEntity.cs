using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Debtors.Entities
{
    public class DebtorEntityObject
    {
        public string DebtorName { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string Email { get; set; }
        public string PublicId { get; set; }
        public ICollection<DebtorAddressEntityObject> Addresses { get; set; }
        public ICollection<DebtorCasesEntityObject> Cases { get; set; }
    }
}