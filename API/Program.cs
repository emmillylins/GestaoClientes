using Application.Clientes.Criar;
using Application.Clientes.Obter;
using Application.Clientes.Listar;
using Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1",
        Description = "API para gerenciamento de clientes com CNPJ"
    });
});

builder.Services.AddScoped<CriaClienteCommandHandler>();
builder.Services.AddScoped<ObtemClientePorIdQueryHandler>();
builder.Services.AddScoped<ListarClientesQueryHandler>();

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = "swagger";
    });
    
    // Middleware para debug de erro
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Log de inicialização
Console.WriteLine("Banco em memória configurado");

app.Run();
