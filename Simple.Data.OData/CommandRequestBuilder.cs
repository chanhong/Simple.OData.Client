﻿using System.Net;
using Simple.NExtLib;

namespace Simple.Data.OData
{
    public class CommandRequestBuilder : RequestBuilder
    {
        public CommandRequestBuilder(string urlBase)
            : base(urlBase)
        {
        }

        public override void AddCommand(string command, string method, string content = null)
        {
            var uri = CreateRequestUrl(command);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentLength = (content ?? string.Empty).Length;

            // TODO: revise
            //if (method == "PUT" || method == "DELETE" || method == "MERGE")
            //{
            //    request.Headers.Add("If-Match", "*");
            //}

            if (content != null)
            {
                request.ContentType = "application/atom+xml";
                request.SetContent(content);
            }

            this.Request = request;
        }
    }
}