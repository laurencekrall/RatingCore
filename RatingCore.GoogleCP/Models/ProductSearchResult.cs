using System;
using System.Collections.Generic;
using System.Text;

namespace RatingCore.GoogleCP.Models
{
    public class ProductSearchResult
    {
        public List<string> ReferenceImages { get; internal set; } = new List<string>();
        public float Score { get; internal set; }
        public string ProductName { get; internal set; }
    }
}
