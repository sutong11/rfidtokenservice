using AutoMapper;

namespace RFID.Token.Infrastructure.AutoMapper;

public class InfrastructureAutoMappingProfile : Profile
{
	public InfrastructureAutoMappingProfile()
	{
		CreateMap<Models.RFIDToken, Application.Models.RFIDToken>();
	}
}
