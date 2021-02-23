using RssFeed.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssFeed.Contracts
{
    /// <summary>
    /// Contact for processing api call to get the xml
    /// </summary>
    public interface IRssCallServices
    {
        /// <summary>
        /// method to call the api rss feed
        /// </summary>
        /// <param name="listOfRssFeeds">list of feeds url</param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetFeedXmlContent(RssFeeds listOfRssFeeds);
    }
}