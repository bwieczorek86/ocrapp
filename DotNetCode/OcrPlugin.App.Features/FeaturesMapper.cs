using OcrPlugin.App.Azure.Storage.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OcrPlugin.App.Features
{
    public static class FeaturesMapper
    {
        public static IEnumerable<Feature> MapToFeatures(this CompanyFeaturesEntity featuresEntity)
        {
            return featuresEntity.Value.Select(ParseToEnum);
        }

        private static Feature ParseToEnum(string feature)
        {
            return Enum.Parse<Feature>(feature);
        }
    }
}