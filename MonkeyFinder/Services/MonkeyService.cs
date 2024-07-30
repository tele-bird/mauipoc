using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;

    public MonkeyService()
    {
        this.httpClient = new();
    }

    public async Task<List<Monkey>> GetMonkeys(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json", cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Monkey>>(cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
    }
}
