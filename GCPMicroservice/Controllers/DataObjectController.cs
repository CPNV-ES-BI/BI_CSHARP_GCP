using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create([FromForm]string key, [FromForm]string base64Content)
    {
        try {
            byte[] content = Convert.FromBase64String(base64Content);

            await _dataObject.Create(key, content);
            return Ok();
        }
        catch (FormatException e)
        {
            return BadRequest("Invalid base64 string");
        }
    }

    [HttpGet]
    [Route("{key}")]
    public async Task<IActionResult> Download(string key)
    {
        byte[] content = await _dataObject.Download(key);
        return Ok(Convert.ToBase64String(content));
    }

    [HttpPatch]
    [Route("{key}/publish")]
    public async Task<IActionResult> Publish(string key)
    {
        string url = await _dataObject.Publish(key);
        return Ok(url);
    }

    [HttpDelete]
    [Route("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        await _dataObject.Delete(key);
        return Ok();
    }
}
