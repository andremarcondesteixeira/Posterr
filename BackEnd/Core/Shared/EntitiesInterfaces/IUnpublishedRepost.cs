﻿namespace Posterr.Core.Shared.EntitiesInterfaces;

public interface IUnpublishedRepost
{
    IUser Author { get; }
    string Content { get; }
    IPost OriginalPost { get; }
}
