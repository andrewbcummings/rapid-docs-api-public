using Microsoft.Extensions.Options;
using RestSharp;
using rapid_docs_core.ApplicationSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace rapid_docs_services.Services.TextControl
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly TextControlSettings _textControlSettings;
        public ThumbnailService(IOptions<TextControlSettings> textControlSettings) {
            _textControlSettings = textControlSettings.Value;
        }
        public async Task<string[]> GetThumbnail(string base64Document)
        {
            var body = base64Document;
            var client = new RestClient(_textControlSettings.BaseUrl + "documentprocessing/document/thumbnails?imageFormat=PNG&fromPage=1&toPage=1&zoomFactor=30");

            var request = new RestRequest()          
                {
                    Method = Method.Post
                }
            .AddHeader("Content-Type", "application/json")
            .AddBody(body);

            RestResponse response = await client.ExecutePostAsync(request);
            if(response.Content != null)
            {
                var thumbnails = JsonSerializer.Deserialize<string[]>(response.Content);
                return thumbnails ?? new string[] { };
            }
            return new string[] { };
        }
    }
}
