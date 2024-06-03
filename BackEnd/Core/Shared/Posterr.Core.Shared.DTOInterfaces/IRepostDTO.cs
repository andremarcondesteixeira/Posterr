namespace Posterr.Core.Shared.DTOInterfaces;

public interface IRepostDTO : IPostDTO
{
    IPostDTO OriginalPost { get; }
}
