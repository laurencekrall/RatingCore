﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingCore.Api.DTO
{
    public class AddImageRequest : ImageRequest
    {
        public string Name { get; set; }
    }
}