using System;
using System.Collections.Generic;
using OrigoDB.Core;

namespace JsonNet.Test
{
    [Serializable]
    public class MySubModel : Model
    {
        private Dictionary<string, Model> _models = new Dictionary<string, Model>();

        public void AddModel(Model m)
        {
            _models.Add(m.GetType().FullName, m);
        }

        public IDictionary<string, Model> Ugg { get { return _models; } }
    }
}