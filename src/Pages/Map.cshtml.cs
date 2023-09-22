using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;

namespace K12OutreachMap.Pages;

public class MapModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private static string SvgText {get;}

    static MapModel() {
        SvgText = System.IO.File.ReadAllText("map.svg");
    }

    public MapModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet(string districts, string fill="#ccc")
    {
        AngleSharp.IConfiguration config = Configuration.Default;
        IBrowsingContext context = BrowsingContext.New(config);
        IDocument doc = await context.OpenAsync(req => req.Content(SvgText));
        
        // Convert fill to hex code if necessary
        if(int.TryParse(fill, System.Globalization.NumberStyles.HexNumber, null, out int output))
        {
            fill = $"#{fill}";
        }

        // Apply colors
        foreach(string num in districts.Replace(" ", string.Empty).Split(',')) {
            string selector = $"#usd{num} polyline";
            IHtmlCollection<IElement> shapes = doc.QuerySelectorAll(selector);
            foreach(Element shape in shapes){
                shape.SetAttribute("fill", fill);
            }
        }

        return Content(doc.DocumentElement.OuterHtml, "image/svg+xml");
    }
}
