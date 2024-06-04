namespace Posterr.Core.Application.UseCases;

public interface IUseCaseRegisteredInDependencyInjectionContainer;

public interface IUseCase<INPUT, OUTPUT> : IUseCaseRegisteredInDependencyInjectionContainer
{
    OUTPUT Run(INPUT input);
}

public interface IUseCase<OUTPUT> : IUseCaseRegisteredInDependencyInjectionContainer
{
    OUTPUT Run();
}
