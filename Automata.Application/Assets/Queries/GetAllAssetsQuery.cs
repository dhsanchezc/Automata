using Automata.Application.Assets.Dtos;
using MediatR;

namespace Automata.Application.Assets.Queries;

public record GetAllAssetsQuery() : IRequest<List<AssetDto>> { }