using Microsoft.AspNetCore.Html;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var name = "Fernando";

app.MapGet("/", async () => 
{
    using var client = new HttpClient();

    client.DefaultRequestHeaders.Add("User-Agent", "C# Monitor Application");
    
    // Valores padrão caso ocorra algum problema de carregamento
    string dolar = "Carregando...";
    string euro = "Carregando...";
    string libra = "Carregando...";
    string iene = "Carregando...";
    string yuan = "Carregando...";

    try
    {
        // Adicionamos o CNY-BRL no final da URL da API
        string url = "https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL,GBP-BRL,JPY-BRL,CNY-BRL";
        
        // Busca os dados da internet de forma assíncrona
        string response = await client.GetStringAsync(url);

        // Converte o JSON para o nosso objeto C#
        var dados = JsonSerializer.Deserialize<ApiResponse>(response);

        var culturaAmericana = System.Globalization.CultureInfo.InvariantCulture;

        // Formata os valores. O Iene (JPY) costuma valer frações de centavos, por isso mantemos 4 casas decimais.
        dolar = double.Parse(dados.USDBRL.Bid, culturaAmericana).ToString("N2");
        euro = double.Parse(dados.EURBRL.Bid, culturaAmericana).ToString("N2");
        libra = double.Parse(dados.GBPBRL.Bid, culturaAmericana).ToString("N2");
        iene = double.Parse(dados.JPYBRL.Bid, culturaAmericana).ToString("N4");
        yuan = double.Parse(dados.CNYBRL.Bid, culturaAmericana).ToString("N2");
    }
    catch (Exception ex)
    {
        Console.WriteLine("================ ERRO REAL ================");
        Console.WriteLine(ex.ToString());
        Console.WriteLine("===========================================");

        dolar = euro = libra = iene = yuan = "Erro ao carregar";
    }

    // Montagem do HTML com a lista na ordem solicitada
    var htmlContent = $@"
        <html>
        <head>
            <meta charset='utf-8'>
            <style>
                body {{
                    background-color: #121212;
                    color: #ffffff;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    padding: 40px;
                }}
                h1 {{
                    color: #4caf50;
                }}
                ul {{
                    list-style-type: none;
                    padding: 0;
                }}
                li {{
                    font-size: 1.2rem;
                    margin-bottom: 12px;
                    padding: 12px 20px;
                    background-color: #1e1e1e;
                    border-radius: 6px;
                    max-width: 450px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.2);
                }}
                .moeda {{
                    font-weight: bold;
                    color: #4caf50;
                }}
            </style>
        </head>
        <body>
            <h1>Olá, {name}!</h1>
            <p>Seja bem-vindo ao seu monitor de moedas:</p>
            
            <ul>
                <li><span class='moeda'>🇧🇷 Real (BRL):</span> R$ 1,00 (Base)</li>
                <li><span class='moeda'>🇺🇸 Dólar (USD):</span> R$ {dolar}</li>
                <li><span class='moeda'>🇪🇺 Euro (EUR):</span> R$ {euro}</li>
                <li><span class='moeda'>🇬🇧 Libra Esterlina (GBP):</span> R$ {libra}</li>
                <li><span class='moeda'>🇯🇵 Iene Japonês (JPY):</span> R$ {iene}</li>
                <li><span class='moeda'>🇨🇳 Yuan Chinês (CNY):</span> R$ {yuan}</li>
            </ul>
        </body>
        </html>
    ";

    return Results.Content(htmlContent, "text/html", System.Text.Encoding.UTF8);
});

app.Run();

public class ApiResponse
{
    [JsonPropertyName("USDBRL")] public MoedaData USDBRL { get; set; }
    [JsonPropertyName("EURBRL")] public MoedaData EURBRL { get; set; }
    [JsonPropertyName("GBPBRL")] public MoedaData GBPBRL { get; set; }
    [JsonPropertyName("JPYBRL")] public MoedaData JPYBRL { get; set; }
    [JsonPropertyName("CNYBRL")] public MoedaData CNYBRL { get; set; }
}

public class MoedaData
{
    [JsonPropertyName("bid")] public string Bid { get; set; }
}