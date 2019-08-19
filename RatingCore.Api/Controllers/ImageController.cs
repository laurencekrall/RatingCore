using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using RatingCore.Api.DTO;
using RatingCore.Data.Models;
using RatingCore.GoogleCP;
using RatingCore.GoogleCP.Models;

namespace RatingCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IGoogleApiService _googleService;
        RatingCoreContext _ratingCoreContext;

        public ImageController(IGoogleApiService googleService, RatingCoreContext ratingCoreContext)
        {
            _googleService = googleService;
            _ratingCoreContext = ratingCoreContext;
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
            List<GoogleCP.Models.ProductSearchResult> getSimilar = await _googleService.GetSimilarAsync(imageRequest.Base64Image);

            var ratings = _ratingCoreContext.Products.Where(x => getSimilar.Select(y => y.ProductName.ToUpper()).Contains(x.ProductName.ToUpper())).ToList();

            var mapped = getSimilar.Select(x => new DTO.ProductSearchResult()
            {
                ProductName = x.ProductName,
                ReferenceImages = x.ReferenceImages,
                Score = x.Score,
                Ratings = ratings.FirstOrDefault(y => y.ProductName.ToUpper() == x.ProductName.ToUpper())?.Rating.Select(z => z.Value).ToList()
            });

            return Ok(mapped);
        }


        [HttpPost("Rate/{productId}")]
        public async Task<ActionResult> Rate(int productId, [FromBody] RateProductRequest imageRequest)
        {

            var product = _ratingCoreContext.Products.FirstOrDefault(x => x.ProductId == productId);
            if(product == null)
            {
                return NotFound("No product with this id");
            }
            product.Rating.Add(new Rating()
            {
                Value = imageRequest.Rating
            });
            await _ratingCoreContext.SaveChangesAsync();
            return Ok();
        }

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

            var product = new Data.Models.Product()
            {
                ProductName = ratable.Name,
                Rating = new List<Rating>() { }
            };
            product.Rating.Add(new Rating()
            {
                Value = ratable.Rating
            });
            _ratingCoreContext.Products.Add(product);
            int saved = _ratingCoreContext.SaveChanges();

            return Ok(product.ProductId);
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
