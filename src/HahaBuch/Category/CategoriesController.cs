using System.Security.Claims;
using HahaBuch.SharedContracts;
using HahaBuch.SharedContracts.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch.Category;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await categoryService.GetCategories();
            return Ok(categories);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        CategoryDto? category = await categoryService.GetCategory(id);
        
        if (category != null)
            return Ok(category);
        
        return NotFound();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutCategory(Guid id, CategoryDto categoryDto)
    {
        if (categoryDto.Id != id)
            return BadRequest("Category ID mismatch.");

        try
        {
            var updatedCategory = await categoryService.PutCategory(categoryDto);
            return Ok(updatedCategory);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await categoryService.DeleteCategory(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ReferencedByEntitiesException)
        {
            return BadRequest("Could not delete category, it used by transactions.");
        }
    }
}