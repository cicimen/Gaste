using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abot;
using Gaste.Data;
using Gaste.Data.Objects;
using Abot.Poco;
using Abot.Crawler;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace Crawler
{
    class Program
    {
        static object locker;
        static SiteConfiguration siteConfiguration;
        static Encoding encoding;
        static void Main(string[] args)
        {
            locker = new object();
            args = new string[1];
            args[0] = "3";
            int siteID = 0;
            if(args.Length != 1 || string.IsNullOrWhiteSpace(args[0]) || !int.TryParse(args[0], out siteID) )
            {
                return;
            }

            siteConfiguration = Configuration.GetSiteConfiguration(siteID);

            if(siteID == 1 || siteID  == 2 )
            {
                encoding = Encoding.GetEncoding("windows-1254");
            }
            if(siteID == 3)
            {
                encoding = Encoding.UTF8;
            }


            CrawlConfiguration crawlConfig = new CrawlConfiguration();
            crawlConfig.DownloadableContentTypes = @"text/html, text/plain";
            crawlConfig.IsExternalPageCrawlingEnabled = false;
            crawlConfig.IsExternalPageLinksCrawlingEnabled = false;
            crawlConfig.IsUriRecrawlingEnabled = false;
            crawlConfig.CrawlTimeoutSeconds = 0;
            crawlConfig.MaxConcurrentThreads = 100;
            crawlConfig.MaxPagesToCrawl = 100000000;
            crawlConfig.UserAgentString = "abot v1.0 http://code.google.com/p/abot";

            crawlConfig.Encoding = encoding;

            PoliteWebCrawler crawler = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            CrawlResult result = crawler.Crawl(new Uri(siteConfiguration.SiteURL));
           
        }

        static void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
        }

        static void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;


            if (crawledPage.WebException == null && crawledPage.HttpWebResponse.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(crawledPage.RawContent))
            {
                string htmlDoc = crawledPage.RawContent;
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlDoc);


                if(siteConfiguration.SiteID == 1)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//article[@id='news-content-contain']");
                    if(node != null)
                    {
                        lock(locker)
                        {
                            string fileName = siteConfiguration.DownloadPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Guid();
                            using (StreamWriter sw = new StreamWriter( fileName))
                            {
                                sw.WriteLine( e.CrawledPage.Uri);
                                sw.Write(node.InnerHtml);
                            }
                        }
                    }
                }

                if (siteConfiguration.SiteID == 2)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='hurLeft']");
                    if (node != null)
                    {
                        lock (locker)
                        {
                            string fileName = siteConfiguration.DownloadPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Guid();
                            using (StreamWriter sw = new StreamWriter(fileName))
                            {
                                sw.WriteLine(e.CrawledPage.Uri);
                                sw.Write(node.InnerHtml);
                            }
                        }
                    }
                }

                if (siteConfiguration.SiteID == 3)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='content']");
                    if (node != null)
                    {
                        lock (locker)
                        {
                            string fileName = siteConfiguration.DownloadPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Guid();
                            using (StreamWriter sw = new StreamWriter(fileName))
                            {
                                sw.WriteLine(e.CrawledPage.Uri);
                                sw.Write(node.InnerHtml);
                            }
                        }
                    }
                }


            }

        }

        static void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
        }

        static void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
        }
    }
}
