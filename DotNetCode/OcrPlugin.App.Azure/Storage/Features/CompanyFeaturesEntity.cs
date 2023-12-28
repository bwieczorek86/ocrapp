using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Features
{
    public class CompanyFeaturesEntity : CustomTableEntity
    {
        public IEnumerable<string> Value { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public CompanyFeaturesEntity()
        {
        }
    }
}