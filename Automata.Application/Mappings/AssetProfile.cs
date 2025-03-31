using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Application.Assets.Dtos;
using Automata.Domain.Aggregates.Assets;

namespace Automata.Application.Mappings;

public class AssetProfile : Profile
{
    public AssetProfile()
    {
        CreateMap<Asset, AssetDto>()
            .ForMember(dest => dest.MaintenanceRecords, opt => opt.MapFrom(src => src.MaintenanceRecords));

        CreateMap<CreateAssetCommand, Asset>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID is auto-generated

        CreateMap<UpdateAssetCommand, Asset>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.MaintenanceRecords, opt => opt.Ignore());

        CreateMap<MaintenanceRecord, MaintenanceRecordDto>();

        CreateMap<MaintenanceRecordDto, MaintenanceRecord>()
            .ConstructUsing(dto => new MaintenanceRecord(
                dto.ScheduledDate,
                Enum.Parse<MaintenanceType>(dto.Type, true),
                dto.Technician,
                dto.Notes
            ));

    }
}