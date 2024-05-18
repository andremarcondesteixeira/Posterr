﻿namespace Posterr.Core.Boundaries.EntitiesInterfaces;

public interface IPublication
{
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}