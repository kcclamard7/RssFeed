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

namespace RssFeed.Contracts
{
    /// <summary>
    /// Class for calculating inactivity period
    /// </summary>
    public class LastPublishedDateServices : ILastPublishedDateServices
    {
        /// <summary>
        /// Method return the Rssfeed with inactivity period and name
        /// </summary>
        /// <param name="listOfRssFeeds">collection of feed with url</param>
        /// <param name="rssCallServices">instance of htppc client to  for calling the api</param>
        /// <returns></returns>
        public async Task<List<RssFeedWithInactivityPeriod>> NumberOfInactivityDays(RssFeeds listOfRssFeeds , IRssCallServices rssCallServices)
        {
            try
            {

                // we start by downloading the XML file and storing them inside of an object
                // we can retrieve the value of the RSS by using a HTTP client call 
                // for simplicity we instanciate a client and make a call (no dependency injection of service)

                // we store the http response in a dictionary
                var reponseContentOfFeeds = await rssCallServices.GetFeedXmlContent(listOfRssFeeds);

                //add the list of RssFeedWithInactivty 
                var inactivityListPeriod = new List<RssFeedWithInactivityPeriod>();

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

                   inactivityListPeriod.Add(new RssFeedWithInactivityPeriod
                    {
                        FeedName = item.Key,
                        InactivityPeriod = (int)dateDifferences
                    });
                       
                }

                return inactivityListPeriod;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
