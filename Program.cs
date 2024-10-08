using ShoppingAPI.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("KeleShopping")));

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://alectosinterdimensionalblog.com", "https://www.alectosinterdimensionalblog.com", "https://localhost:4200", "http://localhost:4200"));


app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();
app.MapFallbackToController("Index", "Fallback");


app.Run();
