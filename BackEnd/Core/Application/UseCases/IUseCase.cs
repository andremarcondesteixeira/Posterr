namespace Posterr.Core.Application.UseCases;

public interface IUseCaseRegisteredInDependencyInjectionContainer;

public interface IUseCase<INPUT, OUTPUT> : IUseCaseRegisteredInDependencyInjectionContainer
{
    Task<OUTPUT> Run(INPUT input);
}
