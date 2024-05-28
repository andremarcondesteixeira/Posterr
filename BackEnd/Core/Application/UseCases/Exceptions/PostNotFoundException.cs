using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Application.UseCases.Exceptions;

public sealed class PostNotFoundException(long postId) : DomainException($"No post found with ID {postId}")
{
}
