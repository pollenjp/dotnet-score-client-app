using System.Net.Http.Headers;
using System.Text.Json;

namespace ScoreClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            await ProcessRepositoriesAsync(client);
        }

        private static async Task ProcessRepositoriesAsync(HttpClient client)
        {
            // var json = await client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            // Console.Write(json);

            await using Stream stream = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(stream);
            foreach (var repo in repositories ?? Enumerable.Empty<Repository>())
            {
                Console.Write(repo.name);
            }

        }
    }
}
