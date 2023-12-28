using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.BlazorClient.Server.Components.Reports.ReportDTO;
using OcrPlugin.App.Core.Models;
using DebtorAddress = OcrPlugin.App.Core.Models.DebtorAddress;
using DebtorAddressDto = OcrPlugin.App.BlazorClient.Client.DTOs.DebtorAddress;
using DebtorCase = OcrPlugin.App.Core.Models.DebtorCase;
using DebtorCaseDto = OcrPlugin.App.BlazorClient.Client.DTOs.DebtorCase;
using DebtorInCase = OcrPlugin.App.Core.Models.DebtorInCase;
using DebtorInCaseDto = OcrPlugin.App.BlazorClient.Client.DTOs.DebtorInCase;
using QueueFilesDto = OcrPlugin.App.BlazorClient.Client.DTOs.QueueFiles;

namespace OcrPlugin.App.BlazorClient.Server.Components.Reports.Mappings;

public static class OcrResultMapper
{
    public static DebtorCase ToOcrResultUpdateDtoContract(DebtorCaseDto dto)
    {
        return new DebtorCase
        {
            ContractId = dto.ContractId,
            Debtors = dto.Debtors.Select(ToOcrResultUpdateDtoDebtorInCase).ToList()
        };
    }

    public static OcrResult ToReportOcrResult(this OcrResutltUpdateDto entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new()
        {
            ReportId = entity.ReportId,
            FileName = entity.FileName,
            TemplateName = entity.TemplateName,
            QueueFiles = entity.QueueFiles.Select(ToOcrResultUpdateQueueFile).ToList(),
            ErrorMessage = entity.ErrorMessage,
            Contracts = entity.Contracts.Select(ToOcrResultUpdateDtoContract).ToList(),
        };
    }

    public static DebtorInCase ToOcrResultUpdateDtoDebtorInCase(DebtorInCaseDto entity)
    {
        return new DebtorInCase
        {
            Email = entity.Email,
            Nip = entity.Nip,
            Pesel = entity.Pesel,
            Regon = entity.Regon,
            DebtorName = entity.DebtorName,
            PublicId = entity.PublicId,
            Addresses = entity.Addresses.Select(ToOcrResultUpdateDtoDebtorAddress).ToList()
        };
    }

    private static DebtorAddress ToOcrResultUpdateDtoDebtorAddress(DebtorAddressDto entity)
    {
        return new DebtorAddress
        {
            City = entity.City,
            Street = entity.Street,
            PostalCode = entity.PostalCode
        };
    }

    private static QueueFiles ToOcrResultUpdateQueueFile(QueueFilesDto entity)
    {
        return new QueueFiles
        {
            BlobFileName = entity.BlobFileName,
            FileExtension = entity.FileExtension,
            IsOriginal = entity.IsOriginal
        };
    }
}