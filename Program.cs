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
            await GetScoresAsync(client);
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
                Console.Write($"{repo.Id} {repo.Username} {repo.Value}");
            }
        }
    }
}
