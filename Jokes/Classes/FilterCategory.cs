using Jokes.Interfaces;
using System;

namespace Jokes.Classes
{
    /// <summary>
    /// This represents the filters used to group jokes.
    /// </summary>
    public class FilterCategory: IFilterCategory
    {
        public string Name { get; set; }

        public Func<string, bool> CompareFunc { get; set; }
    }
}
