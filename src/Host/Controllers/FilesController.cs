using BuildingBlocks.Interfaces;
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
    private readonly IUserContext _userContext;
    
    public FilesController(IResumeService resumeService, IUserContext userContext)
    {
        _resumeService = resumeService;
        _userContext = userContext;
    }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ResumeResponse>> UploadResumeAsync([FromForm]IFormFile file, CancellationToken ct = default)
    {
        await using var stream = file.OpenReadStream();

        var command = UploadFileCommand.Create(
            _userContext.UserId,
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            DateTime.UtcNow,
            string.Empty
        );

        var result = await _resumeService.UploadResumeAsync(command, ct);
        return Ok(result);
    }
}