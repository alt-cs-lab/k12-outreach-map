using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Http.Extensions;

namespace K12OutreachMap.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string? Host {get; set;}

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        Host = Environment.GetEnvironmentVariable("HOST");
    }

    public void OnGet()
    {

    }
}
