using Automata.Domain.Assets;
using MediatR;

namespace Automata.Application.Assets.Queries;

public record GetAllAssetsQuery() : IRequest<List<Asset>> { }