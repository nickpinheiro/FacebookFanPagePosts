using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.IO;
using System.Web;
using System.Net.Http;
using System.Configuration;

namespace FacebookFanPagePosts
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            //var host = new JobHost();
            //// The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();

            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            // Use HttpClient
            using (var client = new HttpClient())
            {
                // Set variable values for post to facebook
                string uri = "https://graph.facebook.com/v2.5/" + ConfigurationManager.AppSettings["FacebookPageId"] + "/feed?";
                string accessToken = ConfigurationManager.AppSettings["FacebookAccessToken"];
                string link = "http://www.msdevz.com/news/article.aspx?id=5899&o=3";
                string picture = "http://news.kuwaittimes.net/website/wp-content/uploads/2015/11/microsoft.jpg";
                string name = "Microsoft Cloud Roadshow 2016 leads towards cloud innovation ";
                string description = "KT: Tell us something more about Microsoft Azure and new projects. Phillips: We recently introduced Azure, which marries our business intelligence capabilities and data visualization with learning in advance analytics with the ability to create applications.";

                // Formulate querystring for graph post
                StringContent queryString = new StringContent("access_token=" + accessToken + "&link=" + link + "&picture=" + picture + "&name=" + name + "&description=" + description);

                // Post to facebook /{page-id}/feed edge
                HttpResponseMessage response = await client.PostAsync(new Uri(uri), queryString);
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    string postId = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(postId);
                    Console.ReadLine();
                }
            }
        }
    }
}
