using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TimeChimp.Backend.Assessment.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class ExampleController : Controller
    {
        public class Example
        {
            [Required]
            public Guid Id { get; set; }

            public string Message { get; set; }
        }

        /// <summary>
        /// This is an example GET action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Example{ Id = Guid.NewGuid(), Message = "GET Example" });
        }

        /// <summary>
        /// This is an example POST action
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /v1/example
        ///     {
        ///        "id": "40f96542-ac26-45c3-8fe1-a7b8c8f97d08",
        ///        "message": "This is an example item"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Example item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null or invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public IActionResult Create(Example example)
        {
            return CreatedAtRoute("Get", new {id = example.Id}, example);
        }
    }
}