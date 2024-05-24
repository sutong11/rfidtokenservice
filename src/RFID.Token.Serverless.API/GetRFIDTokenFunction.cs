using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RFID.Token.Application.Models;
using RFID.Token.Application.UseCases;
using System.Net;

namespace RFID.Token.Serverless.API;

/// <summary>
/// Serverless API to get RFID Token
/// </summary>
/// <param name="mediator"></param>
/// <param name="loggerFactory"></param>
public class GetRFIDTokenFunction(IMediator mediator, ILoggerFactory loggerFactory)
{
        private readonly ILogger _logger = loggerFactory.CreateLogger<GetRFIDTokenFunction>();
        private readonly IMediator _mediator = mediator;

	[Function("Get")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
		//var customNumber = executionContext.Features.Get<CustomerNumber>();
		
            var queryParam = req.Query["id"];
            if (string.IsNullOrEmpty(queryParam))
            { 
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            var RFIDTokenId = Guid.Parse(queryParam);
		var rfidToken = await _mediator.Send(new FindRFIDTokenQuery(RFIDTokenId));
            
            if (rfidToken == null)
            {
			return req.CreateResponse(HttpStatusCode.NotFound);
		}

		var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<RFIDToken>(rfidToken);

            return response;
        }
    }
