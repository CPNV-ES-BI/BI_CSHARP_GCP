using Microsoft.AspNetCore.Mvc;

namespace GCPMicroservice.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataObjectController : ControllerBase
{
    private GCPDataObject _dataObject;

    public DataObjectController()
    {
        _dataObject = new();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateDataObject(string key, string base64Content)
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

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteDataObject(string key)
    {
        await _dataObject.Delete(key);
        return Ok();
    }
}
