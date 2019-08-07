using System.Collections.Generic;

namespace RatingCore.Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public virtual IEnumerable<Rating> Rating { get; set; }
    }
}