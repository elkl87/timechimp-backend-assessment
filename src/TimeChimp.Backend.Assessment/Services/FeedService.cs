using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TimeChimp.Backend.Assessment.DTO;
using TimeChimp.Backend.Assessment.Extensions;

namespace TimeChimp.Backend.Assessment.Services
{
    public class FeedService : IFeedService
    {
        private const string rssNewsItemsCacheKey = "rssResult";
        private readonly IMapper Mapper;
        private readonly IDistributedCache DistributedCache;
        private readonly IOptions<FeedUrlsOptions> Options;

        public FeedService(IMapper mapper, IDistributedCache distributedCache, IOptions<FeedUrlsOptions> options)//, string feedUri)
        {
            Mapper = mapper;
            DistributedCache = distributedCache;
            Options = options;

        }

        public async Task<RssFeed> GetRssFeedByNameAsync(string feedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var uri = GetFeedUri(feedName);

            var rssFeed = new RssFeed();
            var rssNewsItems = new List<Item>();

            var cachedResult = await DistributedCache.GetCacheValueAsync<RssFeed>($"{rssNewsItemsCacheKey}_{feedName}");
            if (cachedResult != null)
            {
                return cachedResult;
            }

            using (var xmlReader = XmlReader.Create(uri, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {
                        ISyndicationItem item = await feedReader.ReadItem();
                        Item feedItem = Mapper.Map<Item>(item);
                        rssNewsItems.Add(feedItem);
                    }
                }
                rssFeed.items = rssNewsItems;
            }

            await DistributedCache.SetCacheValueAsync($"{rssNewsItemsCacheKey}_{feedName}", rssFeed);

            return rssFeed;
        }

        public string GetFeedUri(string feedName) => feedName switch
        {
            "film" => Options.Value.Film,
            "tech" => Options.Value.Tech,
            "sport" => Options.Value.Sport,
            _ => throw new ArgumentException(message: "invalid feed name", paramName: nameof(feedName)),
        };
    }
}
