using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ScoreClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Score Client App");

            await PostScoreAsync();

            await GetScoresAsync(client);
        }
        public static async Task<ScoreResponse?> PostScoreAsync()
        {
            using (HttpClient client = new())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8000/scores");

                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Charset", "utf-8");

                var scoreReq = new ScoreRequest("test", 100);
                string reqBodyJson = JsonSerializer.Serialize(scoreReq);
                var content = new StringContent(reqBodyJson, Encoding.UTF8, @"application/json");

                request.Content = content;

                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var score = await JsonSerializer.DeserializeAsync<ScoreResponse>(stream);
                    if (score != null)
                    {
                        Console.Write($"ID: {score.Id}, Username: {score.Username}, Value: {score.Value}");
                    }
                    return score;
                }
                else
                {
                    // Handle the situation when the request fails, e.g., throw an exception, return null, etc.
                    // throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
                    return null;
                }
            }
        }


        private static async Task GetScoresAsync(HttpClient client)
        {

            var url = "http://localhost:8000";
            var path = "/scores";
            var requestEndPoint = url + path;

            await using Stream stream = await client.GetStreamAsync(requestEndPoint);
            Encoding encoding = Encoding.GetEncoding("utf-8");

            var repositories = await JsonSerializer.DeserializeAsync<List<ScoreResponse>>(stream);
            foreach (var repo in repositories ?? Enumerable.Empty<ScoreResponse>())
            {
                Console.Write($"ID: {repo.Id}, Username: {repo.Username}, Value: {repo.Value}");
            }
        }
    }
}
