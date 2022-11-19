using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Db;
using ShopOnline.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
builder.Services.RegisterServices();
builder.Services.AddSingleton(typeof(IAppServiceScopeFactory<>), typeof(AppServiceScopeFactory<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(opts => opts
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}
else
{
    app.UseCors(opts => opts
        .WithOrigins("https://localhost:7131", "http://localhost:7131")
        .AllowAnyMethod()
        .AllowAnyHeader());
}


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
