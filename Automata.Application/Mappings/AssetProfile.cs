using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Application.Assets.Dtos;
using Automata.Domain.Assets;

namespace Automata.Application.Mappings;

public class AssetProfile : Profile
{
    public AssetProfile()
    {
        // Domain <-> DTO mapping
        CreateMap<Asset, AssetDto>().ReverseMap();

        // Map CreateAssetCommand -> Asset
        CreateMap<CreateAssetCommand, Asset>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID is auto-generated

        // Map UpdateAssetCommand -> Asset
        CreateMap<UpdateAssetCommand, Asset>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Preserve original CreatedAt
    }
}