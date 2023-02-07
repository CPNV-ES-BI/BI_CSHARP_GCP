using MimeTypes;
using Microsoft.AspNetCore.Mvc;
using GCPMicroservice.Services;

namespace GCPMicroservice.Api.Controllers;

[Route("api/data-objects")]
[ApiController]
public class DataObjectController : ControllerBase
{
    private GCPDataObject _dataObject;

    public DataObjectController()
    {
        _dataObject = new();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]string key, [FromForm] IFormFile file, [FromQuery] bool force = false)
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
    public async Task<IActionResult> Download(string key)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];
        if (contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }
        
        string fileExt = MimeTypeMap.GetExtension(contentType);
        string fullKey = $"{key}{fileExt}";
        
        byte[] content = await _dataObject.Download(fullKey);
        
        return File(content, contentType, key);
    }

    [HttpPatch]
    [Route("{key}/publish")]
    public async Task<IActionResult> Publish(string key)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];
        if (contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }

        string fileExt = MimeTypeMap.GetExtension(contentType);
        string fullKey = $"{key}{fileExt}";

        string url = await _dataObject.Publish(fullKey);
        return Ok(url);
    }

    [HttpDelete]
    [Route("{key}")]
    public async Task<IActionResult> Delete(string key, [FromQuery] bool recursively = false)
    {
        string contentType = HttpContext.Request.Headers["Content-Type"];
        if (contentType is null)
        {
            return BadRequest("Missing Content-Type header");
        }

        string fullKey = key;

        if (!recursively)
        {
            string fileExt = MimeTypeMap.GetExtension(contentType);
            fullKey += fileExt;
        }
        
        await _dataObject.Delete(fullKey, recursively);
        return Ok();
    }
}
