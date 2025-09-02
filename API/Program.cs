using Application.Clientes.Criar;
using Application.Clientes.Obter;
using Application.Clientes.Listar;
using Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1",
        Description = "API para gerenciamento de clientes com CNPJ"
    });
});

// Application services (handlers) - simples, sem MediatR
builder.Services.AddScoped<CriaClienteCommandHandler>();
builder.Services.AddScoped<ObtemClientePorIdQueryHandler>();
builder.Services.AddScoped<ListarClientesQueryHandler>();

// Infra
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
