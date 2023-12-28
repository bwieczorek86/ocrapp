namespace OcrPlugin.App.Azure.Company
{
    public interface ICompanyProvider
    {
        public Company GetCompanyId(string name);
    }
}