using RssFeed.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RssFeed.Services
{
    internal class LastPublishedDateServices : ILastPublishedDateServices
    {
        public async Task<RSSFeed> NumberOfInactivityDays()
        {
            try
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
       

                // we start by downloading the XML file and storing them inside of an object
                // we can retrieve the value of the RSS by using a HTTP client call 
                // for simplicity we instanciate a client and make a call (no dependency injection of service)

                using (var client = new HttpClient())
                {
                    // we create a response 
                    var reponseContentOfFeeds = new Dictionary<string, string>();

                    // we can just asynchronously iterate over the list and get the feed 
                    // content for each one 
                    foreach (var feed in listOfRssFeeds.ListOfRssFeeds)
                    {
                        using (var response = await client.GetAsync(feed.FeedUrl))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                // feedcontent is store in an collection for later process
                                reponseContentOfFeeds.Add(feed.FeedName, await response.Content.ReadAsStringAsync());
                            }
                            else
                            {
                                // in case of error we return a friendly message 
                                Console.WriteLine("Cannot connect to the Feed. Please try again later");
                            }
                        }
                    }

                    // once we have all the content for each on of those item, we can then read the xml file and get the pubdate items 
                    // alternatively we could have used the XDocument xml = XDocument.Load(item.FeedUrl) instead of call the httpclient;
                    foreach (var item in reponseContentOfFeeds)
                    {
                        var feedName = item.Key;
                        XDocument xmldoc = XDocument.Parse(item.Value);

                        // we read all the pubdate in the xml
                        var lastDatePubs = xmldoc.Descendants("pubDate")
                            .Select(x => Convert.ToDateTime(((string)x).Split(',')[1]));
   

                        // we get the last date 
                        var lastDatePub = lastDatePubs.Max();


                        // we compare the difference between the last date and today date 
                        var dateDifferences = (DateTime.Today - lastDatePub).TotalDays;

                        var msg = $"Feed {item.Key}, a total of {(int)dateDifferences} days has past since the last publication";

                        if ((int)dateDifferences == 0)

                            msg = $"Feed {item.Key}, a total of 0 day has past since the last publication. The RSS is current";


                        Console.WriteLine(msg);

                    }
                }

                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
