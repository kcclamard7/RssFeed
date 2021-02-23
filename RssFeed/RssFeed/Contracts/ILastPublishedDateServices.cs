using RssFeed.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssFeed.Contracts
{
    /// <summary>
    /// contract for processing inactivity date
    /// </summary>
    public interface ILastPublishedDateServices
    {
        /// <summary>
        /// method for calculating feed inactivity
        /// </summary>
        /// <param name="rssFeeds">dictionary of feed with name and url</param>
        /// <param name="rssCallServices">service for https call</param>
        /// <returns></returns>
        Task<List<RssFeedWithInactivityPeriod>> NumberOfInactivityDays(RssFeeds rssFeeds, IRssCallServices rssCallServices);
    }
}