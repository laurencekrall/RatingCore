﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingCore.Api.DTO
{
    public class CreateRateableRequest : RateImageRequest
    {
        public string Name { get; set; }
        public string FileName { get; set; }
    }
}
