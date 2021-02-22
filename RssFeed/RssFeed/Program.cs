using RssFeed.Services;

using System;
using System.Threading.Tasks;

namespace RssFeed
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // we instanciate the rssService 
            ILastPublishedDateServices _rssService = new LastPublishedDateServices();

            // we call the service 
            await _rssService.NumberOfInactivityDays();
        }
    }
}
