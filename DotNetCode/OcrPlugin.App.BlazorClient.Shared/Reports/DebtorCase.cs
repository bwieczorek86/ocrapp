namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record DebtorCase(string ContractId, ICollection<DebtorInCaseDto> Debtors);