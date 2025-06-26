namespace comebackapi.Middlewaress;

public class ExepctionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExepctionMiddleware> _logger;

    public ExepctionMiddleware(
        RequestDelegate next,
        ILogger<ExepctionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Исключение въебенил: {ex.Message}");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync("Error was occured");
        }
    }
}