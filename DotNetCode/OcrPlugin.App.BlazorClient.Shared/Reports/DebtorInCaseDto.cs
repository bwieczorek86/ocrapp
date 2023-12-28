namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record DebtorInCaseDto(string ContractId, string DebtorName, string PublicId, string Nip, string Pesel, string Regon, string Email, ICollection<DebtorAddressDto> Addresses, string PublicIdType)
{
    public string GetUniqueId()
    {
        return ContractId + DebtorName + PublicId;
    }

    public string Identifier()
    {
        if (!string.IsNullOrWhiteSpace(PublicId))
        {
            return $"{PublicId} ({PublicIdType})";
        }

        if (!string.IsNullOrWhiteSpace(Nip))
        {
            return Nip;
        }

        if (!string.IsNullOrWhiteSpace(Pesel))
        {
            return Pesel;
        }

        if (!string.IsNullOrWhiteSpace(Regon))
        {
            return Regon;
        }

        return string.Empty;
    }
}