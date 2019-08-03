using System;
using System.Collections.Generic;
using System.Text;

namespace RatingCore.GoogleCP
{
    public interface IGcpProjectInfo
    {
        string ComputeRegion { get; set; }
        string ProjectID { get; set; }
        string BucketName { get; set; }
    }
}
