using RssFeed.Models;
using RssFeed.Contracts;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssFeed
{
    /// <summary>
    /// main entry point of the application
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {

            // we gave two RSS Feed for the sake of the demo

            // we generate two object with the feed property 
            var theBibleInAYear = new RSSFeed
            {
                FeedId = 1,
                FeedName = "The Bible in a Year",
                FeedUrl = new Uri("https://feeds.fireside.fm/bibleinayear/rss"),
            };


            //feed two 
            var theApologyLine = new RSSFeed
            {
                FeedId = 2,
                FeedName = "The Apology Line",
                FeedUrl = new Uri("https://rss.art19.com/apology-line"),
            };

            // we store all the feed in the listOfRssFeed
            var listOfRssFeeds = new RssFeeds()
            {
                ListOfRssFeeds = new List<RSSFeed>
                 {
                     theApologyLine,
                     theBibleInAYear
                 }
            };

            // we instanciate the rssService 
            ILastPublishedDateServices lastPublishedDateServices = new LastPublishedDateServices();


            //we instanciate the rss call service to get the xml from api call
            IRssCallServices rssCallServices = new RssCallServices();

            // we call the service 
            var result =  await lastPublishedDateServices.NumberOfInactivityDays(listOfRssFeeds, rssCallServices);

            foreach(var item in result)
            {
                var msg = $"Feed {item.FeedName}, a total of {(int)item.InactivityPeriod} days has past since the last publication";

                if ((int)item.InactivityPeriod == 0)

                    msg = $"Feed {item.FeedName}, a total of 0 day has past since the last publication. The RSS is current";


                Console.WriteLine(msg);

            }
        }
    }
}
