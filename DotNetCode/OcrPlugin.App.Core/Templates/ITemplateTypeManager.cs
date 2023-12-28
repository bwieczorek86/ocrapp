using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Core.Templates
{
    public interface ITemplateTypeManager
    {
        Type ResolveType(string templateType);
        IEnumerable<string> GetTypeNames();
    }
}
