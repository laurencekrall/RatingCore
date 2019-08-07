using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RatingCore.Data.Models
{
    public class RatingCoreContext : DbContext
    {
        public RatingCoreContext(DbContextOptions<RatingCoreContext> options)
            : base(options)
        { }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Product> Products { get; set; }

    }

}
