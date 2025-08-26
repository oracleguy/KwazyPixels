using System.Reflection;

namespace KwazyPixels;

/// <summary>
/// The entry point of the application.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Explicitly add environment variables to configuration
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddSingleton<IGalleryCollection, Services.GalleryCollection>();
        builder.Services.AddTransient<IImageProcessor, Services.ImageProcessor>();
        builder.Services.AddSingleton<Microsoft.IO.RecyclableMemoryStreamManager>();

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "Kwazy Pixels API",
                Description = "API for serving a gallary of images for multiple uses.",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "KwazyPixels Git Repository",
                    Url = new Uri("https://github.com/oracleguy/KwazyPixels")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .WithMethods("GET")
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors();

        app.MapControllers();

        app.Run();
    }
}
