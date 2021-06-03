using System;

namespace TimeChimp.Backend.Assessment.DTO
{
    public class Link
    {
        public Uri Uri { get; set; }
        public string Title { get; set; }
        public string MediaType { get; set; }
        public string RelationshipType { get; set; }
        public long Length { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
    }
}
