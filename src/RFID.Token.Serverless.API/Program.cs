using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RFID.Token.Application.Handler;
using RFID.Token.Application.Interfaces;
using RFID.Token.Infrastructure.AutoMapper;
using RFID.Token.Infrastructure.Data;
using RFID.Token.Infrastructure.Repository;
using RFID.Token.Serverless.API.Middleware;

var host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults(worker => worker.UseMiddleware<CustomerNumberAzureFunctionMiddleware>())
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		services.AddDbContext<RFIDTokenDbContext>(options =>
		{
			options.UseInMemoryDatabase("RFIDTokenDb");
		});

		services.AddScoped<IReadRFIDTokenRepository, ReadRFIDTokenRepository>();
		services.AddTransient<SeedData>();
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(ReadRFIDTokenQueryHandler).Assembly);
		});

		var mappingConfig = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new InfrastructureAutoMappingProfile()); // Add your profiles
																	// Add more profiles if needed
		});

		IMapper mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);
	})
	.Build();

// Seed data before functions start running
using (var scope = host.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	SeedData.Initialize(serviceProvider);
}

host.Run();