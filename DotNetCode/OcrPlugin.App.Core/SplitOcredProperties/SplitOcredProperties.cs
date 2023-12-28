using Microsoft.Azure.Documents.SystemFunctions;
using OcrPlugin.App.Spelling;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OcrPlugin.App.Core.SplitOcredProperties;

public class SplitOcredProperties : ISplitOcredProperties
{
    public IReadOnlyCollection<OcredModel> SplitDebtorAddressDataList(IReadOnlyCollection<OcredModel> ocredModels)
    {
        var newOcredModels = new List<OcredModel>();

        foreach (var model in ocredModels)
        {
            newOcredModels.Add(model);
        }

        var modelToSplit = ocredModels.FirstOrDefault(x => x.PropertyName == "DebtorName")!.Text.Split("\n");
        if (modelToSplit.Any())
        {
            foreach (var line in modelToSplit)
            {
                SplitToProperties(line, newOcredModels);
            }
        }

        return newOcredModels;
    }

    public IReadOnlyCollection<OcredModel> SplitDebtorAddressData(OcredModel ocredModel)
    {
        var newOcredModels = new List<OcredModel>();
        newOcredModels.Add(ocredModel);

        if (ocredModel != null)
        {
            foreach (string line in ocredModel!.Text.Split("\n"))
            {
                SplitToProperties(line, newOcredModels);
            }
        }

        return newOcredModels;
    }

    private void SplitToProperties(string line, List<OcredModel> newOcredModels)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            var debtorNameModel = newOcredModels.FirstOrDefault(x => x.PropertyName == "DebtorName");
            var streetModel = newOcredModels.FirstOrDefault(x => x.PropertyName == "Street");
            var cityModel = newOcredModels.FirstOrDefault(x => x.PropertyName == "City");
            var postalCodeModel = newOcredModels.FirstOrDefault(x => x.PropertyName == "PostalCode");

            if (ContainsPolishName(line))
            {
                debtorNameModel!.Text = line;
            }

            if (IsPolishStreetAddress(line))
            {
                AddUpdateStreetModel(line, newOcredModels, streetModel);
            }

            if (ContainsPolishPostalCodeAndCity(line))
            {
                AddUpdateCityModel(line, newOcredModels, cityModel);
                AddUpdatePostalCodeModel(line, newOcredModels, postalCodeModel);
            }
        }
    }

    private void AddUpdatePostalCodeModel(string line, List<OcredModel> newOcredModels, OcredModel postalCodeModel = null)
    {
        if (postalCodeModel != null)
        {
            postalCodeModel!.Text = line;
        }
        else
        {
            newOcredModels.Add(new OcredModel()
            {
                Text = GetPostalCodeFromString(line),
                PropertyName = "PostalCode"
            });
        }
    }

    private void AddUpdateCityModel(string line, List<OcredModel> newOcredModels, OcredModel cityModel = null)
    {
        if (cityModel != null)
        {
            cityModel!.Text = line;
        }
        else
        {
            newOcredModels.Add(new OcredModel()
            {
                Text = GetCityFromString(line),
                PropertyName = "City"
            });
        }
    }

    private void AddUpdateStreetModel(string line, List<OcredModel> newOcredModels, OcredModel streetModel = null)
    {
        if (streetModel != null)
        {
            streetModel!.Text = line;
        }
        else
        {
            newOcredModels.Add(new OcredModel()
            {
                Text = line,
                PropertyName = "Street"
            });
        }
    }

    private bool ContainsPolishName(string input)
        {
            // Define the regular expression pattern for a Polish name or surname
            string pattern = @"^\s*[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+\s+[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+\s*$";

            // Create a regular expression object
            Regex regex = new Regex(pattern);

            // Check if the input string contains a match that doesn't include digits
            Match match = regex.Match(input);
            while (match.Success)
            {
                if (!Regex.IsMatch(match.Value, @"\d"))
                {
                    return true;
                }

                match = match.NextMatch();
            }

            return false;
        }

    private bool ContainsPolishPostalCodeAndCity(string input)
    {
        // Regular expression pattern for a Polish postal code and city name
        string pattern = @"\b\d{2}-\d{3}\s*(\p{L}{1,2}\s*\.*\s*)+";

        // Create a Regex object with the pattern
        Regex regex = new Regex(pattern);

        // Try to match the pattern in the input string
        Match match = regex.Match(input);

        // If the pattern is not found, try matching with a reverse pattern (city name and then postal code)
        if (!match.Success)
        {
            // Reverse pattern for a Polish postal code and city name
            string reversePattern = @"(\p{L}{1,2}\s*\.*\s*)+\s*\b\d{2}-\d{3}\b";

            // Create a Regex object with the reverse pattern
            regex = new Regex(reversePattern);

            // Try to match the reverse pattern in the input string
            match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
        }

        return false;
    }

    private string GetCityFromString(string input)
    {
        string pattern = @"([A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s]+)\s*(\d{2}-\d{3})|(\d{2}-\d{3})\s*([A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s]+)";

        // Create a regular expression object
        Regex regex = new Regex(pattern);

        // Get the first match in the input string
        Match match = regex.Match(input);

        // If there is a match, return the city name
        if (match.Success)
        {
            // Check which capturing group contains the city name
            if (match.Groups[1].Success)
            {
                return match.Groups[1].Value.Trim();
            }
            else if (match.Groups[4].Success)
            {
                return match.Groups[4].Value.Trim();
            }
        }

        return string.Empty;
    }

    private string GetPostalCodeFromString(string input)
    {
        string postalCodePattern = @"\d{2}[-.]\d{3}";
        Regex postalCodeRegex = new Regex(postalCodePattern);
        string postalCode = postalCodeRegex.Match(input).Value;

        return postalCode;
    }

    private bool IsPolishStreetAddress(string input)
    {
        // Regular expression pattern for a Polish street address
        string streetPattern = @"\b[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+\s*(ul\.|\b)(\s*\d{1,3}(/?\s*[A-Za-z]?)?|\s*\d{1,3}[A-Za-z]?(/\d{1,3}(/?\s*[A-Za-z]?)?)?)?\b";

        // Regular expression pattern for a Polish postal code
        string postalPattern = @"\b\d{2}-\d{3}\b";

        // Create Regex objects with the patterns
        Regex streetRegex = new Regex(streetPattern);
        Regex postalRegex = new Regex(postalPattern);

        // Check if the input string matches the street address pattern and does not match the postal code pattern
        Match streetMatch = streetRegex.Match(input);
        Match postalMatch = postalRegex.Match(input);
        return streetMatch.Success && Regex.IsMatch(input, @"\d") && !postalMatch.Success;
    }
}