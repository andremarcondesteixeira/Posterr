using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Core.Shared.PersistenceInterfaces;

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
