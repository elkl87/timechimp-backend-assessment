using System.Collections.Generic;

namespace TimeChimp.Backend.Assessment.DTO
{
    public class IndexViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
