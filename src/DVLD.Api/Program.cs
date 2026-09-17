using Serilog;
using DVLD.Application.Interfaces;
using DVLD.Application.Services;
using DVLD.Infrastructure.Settings;
using DVLD.Infrastructure.Repositories;
using DVLD.Shared;
using DVLD.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. Logging Configuration (Serilog)
// Replace default .NET logging with Serilog and direct output to Console & Seq
// -----------------------------------------------------------------------------
builder.Host.UseSerilog((context, configuration) => configuration
    .WriteTo.Console()
    .WriteTo.Seq(context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341"));
// -----------------------------------------------------------------------------
// 2. Controller & API Explorer Registration
// -----------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------------------------------
// 3. Dependency Injection (Application & Infrastructure Services)
// Register repositories and business services with their appropriate lifecycles
// -----------------------------------------------------------------------------
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
// builder.Services.AddScoped<IPersonService, PersonService>();

// Time provider abstraction for testability and flexibility
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

// Strongly-typed options configuration for file storage settings
builder.Services.Configure<FileStorageSettings>(
    builder.Configuration.GetSection(FileStorageSettings.SectionName));
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// -----------------------------------------------------------------------------
// 4. Centralized Exception Handling & RFC 7807 ProblemDetails
// Register custom exception handler to catch fatal, unexpected errors
// -----------------------------------------------------------------------------
builder.Services.AddExceptionHandler<DVLD.Api.Middlewares.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// =============================================================================
// HTTP Request Pipeline (Middleware execution order is critical!)
// =============================================================================

// Step A: Outer layer - logs complete HTTP lifecycle (status codes, execution time)
app.UseSerilogRequestLogging();

// Step B: Safety net - wraps subsequent middleware in a global try/catch mechanism
app.UseExceptionHandler();

// Step C: Interactive documentation for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Step D: Security & routing pipeline
app.UseHttpsRedirection();
app.UseAuthorization();

// Step E: Endpoints execution (invokes controllers and action methods)
app.MapControllers();

app.Run();