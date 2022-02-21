using System.Text.Json.Serialization;
using Orlfi.EgrulLibrary.JsonConverters;

namespace Orlfi.EgrulLibrary.DTO;

public class CompanyInfo
{
    [JsonPropertyName("a")]
    public string Address { get; init; } = string.Empty;

    [JsonPropertyName("c")]
    public string ShortName { get; init; } = string.Empty;

    [JsonPropertyName("n")]
    public string FullName { get; init; } = string.Empty;

    [JsonPropertyName("g")]
    public string Manager { get; init; } = string.Empty;

    [JsonPropertyName("cnt")]
    [JsonConverter(typeof(IntConverter))]
    public int Count { get; init; }

    [JsonPropertyName("i")]
    public string Inn { get; init; } = string.Empty;

    [JsonPropertyName("k")]
    public string CompanyType { get; init; } = string.Empty;

    [JsonPropertyName("o")]
    public string Ogrn { get; init; } = string.Empty;

    [JsonPropertyName("p")]
    public string Kpp { get; init; } = string.Empty;

    [JsonPropertyName("r")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime RegistrationDate { get; init; }

    [JsonPropertyName("t")]
    public string Token { get; init; } = string.Empty;

    [JsonPropertyName("pg")]
    [JsonConverter(typeof(IntConverter))]
    public int Page { get; init; }

    [JsonPropertyName("tot")]
    [JsonConverter(typeof(IntConverter))]
    public int Total { get; init; }
}