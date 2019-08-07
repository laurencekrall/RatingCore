using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingCore.Api.DTO
{
    public class RateImageRequest : ImageRequest
    {
        public double Rating { get; set; }
    }
}
