using OcrPlugin.App.Core.Templates.TemplateTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OcrPlugin.App.Core.Templates
{
    public class TemplateTypeManager : ITemplateTypeManager
    {
        private readonly Dictionary<string, Type> _templateTypeDictionary = new()
        {
            { nameof(GeneralType), typeof(GeneralType) },
        };

        public Type ResolveType(string templateType)
        {
            return _templateTypeDictionary[templateType];
        }

        public IEnumerable<Type> GetTypes()
        {
            return _templateTypeDictionary.Values;
        }

        public IEnumerable<string> GetTypeNames()
        {
            return _templateTypeDictionary.Values.Select(type => type.Name);
        }
    }
}
