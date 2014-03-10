using System;
using System.Collections.Generic;

namespace JsonNet.Test
{
    [Serializable]
    public class Category
    {
        public readonly string Name;

        public Category(string name)
        {
            Name = name;
            Things = new HashSet<Thing>();
        }

        public ISet<Thing> Things { get; private set; }
    }
}