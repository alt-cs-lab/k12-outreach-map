using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Http.Extensions;

namespace K12OutreachMap.Pages;


/// <summary>
/// The model for the index page
/// </summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// The current base url for the site (i.e. https://k12map.cs.ksu.edu)
    /// </summary>
    public string? BaseUrl {get; set;}

    /// <summary>
    /// Constructs a new Index page model instance
    /// </summary>
    /// <param name="logger">The logger object</param>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        BaseUrl = Environment.GetEnvironmentVariable("BASE_URL");
    }

    /// <summary>
    /// Serves a page with instructions on how to use the map
    /// </summary>
    public void OnGet()
    {

    }
}
