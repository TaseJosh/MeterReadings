using AutoMapper;
using MeterReadings.Core.Domain.Entities;
using MeterReadings.Core.Domain.Models;

namespace MeterReadings.Core.Web.Profiles
{
    public class MeterReadingProfile : Profile
    {
        public MeterReadingProfile()
        {
            CreateMap<MeterReading, MeterReadingDto>()
                .ForMember(
                    dest => dest.MeterReadValue,
                    opt => opt.MapFrom
                    (src => src.MeterReadValue <= 9999
                        ? src.MeterReadValue.ToString("00000")
                        : src.MeterReadValue.ToString()));

            CreateMap<MeterReadingCsv, MeterReading>();
            CreateMap<MeterReading, MeterReadingCsv>();
            CreateMap<MeterReadingCreationDto, MeterReading>();
            CreateMap<MeterReadingUpdateDto, MeterReading>();
        }
    }
}