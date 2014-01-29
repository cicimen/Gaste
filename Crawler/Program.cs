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
        static char seperator;
        static object locker;
        static SubdomainConfiguration subdomainConfiguration;
        static Encoding encoding;
        static Dictionary<string, string> urlFilePath;
        static Dictionary<string, string> urlImageLink;
        static int count;
        static void Main(string[] args)
        {

            //for (int i = 0; i < 1000000; ++i)
            //{
            //    Console.Write("\r{0}%   ", i);
            //}


            urlFilePath     = new Dictionary<string, string>();
            urlImageLink    = new Dictionary<string, string>();
            count           = 0;
            seperator       = (char)007;
            locker          = new object();
            //args          = new string[1];
            //args[0]       = "1";
            int subdomainID      = 0;
            if (args.Length != 1 || string.IsNullOrWhiteSpace(args[0]) || !int.TryParse(args[0], out subdomainID))
            {
                return;
            }

            subdomainConfiguration = Configuration.GetSubdomainConfiguration(subdomainID);

            if (subdomainID == 1 || subdomainID == 2)
            {
                encoding = Encoding.GetEncoding("windows-1254");
            }
            if (subdomainID == 3)
            {
                encoding = Encoding.UTF8;
            }


            CrawlConfiguration crawlConfig                 = new CrawlConfiguration();
            crawlConfig.DownloadableContentTypes           = @"text/html, text/plain";
            crawlConfig.IsExternalPageCrawlingEnabled      = false;
            crawlConfig.IsExternalPageLinksCrawlingEnabled = false;
            crawlConfig.IsUriRecrawlingEnabled             = false;
            crawlConfig.CrawlTimeoutSeconds                = 0;
            crawlConfig.MaxConcurrentThreads               = 100;
            crawlConfig.MaxPagesToCrawl                    = 100000000;
            crawlConfig.UserAgentString                    = "abot v1.0 http://code.google.com/p/abot";


            crawlConfig.Encoding = encoding;

            PoliteWebCrawler crawler = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            CrawlResult result = crawler.Crawl(new Uri(subdomainConfiguration.SubdomainURL));
           
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

                #region
                //foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                // {
                //     string href = link.GetAttributeValue("href", null).Trim();
                //     lock (locker)
                //     {
                //         if (!urlImageLink.ContainsKey(href))
                //         {
                //             HtmlNode img = link.SelectSingleNode("//img[@src]");
                //             if (img != null)
                //             {
                //                 string imgSrc = img.GetAttributeValue("src", null).Trim();
                //                 if(siteConfiguration.SiteID == 1)
                //                 {
                //                     if (!imgSrc.Contains("http://i.radikal.com.tr"))
                //                    {
                //                        continue;
                //                    }
                //                 }
                //                 if (!string.IsNullOrWhiteSpace(imgSrc))
                //                 {
                //                     using (StreamWriter sw = new StreamWriter(siteConfiguration.URLImageLink, true))
                //                     {
                //                         sw.WriteLine(href + seperator + imgSrc);
                //                     }
                //                     urlImageLink.Add(href, "");
                //                 }
                //             }
                //         }
                //     }
                // }
                #endregion

                if (subdomainConfiguration.SubdomainID == 1)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//article[@id='news-content-contain']");
                    if(node != null)
                    {
                        lock(locker)
                        {
                            string guid =  Guid.NewGuid().ToString();
                            string fileName = subdomainConfiguration.DownloadPath + "\\" + guid;
                            if (!urlFilePath.ContainsKey(e.CrawledPage.Uri.ToString()))
                            {
                                urlFilePath.Add(e.CrawledPage.Uri.ToString(), guid);
                                using (StreamWriter sw = new StreamWriter(fileName, true))
                                {
                                    sw.Write(node.OuterHtml);
                                }
                                using (StreamWriter sw = new StreamWriter(subdomainConfiguration.URLFilePath, true))
                                {
                                    sw.WriteLine(e.CrawledPage.Uri.ToString().Trim() + seperator + guid);
                                }
                                count++;
                                Console.WriteLine(count);
                            }
                        }
                    }
                }

                if (subdomainConfiguration.SubdomainID == 2)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='hurLeft']");
                    if (node != null)
                    {
                        lock (locker)
                        {
                            string guid = Guid.NewGuid().ToString();
                            string fileName = subdomainConfiguration.DownloadPath + "\\" + guid;
                            if (!urlFilePath.ContainsKey(e.CrawledPage.Uri.ToString()))
                            {
                                urlFilePath.Add(e.CrawledPage.Uri.ToString(), guid);
                                using (StreamWriter sw = new StreamWriter(fileName, true))
                                {
                                    sw.Write(node.OuterHtml);
                                }
                                using (StreamWriter sw = new StreamWriter(subdomainConfiguration.URLFilePath, true))
                                {
                                    sw.WriteLine(e.CrawledPage.Uri.ToString().Trim() + seperator + guid);
                                }
                                count++;
                                Console.WriteLine(count);
                            }
                        }
                    }
                }

                if (subdomainConfiguration.SubdomainID == 3)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='content']");
                    if (node != null)
                    {
                        lock (locker)
                        {
                            string guid = Guid.NewGuid().ToString();
                            string fileName = subdomainConfiguration.DownloadPath + "\\" + guid;
                            if (!urlFilePath.ContainsKey(e.CrawledPage.Uri.ToString()))
                            {
                                urlFilePath.Add(e.CrawledPage.Uri.ToString(), guid);
                                using (StreamWriter sw = new StreamWriter(fileName, true))
                                {
                                    sw.Write(node.OuterHtml);
                                }
                                using (StreamWriter sw = new StreamWriter(subdomainConfiguration.URLFilePath, true))
                                {
                                    sw.WriteLine(e.CrawledPage.Uri.ToString().Trim() + seperator + guid);
                                }
                                count++;
                                Console.WriteLine(count);
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
