using System.Collections.Generic;

namespace RatingCore.Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public virtual List<Rating> Rating { get; set; }
    }
}