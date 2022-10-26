using Microsoft.EntityFrameworkCore;
using OohelpWebApps.Boards.Configurations.Downloading;
using OohelpWebApps.Boards.Database;
using OohelpWebApps.Boards.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionString");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //кодирование кириллицы (иначе на выходе строка в UTF8)
        //options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);

        //Сериализация типа Enum в строку
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        //Сериализация типа DateOnly
        //options.JsonSerializerOptions.Converters.Add(new JsonDateOnlyConverter());

        //запись форматированного JSON 
        //options.JsonSerializerOptions.WriteIndented = true;

        //сбрасываем в ноль формат имен свойств (по умолчанию CamelCase, вида "myData")
        options.JsonSerializerOptions.PropertyNamingPolicy = null;

        //игнорировать свойства с нулевыми значениями
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Добавляем сервисы api
builder.Services.AddScoped<GridsService>();
builder.Services.AddSingleton<DownloadConfigurationBuilder>();





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
