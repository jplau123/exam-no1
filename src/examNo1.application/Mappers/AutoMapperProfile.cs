using AutoMapper;
using examNo1.application.Dtos.Internal;
using examNo1.application.Dtos.Responses;
using examNo1.domain.Entities;

namespace examNo1.application.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ExchangeRateItemEntity, ExchangeRateInternal>().ReverseMap();
        CreateMap<ExchangeRateInternal, ExchangeRateResponse>().ReverseMap();
    }
}
