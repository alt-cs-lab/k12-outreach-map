using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;

namespace K12OutreachMap.Pages;

public class MapModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// A string to cache the starting SVG file's text across model instances
    /// </summary>
    private static readonly string SvgText;

    /// <summary>
    /// The static constructor loads the starting SVG file text into a cache property for reuse
    /// </summary>
    static MapModel() {
        SvgText = System.IO.File.ReadAllText("map.svg");
    }

    /// <summary>
    /// Constructs a new instance of the Map page model
    /// </summary>
    /// <param name="logger">The logger to log to</param>
    public MapModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Generates a customized SVG Map based on the query parameters
    /// </summary>
    /// <param name="districts">The school districts to mark</param>
    /// <param name="fill">The color to highlight the specified districts with</param>
    /// <returns>A customized SVG file to serve</returns>
    public async Task<IActionResult> OnGet(string districts, int? width, int? height, string fill="#512888")
    {
        // Use AngleSharp to load the SVG string into a manipulable document 
        AngleSharp.IConfiguration config = Configuration.Default;
        IBrowsingContext context = BrowsingContext.New(config);
        IDocument doc = await context.OpenAsync(req => req.Content(SvgText));

        // Grab the top-level SVG element, if it exists
        IElement? svgElement = doc.QuerySelector("svg");
        if(svgElement == null) return StatusCode(500);
        
        // Convert fill color to a hex code string if necessary
        if(fill.All("0123456789abcdefABCDEF".Contains))
        {
            fill = $"#{fill}";
        }

        // Apply fill color to selected school districts
        if(!String.IsNullOrEmpty(districts))
        {
            foreach(string num in districts.Replace(" ", string.Empty).Split(',')) {
                string selector = $"#usd{num} polyline";
                IHtmlCollection<IElement> shapes = doc.QuerySelectorAll(selector);
                foreach(Element shape in shapes){
                    shape.SetAttribute("fill", fill);
                }
            }
        }

        // Modify the image size
        if(width != null || height != null )
        {
            if(width != null && height != null) {
                // Apply the specified width and height 
                // Note, this can distort the image if it does not conform to the aspect ratio!
                svgElement.SetAttribute("width", width.ToString());
                svgElement.SetAttribute("height", height.ToString());
            }
            else if(width != null) {
                // Apply the specified width, and calculate the corresponding height from the aspect ratio
                const double aspectRatio = 400.0/700.0;
                int newHeight = (int)Math.Floor(width.Value * aspectRatio);
                svgElement.SetAttribute("width", width.ToString());
                svgElement.SetAttribute("height", newHeight.ToString());
            }
            else if(height != null){
                // Apply the specified height, and calculate the corresponding width from the aspect ratio
                const double aspectRatio = 700.0/400.0;
                int newWidth = (int)Math.Floor(height.Value * aspectRatio);
                svgElement.SetAttribute("width", newWidth.ToString());
                svgElement.SetAttribute("height", height.ToString());
            }
        }


        // Serve the modified SVG
        return Content(svgElement.OuterHtml, "image/svg+xml");
    }
}
