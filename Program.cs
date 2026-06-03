using Microsoft.AspNetCore.Html;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var name = "Fernando";

app.MapGet("/", () => {

    var htmlContent = $@"
    <html>
    <head>
        <style>
            body {{
                background-color: #121212; /* Fundo grafite escuro */
                color: #ffffff;            /* Texto branco */
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                padding: 40px;
            }}
            h1 {{
                color: #4caf50;            /* Título em verde (cor de dinheiro!) */
            }}
        </style>
    </head>
    <body>
        <h1>Olá, {name}!</h1>
        <p>Seja bem-vindo ao seu monitor de moedas.</p>
    </body>
    </html>
";

    return Results.Content(htmlContent, "text/html", System.Text.Encoding.UTF8);
});

app.Run();
