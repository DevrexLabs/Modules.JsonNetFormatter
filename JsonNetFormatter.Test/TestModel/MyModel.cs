using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace JsonNet.Test
{
    [Serializable]
    public class MyModel : Model
    {
        public Collection Collection = new Collection();
        private Dictionary<string, Category> _categories = new Dictionary<string, Category>();

        public IDictionary<string, Category> Categories { get { return _categories; } }

        public void AddThing(Thing thing)
        {
            Collection.Things.Add(thing);
        }

    }
}