using System;
using System.Collections.Generic;

namespace JsonNet.Test
{
    [Serializable]
    public class Thing
    {
        public readonly int Id;
        public readonly string Name;
        public HashSet<Category> _categories = new HashSet<Category>();

        public Thing RelatedThing { get; set; }

        public Thing(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public ISet<Category> Categories { get { return _categories; } }
    }
}