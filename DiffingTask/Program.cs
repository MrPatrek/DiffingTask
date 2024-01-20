using DiffingTask;
using DiffingTask.ActionFilters;
using DiffingTask.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));





// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;     // disable default model state validation ('cause we will have our custom one)
                                                        // i.e., disable automatic HTTP 400 – Bad Request responses
                                                        // (e.g., when we send empty body request to the API when it is expected to be non-empty)
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;       // without this, we would still return JSON even if we ask for e.g. XML

    config.ReturnHttpNotAcceptable = true;          // if browser requests for 'x' format and we don't support 'x', then return 406 error
                                                    // (i.e., do not return JSON just because we support it, return 406)

    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
})
    .AddXmlDataContractSerializerFormatters()       // add XML formatter
    .AddApplicationPart(typeof(DiffingTask.Presentation.AssemblyReference).Assembly);

var app = builder.Build();





// Configure the HTTP request pipeline.

app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();



NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters

    .OfType<NewtonsoftJsonPatchInputFormatter>().First();
