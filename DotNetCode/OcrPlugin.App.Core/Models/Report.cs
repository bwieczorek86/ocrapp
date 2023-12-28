using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Core.Models;

public class Report
{
    public string Id { get; set; }
    public string TemplateName { get; set; }
    public IEnumerable<ReportFile> ReportFiles { get; set; }
    public DateTime DateTime { get; set; }
    public string UserName { get; set; }
}