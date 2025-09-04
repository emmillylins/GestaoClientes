using Application.Clientes.Criar;
using Application.Clientes.Obter;
using Application.Clientes.Listar;
using Application.Clientes.Atualizar;
using Application.Clientes.Atualizar.Ativar;
using Application.Clientes.Atualizar.Desativar;
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
builder.Services.AddScoped<AtivarClienteCommandHandler>();
builder.Services.AddScoped<DesativarClienteCommandHandler>();
builder.Services.AddScoped<AtualizarClienteCommandHandler>();

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

app.Run();
