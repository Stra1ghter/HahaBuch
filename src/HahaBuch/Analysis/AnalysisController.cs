using HahaBuch.SharedContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HahaBuch.Analysis;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnalysisController(IAnalysisService analysisService) : Controller
{
    [HttpGet("categorytotals/{year:int}")]
    public async Task<ActionResult<List<CategoryYearTotalDto>>> GetCategoryYearTotals(int year)
    {
        if (year < 2000 || year > DateTime.UtcNow.Year + 1)
            return BadRequest("Invalid year.");

        try
        {
            var result = await analysisService.GetCategoryYearTotals(year);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
