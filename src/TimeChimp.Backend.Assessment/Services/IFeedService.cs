using System.Threading;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DTO;

namespace TimeChimp.Backend.Assessment.Services
{
    public interface IFeedService
    {
        Task<RssFeed> GetRssFeedByNameAsync(string name, CancellationToken cancelationToken);
    }
}
