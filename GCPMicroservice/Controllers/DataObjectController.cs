using MimeTypes;
using Microsoft.AspNetCore.Mvc;
using GCPMicroservice.Services;
using System.IO;

namespace GCPMicroservice.Api.Controllers;

[ApiController]
[Route("api/data-objects")]
public class DataObjectController : ControllerBase
{
    private GCPDataObject _dataObject;

    public DataObjectController()
    {
        _dataObject = new();
    }

    [HttpPost]
    [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromForm]string key, IFormFile file, [FromQuery] bool force = false)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not found");
        }
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            string fileExt = MimeTypeMap.GetExtension(file.ContentType);
            string fullKey = $"{key}{fileExt}";

            byte[] content = memoryStream.ToArray();
            await _dataObject.Create(fullKey, content, force);
        }
        return Ok();
    }

    [HttpGet]
    [Route("{key}")]
    [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
    public async Task<IActionResult> Download(string key, [FromQuery] string? path = null)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];
        if (contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }
        
        string fileExt = MimeTypeMap.GetExtension(contentType);
        string fullKey = $"{key}{fileExt}";
        string fullPath = path is null ? fullKey : $"{path}/{fullKey}";
        
        byte[] content = await _dataObject.Download(fullPath);
        
        return File(content, contentType, key);
    }

    [HttpPatch]
    [Route("{key}/publish")]
    [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
    public async Task<IActionResult> Publish(string key, [FromQuery] string? path = null)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];
        if (contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }

        string fileExt = MimeTypeMap.GetExtension(contentType);
        string fullKey = $"{key}{fileExt}";
        string fullPath = path is null ? fullKey : $"{path}/{fullKey}";

        string url = await _dataObject.Publish(fullPath);
        return Ok(url);
    }

    [HttpDelete]
    [Route("{key}")]
    [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string key, [FromQuery] string? path = null, [FromQuery] bool recursively = false)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];

        if (!recursively && contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }

        string fullKey = recursively ? key : $"{key}{MimeTypeMap.GetExtension(contentType)}";
        string fullPath = path is null ? fullKey : $"{path}/{fullKey}";

        await _dataObject.Delete(fullPath, recursively);
        return Ok();
    }
}
