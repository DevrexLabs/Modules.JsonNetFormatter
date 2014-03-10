using System;
using System.Collections.Generic;

namespace JsonNet.Test
{
    [Serializable]
    public class Collection
    {
        public IList<Thing> Things { get; private set; }

        public Collection()
        {
            Things = new List<Thing>();
        }
    }
}