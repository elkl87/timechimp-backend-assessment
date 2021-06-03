using System.Collections.Generic;

namespace TimeChimp.Backend.Assessment.DTO
{
    public class RssFeed
    {
        public IEnumerable<Item> items { get; set; }
    }
}
