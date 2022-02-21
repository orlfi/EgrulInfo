using System.Text.Json.Serialization;

namespace Orlfi.EgrulLibrary.DTO;

public class SearchToken
{
    [JsonPropertyName("T")]
    public string Token { get; init; } = string.Empty;
    public bool CaptchaRequired { get; init; }
}
