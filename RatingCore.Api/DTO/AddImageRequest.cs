using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingCore.Api.DTO
{
    public class CreateRateableRequest : ImageRequest
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public double Rating { get; set; }
    }
}
