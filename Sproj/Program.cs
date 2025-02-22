using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

const string Login = "janetnil2003@gmail.com";
const string Password = "12345";

app.MapPost("/login", async (HttpContext context) =>
{
    try
    {
        var user = await JsonSerializer.DeserializeAsync<User>(context.Request.Body);

        if (user == null || string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { success = false, message = "Введите логин и пароль!" }));
            return;
        }

        if (user.Login == Login && user.Password == Password)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { success = true, redirect = "/newpage" }));
            return;
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { success = false, message = "Неверный логин или пароль!" }));
    }
    catch (Exception)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { success = false, message = "Ошибка сервера" }));
    }
});

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/login.html");
});

app.MapGet("/newpage", (HttpContext context) =>
{
    context.Response.Redirect("https://docs.google.com/spreadsheets/d/1bxKKcsxhdPOs-nwUzk4a3ZepsuIY3_m1tbfycVO_KT4/edit?usp=sharing");
    return Task.CompletedTask;
});

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/login.html");
});

app.Run();

class User
{
    [JsonPropertyName("login")]
    public string Login { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}
