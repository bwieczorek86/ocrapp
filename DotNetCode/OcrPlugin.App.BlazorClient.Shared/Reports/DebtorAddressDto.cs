namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record DebtorAddressDto(string Street, string City, string PostalCode)
{
    public override string ToString()
    {
        return $"{PostalCode} {City} {Street}";
    }
}