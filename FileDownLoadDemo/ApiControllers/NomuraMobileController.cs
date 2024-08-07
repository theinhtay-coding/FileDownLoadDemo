using Azure.Core;
using FileDownLoadDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadDemo.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateHeadersAttribute))]
    public class NomuraMobileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            // Check for Content-Type header
            //if (!Request.Headers.TryGetValue("Content-Type", out var contentType) || contentType != "application/x-www-form-urlencoded")
            //{
            //    return BadRequest(new { Message = "Missing or invalid Content-Type header." });
            //}

            return Ok("Get all successfully.");
        }

        [HttpPost]
        public IActionResult Example()
        {
            return Ok("Example post method call successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create")]
        public IActionResult Create()
        {
            return Ok("Create successfully");
        }
    }
}
