namespace Posterr.Core.Shared.Exceptions;

public abstract class PosterrException(string problem, string mitigation) : Exception(problem)
{
    public string Mitigation { get; } = mitigation;
}
