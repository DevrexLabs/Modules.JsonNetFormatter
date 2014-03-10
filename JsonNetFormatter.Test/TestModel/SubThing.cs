using System;

namespace JsonNet.Test
{
    [Serializable]
    public class SubThing : Thing
    {
        [NonSerialized]
        public int Answer = 42;

        public string DingDong { get; set; }

        public SubThing(int id, string name)
            : base(id, name)
        {

        }
    }
}