using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Bll.Interfaces;
using PhotoAlbum.Bll.Services;
using PhotoAlbum.Dal;
using PhotoAlbum.Dal.Interfaces;
using PhotoAlbum.Dal.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://d5khmk-cloud-lab.vercel.app"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var blobConnectionString = builder.Configuration.GetConnectionString("BlobStorage");
builder.Services.AddSingleton(new BlobServiceClient(blobConnectionString));

builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
