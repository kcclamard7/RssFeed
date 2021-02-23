using RssFeed.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RssFeed.Contracts
{
    internal class RssCallServices : IRssCallServices
    {
        public async Task<Dictionary<string, string>> GetFeedXmlContent(RssFeeds listOfRssFeeds)
        {

            var responseXmlcontent = new Dictionary<string, string>();
            using (var client = new HttpClient())
            {
                // we create a response 
                // we can just asynchronously iterate over the list and get the feed 
                // content for each one 
                foreach (var feed in listOfRssFeeds.ListOfRssFeeds)
                {
                    using (var response = await client.GetAsync(feed.FeedUrl))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            // feedcontent is store in an collection for later process
                            responseXmlcontent.Add(feed.FeedName, await response.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            // in case of error we return a friendly message 
                            Console.WriteLine($"Cannot connect to the Feed for {feed.FeedName}. Please try again later");
                        }
                    }
                }

                return responseXmlcontent;
            }
        }
    }
}
