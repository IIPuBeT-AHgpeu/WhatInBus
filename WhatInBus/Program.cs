using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WhatInBus.Croppers;
using WhatInBus.Database;
using WhatInBus.FileManagement;
using WhatInBus.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PfHistoryContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddTransient<IRepository<Recognize>, Repository<Recognize>>();
builder.Services.AddTransient<ICropper<Rectangle>, Cropper>();
builder.Services.AddTransient<IFileManager<ImageInDataset>, ImageFileManager>();

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
