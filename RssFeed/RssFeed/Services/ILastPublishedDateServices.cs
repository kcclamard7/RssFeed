using RssFeed.Models;

using System.Threading.Tasks;

namespace RssFeed.Services
{
    internal interface ILastPublishedDateServices
    {
        Task<RSSFeed> NumberOfInactivityDays();
    }
}