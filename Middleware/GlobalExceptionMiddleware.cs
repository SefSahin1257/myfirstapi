using Microsoft.AspNetCore.Mvc;
public class GlobalExceptionMiddleware {
  private readonly RequestDelegate _next;
  private readonly ILogger<GlobalExceptionMiddleware> _logger;  

  public GlobalExceptionMiddleware (RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger){
   _next = next;
   _logger = logger;
 }
  public async Task InvokeAsync(HttpContext context){
  try
  {
   await _next(context);
  }
  catch(Exception ex)
  {
   _logger.LogError(ex,"Beklenmeyen bir hata oluştu.");
   
   var statusCode = ex switch
   {
    ArgumentException => StatusCodes.Status400BadRequest,
    NotFoundException => StatusCodes.Status404NotFound,
    ConflictException => StatusCodes.Status409Conflict,
    _ => StatusCodes.Status500InternalServerError
   };
   
   context.Response.StatusCode = statusCode;
   context.Response.ContentType = "application/json";

   var problem = new ProblemDetails{
    Status = statusCode,
    Title = statusCode switch{
      StatusCodes.Status400BadRequest => "Bad Request",
      StatusCodes.Status404NotFound => "Not Found",
      StatusCodes.Status409Conflict => "Conflict",
      _ => "Internal Server Error"
     },
    Detail = statusCode == StatusCodes.Status500InternalServerError
 	? "Beklenmeyen bir sunucu hatası oluştu"
	: ex.Message,
    Instance = context.Request.Path
   };
   
   await context.Response.WriteAsJsonAsync(problem);
  }
 }
}
