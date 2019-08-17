using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingCore.Api.DTO
{
    public class ProductSearchResult
    {
        public List<string> ReferenceImages { get; internal set; } = new List<string>();
        public float Score { get; internal set; }
        public string ProductName { get; internal set; }
        public List<double> Ratings { get; set; }
    }
}
