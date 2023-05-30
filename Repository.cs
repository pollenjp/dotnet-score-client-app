using System.Text.Json.Serialization;

namespace ScoreClient
{
    public record class Repository([property: JsonPropertyName("name")] string Name);
    public record class ScoresResponse(List<ScoreResponse> Json);
    public record class ScoreResponse(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("value")] int Value
    );
}
