using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OcrPlugin.App.Spelling.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OcrPlugin.App.Spelling;

public class BingSpellCheck
{
    private const string MktParameter = "&mkt="; // Strongly suggested (ex. pl-PL)
    private const string ModeParameter = "&mode="; // proof (default), spell for pl-PL

    private readonly ILogger<BingSpellCheck> _log;
    private readonly BingClient _bingClient;

    public BingSpellCheck(BingClient bingClient, ILogger<BingSpellCheck> log)
    {
        _bingClient = bingClient;
        _log = log;
    }

    public async Task<IEnumerable<CorrectedModel>> Correct(IReadOnlyCollection<CorrectModel> correctModels)
    {
        var createQuery = string.Join(" ", correctModels.Select(x => x.Text));
        var query = Regex.Replace(createQuery, @"[0-9\-_/.]", " ");
        var results = await QueryBing(query);
        var tokens = MapTokens(results);
        var correctedModels = CorrectedModels(correctModels, tokens);

        return correctedModels;
    }

    private List<CorrectedModel> CorrectedModels(IEnumerable<CorrectModel> correctModels, List<BingSpellCheckDTO> tokens)
    {
        var correctedModels = new List<CorrectedModel>();
        foreach (var correctModel in correctModels)
        {
            var words = correctModel.Text.Split(" ");
            var correctedText = GetCorrectedText(tokens, words);

            correctedModels.Add(
                new CorrectedModel()
                {
                    PropertyName = correctModel.PropertyName,
                    Text = correctModel.Text,
                    CorrectedText = correctedText
                });
        }

        return correctedModels;
    }

    private string GetCorrectedText(List<BingSpellCheckDTO> tokens, string[] words)
    {
        return words.Aggregate(
            string.Empty,
            (current, word) => current + GetSuggestedWord(tokens, word))
            .Trim();
    }

    private string GetSuggestedWord(List<BingSpellCheckDTO> tokens, string word)
    {
        var token = tokens?.FirstOrDefault(x => x.Token == word);
        if (token == null)
        {
            return $"{word} ";
        }

        var highestScoreSuggestion = token.Suggestions.MaxBy(x => x.Score)?.Suggestion;
        var suggestion = !string.IsNullOrEmpty(highestScoreSuggestion)
            ? ToTitleCase(highestScoreSuggestion)
            : token.Token;

        return $"{suggestion} ";
    }

    private List<BingSpellCheckDTO> MapTokens(JEnumerable<JToken> results)
    {
        return results.Select(result => result.ToObject<BingSpellCheckDTO>()).ToList();
    }

    private async Task<JEnumerable<JToken>> QueryBing(string words)
    {
        JEnumerable<JToken> results;
        var uri = CreateParamsForUrl(words.Trim());
        var response = await _bingClient.GetBingTokens(uri);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (string.IsNullOrEmpty(responseJson))
            {
                _log.Log(LogLevel.Warning, $"Bing API request error 500: {response}");
            }

            var json = JObject.Parse(responseJson);
            _log.Log(LogLevel.Warning, $"Bing API bad request response: {json}");
        }
        else
        {
            var json = JObject.Parse(responseJson);
            results = json["flaggedTokens"]!.Children();
        }

        return results;
    }

    private string CreateParamsForUrl(string text)
    {
        var queryString = $"?text={text}"; // Uri.EscapeDataString(text);
        queryString += ModeParameter + "spell"; // "spell"; "proof - only for en-US"
        queryString += MktParameter + "pl-PL";

        return queryString;
    }

    public string ToTitleCase(string str)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }
}