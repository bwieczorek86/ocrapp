using System.Collections.Generic;

namespace OcrPlugin.App.Spelling.Models;

public class BingSpellCheckDTO
{
    public int Offset { get; set; }
    public string Token { get; set; }
    public string Type { get; set; }
    public List<SuggestionDTO> Suggestions { get; set; }
}