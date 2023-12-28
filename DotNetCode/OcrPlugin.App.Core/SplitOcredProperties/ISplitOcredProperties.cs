using OcrPlugin.App.Spelling;
using System.Collections.Generic;

namespace OcrPlugin.App.Core.SplitOcredProperties;

public interface ISplitOcredProperties
{
    public IReadOnlyCollection<OcredModel> SplitDebtorAddressDataList(IReadOnlyCollection<OcredModel> ocredModels);
    public IReadOnlyCollection<OcredModel> SplitDebtorAddressData(OcredModel ocredModel);
}