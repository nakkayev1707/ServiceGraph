using Microsoft.AspNetCore.Mvc;

namespace ServiceGraph.Visualization.Core.Web;

public class WebVisualizationController : ControllerBase
{
    private readonly WebVisualizer _webVisualizer;

    public WebVisualizationController(WebVisualizer webVisualizer)
    {
        _webVisualizer = webVisualizer;
    }

    [HttpGet("/graph")]
    public IActionResult GetGraph()
    {
        string dot = _webVisualizer.GenerateDot();
        return Content(dot, "text/plain");
    }
}