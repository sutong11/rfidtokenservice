using MediatR;
using RFID.Token.Domain.Entity;
using RFID.Token.Domain.Events;
using RFID.Token.Domain.EventStore;
using RFID.Token.Domain.EventStore.Interfaces;
using Microsoft.Extensions.Logging;
using RFID.Token.Domain.Processors.UseCases;

namespace RFID.Token.Domain.Processors.Create
{
	/// <summary>
	/// Processes the Create RFID occurrence send from the application layer and publishes a RFIDTokenCreated if successfull
	/// </summary>
	/// <param name="eventstore"></param>
	/// <param name="mediator"></param>
	/// <param name="logger"></param>
	public class CreateRFIDTokenProcessor(IEventStore eventstore, IMediator mediator, ILogger<CreateRFIDTokenProcessor> logger) : IRequestHandler<CreateRFIDTokenOccurrence, bool>
	{
		private readonly IEventStore _eventStore = eventstore;
		private readonly IMediator _mediator = mediator;
		private readonly ILogger<CreateRFIDTokenProcessor> _logger = logger;
		public Task<bool> Handle(CreateRFIDTokenOccurrence request, CancellationToken cancellationToken)
		{
			try
			{
				var createdEvent = new CreateRFIDTokenEvent()
				{
					Id = request.RfIdToken,
					CustomerNumber = request.CustomerNumber,
					ValidFrom = request.ValidFrom,
					ValidTo = request.ValidTo
				};

				//"rehydrating"
				var domainEntity = new RFIDTokenEntity();
				domainEntity.Apply(createdEvent);

				//if no error we assume everything was ok, and we store it in the event store and publishes an event
				var @event = new Event("CreateRFIDTokenEvent", createdEvent);
				_eventStore.AppendEvent(request.RfIdToken, @event);

				var rfidTokenCreated = new RFIDTokenCreated(request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo);

				//we dont "care" if the receivers of the event handles the event correctly.
				_ = _mediator.Publish(rfidTokenCreated, cancellationToken);

				return Task.FromResult(true);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process RFID created occurrence", new[] { request });
				throw;
			}
		}
	}
}
