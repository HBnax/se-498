using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Models;
using pitchamon.Backend.Services;
using pitchamon.Backend.Data;

namespace pitchamon.Backend.Controllers;

[ApiController]
[Route("process/history")]
public class ProcessingHistoryController : ControllerBase
{
    private readonly BackendDbContext dbContext;
    
    public ProcessingHistoryController(BackendDbContext context)
    {
        dbContext = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<object>> GetProcessingHistory([FromQuery] ProcessingHistoryRequest request)
    {
        var history = await dbContext.ProcessingHistory
            .Where(p => p.UserId == request.UserId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        
        return Ok(new { history });
    }
}