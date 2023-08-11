using Microsoft.AspNetCore.Mvc;
using ReadProcedure.Services;

namespace ReadProcedure.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureReaderService _procedureReaderService;

    private readonly ILogger<ProcedureController> _logger;

    public ProcedureController(ILogger<ProcedureController> logger, IProcedureReaderService procedureReaderService)
    {
        _logger = logger;
        _procedureReaderService = procedureReaderService;
    }


    [HttpGet("{procedureName}")]
    public async Task<string> Read(string procedureName)
    {
        return await _procedureReaderService.ReadAsync(procedureName);
    }
}