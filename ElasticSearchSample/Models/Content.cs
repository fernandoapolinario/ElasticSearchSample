using System;

namespace ElasticSearchSample.Models
{
    public class Content
    {
        public int ContentId { get; set; }
        public DateTime PostDate { get; set; }
        public string ContentText { get; set; }
    }
}
