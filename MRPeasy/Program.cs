using MRPeasy.Shared.Infrastructure.Interfaces.ASP.Configuration;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Repositories;
using MRPeasy.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MRPeasy.Inventories.Application.Internal.CommandServices;
using MRPeasy.Inventories.Application.Internal.QueryServices;
using MRPeasy.Inventories.Domain.Repositories;
using MRPeasy.Inventories.Domain.Services;
using MRPeasy.Inventories.Infrastructure.Persistence.EFC.Repositories;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Application.Internal.CommandServices;
using MRPeasy.Manufacturing.Application.Internal.QueryServices;
using MRPeasy.Manufacturing.Domain.Repositories;
using MRPeasy.Manufacturing.Domain.Services;
using MRPeasy.Manufacturing.Infrastructure.Persistence.EFC.Repositories;
using MRPeasy.Manufacturing.Infrastructure.ACL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging(false)
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "MRPeasy Manufacturing Platform",
            Version = "v1",
            Description = "MRPeasy Manufacturing Platform API",
            TermsOfService = new Uri("https://www.mrpeasy.com/terms/"),
            Contact = new OpenApiContact
            {
                Name = "MRPeasy",
                Email = "contact@mrpeasy.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.EnableAnnotations();
});

// Dependency Injection

// Configuration
builder.Services.Configure<CapacityThresholds>(builder.Configuration.GetSection("CapacityThresholds"));

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Inventories Bounded Context
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductDomainService>();
builder.Services.AddScoped<ProductCommandService>();
builder.Services.AddScoped<ProductQueryService>();

// Manufacturing Bounded Context
builder.Services.AddScoped<IBillOfMaterialsItemRepository, BillOfMaterialsItemRepository>();
builder.Services.AddScoped<IInventoriesContextService, InventoriesContextService>();
builder.Services.AddScoped<BillOfMaterialsItemDomainService>();
builder.Services.AddScoped<BillOfMaterialsItemCommandService>();
builder.Services.AddScoped<BillOfMaterialsItemQueryService>();



var app = builder.Build();

// Verify if the database exists and create it if it doesn't
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


// Apply CORS Policy
app.UseCors("AllowAllPolicy");

// Add Authorization Middleware to Pipeline
//app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();