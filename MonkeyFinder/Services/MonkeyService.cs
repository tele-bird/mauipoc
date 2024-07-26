using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    List<Monkey> monkeyList = new();
    HttpClient httpClient;

    public MonkeyService()
    {
        this.httpClient = new();
    }

    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList.Count > 0)
        {
            await Task.Delay(1000);
            return monkeyList;
        }

        var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

        if(response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }

        return monkeyList;
    }
}
