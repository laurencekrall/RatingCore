using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using RatingCore.Api.DTO;
using RatingCore.GoogleCP;

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
        public ActionResult<ProductSearchResults> GetSimilar([FromBody] ImageRequest imageRequest)
        {
            var res = _googleService.GetSimilar(imageRequest.base64Image);
            return Ok(res);
        }


        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] AddImageRequest addImageRequest)
        {
            var res = await _googleService.AddImageToBucket(addImageRequest.base64Image, addImageRequest.Name);
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
