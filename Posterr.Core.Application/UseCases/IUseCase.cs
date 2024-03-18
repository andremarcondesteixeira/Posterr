namespace Posterr.Core.Application.UseCases;

public interface IUseCase<INPUT, OUTPUT>
{
    Task<OUTPUT> Run(INPUT input);
}
