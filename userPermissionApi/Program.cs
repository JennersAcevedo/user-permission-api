using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using userPermissionApi.Data;
using userPermissionApi.Repositories;
using userPermissionApi.Middlewares;
using Elastic.Clients.Elasticsearch;
using userPermissionApi.CQRS.commands;
using userPermissionApi.Models;
using Elastic.Transport;

var builder = WebApplication.CreateBuilder(args);


var elasticConfig = builder.Configuration.GetSection("Elasticsearch");
var cloudId = elasticConfig["CloudID"];
var apiKey = elasticConfig["ApiKey"];

    // Configurar Elasticsearch Cloud usando el CloudId y apikey
var elasticClient = new ElasticsearchClient(cloudId, new ApiKey(apiKey));

    // Registrar Elasticsearch en el contenedor de dependencias
builder.Services.AddSingleton(elasticClient);

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

//  Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Registrar MediatR (Solo una vez)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreatePermisoCommand>());

//  Inyección de dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<Permiso, int>, Repository<Permiso, int>>();
builder.Services.AddScoped<IRepository<TipoPermiso, int>, Repository<TipoPermiso, int>>();

//  Configuración de la base de datos
builder.Services.AddDbContext<N5dbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("stringSQL"));
});

//  Configurar JSON para evitar ciclos en la serialización
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
//  Usar middleware de Logging para el manejo de Elastic search Cloud
app.UseMiddleware<LoggingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
