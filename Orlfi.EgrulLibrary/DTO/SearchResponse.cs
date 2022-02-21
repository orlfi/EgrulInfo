using System.Text.Json.Serialization;

namespace Orlfi.EgrulLibrary.DTO;

public class SearchResponse
{
    [JsonPropertyName("rows")]
    public ICollection<CompanyInfo> Rows { get; init; } = new List<CompanyInfo>();
}
