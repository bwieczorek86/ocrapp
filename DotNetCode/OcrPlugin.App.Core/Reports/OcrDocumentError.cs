namespace OcrPlugin.App.Core.Reports;

public abstract class OcrDocumentError
{
    public abstract string Message { get; set; }
    public string Type { get; set; }

    public OcrDocumentError()
    {
        Type = GetType().Name;
    }
}

public class TemplatesFieldNotFoundError : OcrDocumentError
{
    public override string Message { get; set; } = "Template Fields Not Found";
}

public class TemplateTitleNotFoundError : OcrDocumentError
{
    public override string Message { get; set; } = "Template Tittle Not Found";
}

public class ContractsNotFoundError : OcrDocumentError
{
    public override string Message { get; set; } = "Contracts Not Found";
}

public class InvalidFileOrConfigurationError : OcrDocumentError
{
    public override string Message { get; set; } = "Invalid File or Configuration";
}