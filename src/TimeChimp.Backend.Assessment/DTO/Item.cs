using System;
using System.Collections.Generic;

namespace TimeChimp.Backend.Assessment.DTO
{
    public class Item
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Person> Contributors { get; set; }
        public IEnumerable<Link> Links { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset Published { get; set; }
    }
}
