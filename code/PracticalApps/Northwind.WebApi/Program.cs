using Microsoft.AspNetCore.HttpLogging; // To use HttpLoggingFields.
using Microsoft.AspNetCore.Mvc.Formatters; // To use IOutputFormatter.
using Microsoft.Extensions.Caching.Memory; // To use IMemoryCache and so on.
using Northwind.EntityModels; // To use AddNorthwindContext method.
using Northwind.WebApi.Repositories; // To use ICustomerRepository.
using Swashbuckle.AspNetCore.SwaggerUI; // To use SubmitMethod.

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
{
	options.LoggingFields = HttpLoggingFields.All;
	options.RequestBodyLogLimit = 4096; // Default is 32k.
	options.ResponseBodyLogLimit = 4096; // Default is 32k.
});

builder.Services.AddCors(options =>
	options.AddPolicy(name: MyAllowSpecificOrigins,
										policy => policy.WithOrigins("https://localhost:5161").AllowAnyMethod().AllowAnyHeader())
	);

// Add services to the container.
builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));

builder.Services.AddNorthwindContext();

builder.Services.AddControllers(options =>
{
	WriteLine("Default output formatters:");
	foreach (IOutputFormatter formatter in options.OutputFormatters)
	{
		OutputFormatter? mediaFormatter = formatter as OutputFormatter;
		if (mediaFormatter is null)
		{
			WriteLine($"  {formatter.GetType().Name}");
		}
		else // OutputFormatter class has SupportedMediaTypes.
		{
			WriteLine("  {0}, Media types: {1}",
				arg0: mediaFormatter.GetType().Name,
				arg1: string.Join(", ",
					mediaFormatter.SupportedMediaTypes));
		}
	}
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters(
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHealthChecks()
	.AddDbContextCheck<NorthwindContext>()
	// Execute SELECT 1 using the specified connection string.
	.AddSqlServer("Data Source=.;Initial Catalog=Northwind;Integrated Security=true;TrustServerCertificate=true;");

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json",
			"Northwind Service API Version 1");

		c.SupportedSubmitMethods(new[] {
			SubmitMethod.Get, SubmitMethod.Post,
			SubmitMethod.Put, SubmitMethod.Delete });
	});
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.UseHealthChecks(path: "/howdoyoufeel");

app.UseMiddleware<SecurityHeaders>();

app.MapControllers();

app.Run();