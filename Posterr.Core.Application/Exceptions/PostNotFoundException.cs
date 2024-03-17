using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Application.Exceptions;

public class PostNotFoundException(long postId) : DomainException($"No post was found with ID {postId}")
{
}
