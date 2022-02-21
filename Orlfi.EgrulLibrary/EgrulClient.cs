using System.Net.Http.Json;
using Orlfi.EgrulLibrary.DTO;

namespace Orlfi.EgrulLibrary;

public class EgrulClient
{
    private const string path = "https://egrul.nalog.ru";
    private const int requestDelay = 200;
    private readonly HttpClient _client;

    public EgrulClient(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(path);
    }

    public async Task<CompanyInfo?> SearchAsync(string inn, CancellationToken cancel = default)
    {
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("vyp3CaptchaToken", ""),
            new KeyValuePair<string, string>("page", ""),
            new KeyValuePair<string, string>("query", inn),
            new KeyValuePair<string, string>("region", ""),
            new KeyValuePair<string, string>("PreventChromeAutocomplete", ""),
        });

        SearchToken? searchToken;
        using HttpResponseMessage tokenMessage = await _client.PostAsync("/", formContent, cancel).ConfigureAwait(false);
        {
            searchToken = await tokenMessage.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<SearchToken>().ConfigureAwait(false);
        }

        var searchResult = await _client.GetFromJsonAsync<SearchResponse>($"search-result/{searchToken?.Token}", cancel).ConfigureAwait(false);

        if (searchResult is null || searchResult?.Rows.Count == 0)
            throw new NullReferenceException("INN not found");

        if (searchResult?.Rows.Count == 0)
            throw new NullReferenceException("Several organizations found");

        return searchResult?.Rows.First();
    }

    public async Task<byte[]> GetFileAsync(string inn, CancellationToken cancel = default)
    {
        using var stream = await GetFileStreamAsync(inn, cancel).ConfigureAwait(false);
        using MemoryStream ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancel).ConfigureAwait(false); ;
        return ms.ToArray();
    }

    public async Task DownloadFileAsync(string inn, string fileName, CancellationToken cancel = default)
    {
        using var stream = await GetFileStreamAsync(inn, cancel).ConfigureAwait(false);
        using FileStream fs = new FileStream(fileName, FileMode.Create);

        byte[] buffer = new byte[16384];
        int readed = 0;
        while ((readed = await stream.ReadAsync(buffer, 0, buffer.Length, cancel).ConfigureAwait(false)) > 0)
        {
            await fs.WriteAsync(buffer, 0, readed, cancel).ConfigureAwait(false);
        }

        await fs.FlushAsync();
    }

    public async Task<Stream> GetFileStreamAsync(string inn, CancellationToken cancel = default)
    {
        var info = await SearchAsync(inn, cancel);
        if (info is null)
            throw new NullReferenceException("INN not found");

        await _client.GetAsync($"vyp-request/{info.Token}").ConfigureAwait(false);
        await Task.Delay(requestDelay);
        while (true)
        {
            var statusResponse = await _client.GetFromJsonAsync<StatusResponse>($"vyp-status/{info?.Token}", cancel).ConfigureAwait(false);

            if (statusResponse?.Status == "ready")
                break;

            await Task.Delay(requestDelay);
        }

        HttpResponseMessage fileMessage = await _client.GetAsync(
            $"vyp-download/{info?.Token}",
            HttpCompletionOption.ResponseHeadersRead,
            cancel
        ).ConfigureAwait(false);

        return await fileMessage.EnsureSuccessStatusCode().Content.ReadAsStreamAsync(cancel).ConfigureAwait(false);
    }
}
