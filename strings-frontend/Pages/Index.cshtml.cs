﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace strings_frontend.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IHttpClientFactory _clientFactory;
        
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string StringInput { get; set;}

        public StringResult StringResult { get; set;}

        public string Message { get; set;  }
        
        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            StringResult = new StringResult();
            Message = string.Empty;
        }

        // public void OnGet()
        // {
        //     var test = StringResult;
        // }

        public async Task OnGetAsync(string input){
            Message = string.Empty;
            if (string.IsNullOrEmpty(input)) {
                return;
            }             

            var client = _clientFactory.CreateClient();
            _logger.LogInformation("\n...Retrieving Data...\n");

            var request = new System.Net.Http.HttpRequestMessage();
            // var query = $"http://strings-api/stringlength/{input}";
            var query = $"http://strings-api:8088/stringlength/{input}";

            //var query = $"http://localhost:8080/stringlength/{input}";
            request.RequestUri = new Uri(query);
            
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Message = "Error. Received " + response.StatusCode.ToString();
            } else
            {
                var jsonResult = await response.Content.ReadAsStringAsync();

                StringResult = JsonSerializer.Deserialize<StringResult>(
                        jsonResult,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                );

                Message = "Request Success";
                _logger.LogInformation("\n...Finished Retrieving Data...\n");

            }
        }  
    }

    public class StringResult {
        
        public string Input { get; set; }
        public int Length { get; set; }

        public bool CacheHit { get; set; }
    }
}
