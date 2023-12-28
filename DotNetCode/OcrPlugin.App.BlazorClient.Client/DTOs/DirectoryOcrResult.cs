namespace OcrPlugin.App.BlazorClient.Client.DTOs
{
    public class DirectoryOcrResult
    {
        public string FileName { get; set; }
        public ICollection<CorrectedModel> CorrectedModels { get; set; } = new List<CorrectedModel>();
        public IEnumerable<DebtorCase> Contracts { get; set; } = new List<DebtorCase>();

        public DirectoryOcrResult(string fileName)
        {
            FileName = fileName;
        }
    }
}