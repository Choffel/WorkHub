using Files.Application.Contracts;
using Files.Application.DTOs;
using Files.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IResumeService _resumeService;
    
    public FilesController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ResumeResponse>> UploadResumeAsync(IFormFile file, CancellationToken ct = default)
    {
        var stream = file.OpenReadStream();
        
        var command = UploadFileCommand.Create(stream, file.FileName, file.ContentType, file.Length);
        
        var result = await _resumeService.UploadResumeAsync(command, ct);
    
        return Ok(result);
    }
}