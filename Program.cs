using System.Text;
using Infraction.Backend.Image.Service.Api.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Infraction.Backend.Image.Service.Infrastructure.Middleware;
using Microsoft.EntityFrameworkCore;
using Minio; 
using Infraction.Backend.Image.Service.Api.Models;
using Invoice.Service.Core.Interfaces;
using Invoice.Service.Core.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

// Register validators 
builder.Services.AddTransient<IReportService, ReportService>();
// Register mappers
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoice Service", Version = "v1" });
    options.OperationFilter<FileUploadOperationFilter>(); // Custom filter para manejar multipart/form-data
});

// Add controllers and configure JSON options for serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Configure JWT authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var MsgNotFound = "JWT key not found";
var key = Encoding.ASCII.GetBytes(jwtKey ?? throw new ArgumentNullException(MsgNotFound));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use authentication middleware to authenticate JWT tokens
app.UseAuthentication();

// Use authorization middleware to enforce authorization policies
app.UseAuthorization();

// Register controllers
app.MapControllers();

app.UseCors("AllowAll");

//Use Error Handling middleware for exception control
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo
            .GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile));

        if (fileParameters.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = new HashSet<string> { "file" }
                        }
                    }
                }
            };
        }
    }
}
