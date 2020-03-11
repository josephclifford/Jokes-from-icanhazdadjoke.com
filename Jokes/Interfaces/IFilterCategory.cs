using System;

namespace Jokes.Interfaces
{
    /// <summary>
    /// This represents the interface used to group jokes.
    /// </summary>
    public interface IFilterCategory
    {
        string Name { get; set; }

        Func<string, bool> CompareFunc { get; set; }
    }
}
