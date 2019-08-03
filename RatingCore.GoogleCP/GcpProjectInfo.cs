using System;
using System.Collections.Generic;
using System.Text;

namespace RatingCore.GoogleCP
{
    public class GcpProjectInfo : IGcpProjectInfo
    {
        public string ComputeRegion { get; set; }
        public string ProjectID { get; set; }
        public string BucketName { get; set; }
    }
}
