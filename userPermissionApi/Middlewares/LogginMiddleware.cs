using Elastic.Clients.Elasticsearch;
using System.Text;
using System.Text.Json;
using userPermissionApi.Models;


namespace userPermissionApi.Middlewares
{
    public class LoggingMiddleware
    {
     
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly ElasticsearchClient _elasticClient;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, ElasticsearchClient elasticClient)
        {
            _next = next;
            _logger = logger;
            _elasticClient = elasticClient;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            var timestamp = DateTime.UtcNow;

            string requestBody = string.Empty;
            string responseBody = string.Empty;

            //  Leer el Body solo en POST y PUT (Crear y Editar)
            if (method == HttpMethods.Post || method == HttpMethods.Put)
            {
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; 
                }
            }

            // Interceptar el Response.Body
            var originalResponseBody = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                // Pasar al siguiente middleware/controlador
                await _next(context);

                // Reiniciar el stream antes de leerlo
                memoryStream.Position = 0;

                // Copiar el stream sin cerrarlo
                responseBody = await new StreamReader(memoryStream, Encoding.UTF8).ReadToEndAsync();

                // Copiar la respuesta de nuevo al Response.Body original
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalResponseBody);
            }

            Permiso permiso = null;

            try
            {
                permiso = JsonSerializer.Deserialize<Permiso>(responseBody);
            }
            catch (JsonException ex)
            {
                _logger.LogError("Error al deserializar la respuesta: {Error}", ex.Message);
            }
            if (permiso != null && permiso.id > 0)
            {
             
                //Enviar a ElasticSearch           
                var indexResponse = await _elasticClient.IndexAsync(permiso, idx => idx.Index("permission"));

                if (!indexResponse.IsValidResponse)
                {
                    _logger.LogError("Error al enviar log a Elasticsearch: {Error}", indexResponse.DebugInformation);
                }

                // Registrar en logs locales
                _logger.LogInformation("Log enviado a Elasticsearch: {Method} {Path} | Status: {StatusCode}", method, path, context.Response.StatusCode);
            }
            else
            {
                _logger.LogWarning("El permiso no tiene un id válido o no se pudo deserializar.");
            }
          

        }

    }
}
