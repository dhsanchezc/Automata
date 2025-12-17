using Automata.Application.Assets.Dtos;
using MediatR;

namespace Automata.Application.Assets.Queries;

public record GetAssetByIdQuery(int Id) : IRequest<AssetDto?>;