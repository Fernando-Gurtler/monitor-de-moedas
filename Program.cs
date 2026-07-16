using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => 
{
    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("User-Agent", "C# Monitor Application");
    
    string dolar = "Erro", euro = "Erro", libra = "Erro", iene = "Erro", yuan = "Erro";

    try
    {
        string url = "https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL,GBP-BRL,JPY-BRL,CNY-BRL";
        string response = await client.GetStringAsync(url);
        var dados = JsonSerializer.Deserialize<ApiResponse>(response);

        var culturaApi = System.Globalization.CultureInfo.InvariantCulture;

        dolar = double.Parse(dados.USDBRL.Bid, culturaApi).ToString("N2");
        euro = double.Parse(dados.EURBRL.Bid, culturaApi).ToString("N2");
        libra = double.Parse(dados.GBPBRL.Bid, culturaApi).ToString("N2");
        iene = double.Parse(dados.JPYBRL.Bid, culturaApi).ToString("N4");
        yuan = double.Parse(dados.CNYBRL.Bid, culturaApi).ToString("N2");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro na API: {ex.Message}");
    }

    string htmlTemplate = await File.ReadAllTextAsync("wwwroot/index.html");

    string htmlFinal = htmlTemplate
        .Replace("{{Dolar}}", dolar)
        .Replace("{{Euro}}", euro)
        .Replace("{{Libra}}", libra)
        .Replace("{{Iene}}", iene)
        .Replace("{{Yuan}}", yuan);

    return Results.Content(htmlFinal, "text/html", System.Text.Encoding.UTF8);
});

app.Run();
