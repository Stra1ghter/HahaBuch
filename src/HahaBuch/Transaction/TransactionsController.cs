using System.Transactions;
using HahaBuch.SharedContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HahaBuch.Transaction;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionService transactionService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<TransactionOverviewDto> transactions = await transactionService.GetTransactions();
        return Ok(transactions);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] TransactionDto transactionDto)
    {
        if (transactionDto.Id != id)
            return BadRequest("Transaction ID mismatch.");
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            TransactionDto result = await transactionService.PutTransaction(transactionDto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Transaction not found.");
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("You do not have access to this transaction.");
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            await transactionService.DeleteTransaction(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Transaction not found.");
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("You do not have access to this transaction.");
        }
    }
}