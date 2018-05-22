﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using UIC.Util.Logging;

namespace HAW.AWS.CommunicationAgent.RESTClient
{
    static class RESTClient
    {
        private static String buffer = "";
        private static int msgID = 0;

     
public static async Task PushAsync(String msg, ILogger logger)
        {
            try
            {
                HttpClient client = new HttpClient();
                // Update port # in the following line.
                client.BaseAddress = new Uri("http://localhost:8080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"rest/iot/legacy/" + msg);
                msgID++;
                response.EnsureSuccessStatusCode();
                logger.Information("[DEBUG] pushing to:" + "rest/iot/legacy" + msg);
                buffer = "";
            }
            catch
            {
                logger.Error("REST Connection failed");
                buffer = buffer + msg;
            }

        }
    





public static  void  Initialize(String serialID,String msg, ILogger logger)

        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    String url = "http://localhost:8080/";
                    String path = "/rest/iot/init";
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        var content = new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("serialid", serialID),
                        new KeyValuePair<string, string>("edms", msg)

                    });
                        var response = client.PostAsync(path, content).Result;
                        string resultContent = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(resultContent);
                        response.EnsureSuccessStatusCode();
                        logger.Information("[DEBUG] Initialize on:" + client.BaseAddress + path + " with content: " + msg);
                        return;
                    }
                }
                catch
                {
                    logger.Error("Initialize failed on try number: " + i);
                    Thread.Sleep(3000);
                }

            }
            logger.Error("Initialize failed finally");
        }
    }
}  

