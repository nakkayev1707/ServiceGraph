using ServiceGraph.Core;
using ServiceGraph.Extensions;
using ServiceGraphUsage.Services;
using ServiceGraphUsage.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceA, ServiceA>();
builder.Services.AddScoped<IServiceB, ServiceB>();
builder.Services.AddScoped<IServiceC, ServiceC>();
builder.Services.AddScoped<IServiceD, ServiceD>();
builder.Services.AddScoped<IServiceE, ServiceE>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseServiceGraphUI(builder.Services, new ServiceGraphOption
    {
        Namespaces = new []
        {
            "*"
        }
    });
}

app.UseHttpsRedirection();

app.Run();