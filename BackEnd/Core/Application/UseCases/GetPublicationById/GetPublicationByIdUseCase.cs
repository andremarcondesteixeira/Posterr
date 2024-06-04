using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.GetPublicationById;

public class GetPublicationByIdUseCase(IPublicationsRepository publicationsRepository)
    : IUseCase<GetPublicationByIdUseCaseInputDTO, IPublication>
{
    public IPublication Run(GetPublicationByIdUseCaseInputDTO input)
    {
        return publicationsRepository.FindById(input.PublicationId)
            ?? throw new PublicationNotFoundException(input.PublicationId);
    }
}
