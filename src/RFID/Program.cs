
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RFID.Token.API.ExceptionFilter;
using RFID.Token.API.Middleware;
using RFID.Token.Application.Handler;
using RFID.Token.Application.Interfaces;
using RFID.Token.Domain.EventStore;
using RFID.Token.Domain.EventStore.Interfaces;
using RFID.Token.Domain.Processors.Create;
using RFID.Token.Infrastructure.AutoMapper;
using RFID.Token.Infrastructure.Data;
using RFID.Token.Infrastructure.Processors;
using RFID.Token.Infrastructure.Repository;
using RFID.Token.Tools.MediatR.Configurations;

namespace RFID;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		
		// Add services to the container.
		builder.Services.AddControllers(options =>
		{
			options.Filters.Add<CustomExceptionFilter>();
		});
		builder.Services.AddLogging();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "RFID Token Service", Version = "v1" });
		});
		builder.Services.AddDbContext<RFIDTokenDbContext>(options =>
		{
			options.UseInMemoryDatabase("RFIDTokenDb");
		});

		builder.Services.AddSingleton<IEventStore, EventStore>();
		builder.Services.AddScoped<IReadRFIDTokenRepository, ReadRFIDTokenRepository>();
		builder.Services.AddScoped<IWriteRFIDTokenRepository, WriteRFIDTokenRepository>();

		var mappingConfig = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new InfrastructureAutoMappingProfile()); // Add your profiles
													// Add more profiles if needed
		});

		IMapper mapper = mappingConfig.CreateMapper();
		builder.Services.AddSingleton(mapper);

		#region MediatR
		builder.Services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(CreateRFIDTokenHandler).Assembly);
			cfg.RegisterServicesFromAssembly(typeof(CreateRFIDTokenProcessor).Assembly);
			cfg.RegisterServicesFromAssembly(typeof(RFIDTokenCreatedProjectionProcessor).Assembly);
		});

		builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestRetryBehavior<,>));
		#endregion

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
		app.UseSwagger();
		app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();
		app.UseMiddleware<CustomerNumberMiddleware>();
		app.UseMiddleware<RequestLoggingMiddleware>();

		app.MapControllers();

		app.Run();
	}
}
