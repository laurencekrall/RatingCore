using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using RatingCore.Api.DTO;
using RatingCore.GoogleCP;
using RatingCore.GoogleCP.Models;

namespace RatingCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IGoogleApiService _googleService;
        public ImageController(IGoogleApiService googleService)
        {
            _googleService = googleService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost("GetSimilar")]
        public async Task<ActionResult> GetSimilar([FromBody] ImageRequest imageRequest)
        {
            List<ProductSearchResult> getSimilar = await _googleService.GetSimilarAsync(imageRequest.Base64Image);
            return Ok(getSimilar);
        }


        //[HttpPost("Rate/{productName}")]
        //public async Task<ActionResult> Rate(string productName, [FromBody] RateProductRequest imageRequest)
        //{
        //    List<ProductSearchResult> getSimilar = await _googleService.GetSimilarAsync(imageRequest.Base64Image);
        //    return Ok(getSimilar);
        //}

        [HttpPost("Add")]
        public async Task<ActionResult> CreateNewRateable([FromBody] CreateRateableRequest ratable)
        {
            var rate = new Rateable()
            {
               Base64Image = ratable.Base64Image,
               FileName = ratable.FileName,
               ProductName = ratable.Name
            };
            var res = await _googleService.CreateNewRateable(rate);
            
            return Ok(res);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
