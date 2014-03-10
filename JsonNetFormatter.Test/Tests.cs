using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Modules.JsonNet;
using NUnit.Framework;

namespace JsonNet.Test
{
    [TestFixture]
    public class Tests
    {
        private MyModel _graph, _target;
        private JsonNetFormatter _formatter;

        [TestFixtureSetUp]
        public void Setup()
        {
            _formatter = new JsonNetFormatter();
            _graph = ConstructComplexGraph();
            var ms = new MemoryStream();
            _formatter.Serialize(ms, _graph);
            ms.Position = 0;
            _target = (MyModel)_formatter.Deserialize(ms);
        }

        private MyModel ConstructComplexGraph()
        {
            var model = new MyModel();
            var t1 = new Thing(1, "Dog");
            var t2 = new Thing(2, "Cat");
            t1.RelatedThing = t2;
            t2.RelatedThing = t1;
            var cat = new Category("Pets");
            cat.Things.Add(t1);
            cat.Things.Add(t2);
            t1.Categories.Add(cat);
            t2.Categories.Add(cat);
            model.AddThing(t1);
            model.AddThing(t2);

            model.Categories.Add(cat.Name, cat);

            var subModel = model.ChildFor<MySubModel>();
            subModel.AddModel(model);
            subModel.AddModel(subModel);
            model.AddThing(null);
            model.AddThing(new SubThing(3, "Fish") { DingDong = "Woohoo!" });
            return model;
        }

        [Test]
        public void Private_dictionary_field_is_initialized()
        {
            Assert.IsNotNull(_target.Categories);
        }

        [Test]
        public void Private_dictionary_field_is_populated()
        {
            Assert.AreEqual(1, _target.Categories.Count);
        }

        [Test]
        public void Public_field_is_initialized()
        {
            Assert.IsNotNull(_target.Collection);
        }

        [Test]
        public void Inherited_field_is_assigned()
        {
            //private field of OrigoDB.Core.Model
            var inheritedFieldValue = _target.GetType().BaseType
                .GetField("_children", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_target);
            Assert.IsNotNull(inheritedFieldValue);
        }

        [Test]
        public void Object_in_inherited_collection_is_populated()
        {
            var subModel = _target.ChildFor<MySubModel>();
            Assert.AreEqual(2, subModel.Ugg.Count);
        }

        [Test]
        public void Objects_in_inherited_collection_have_correct_refs()
        {
            var subModel = _target.ChildFor<MySubModel>();
            Assert.IsTrue(subModel.Ugg.Values.Contains(subModel));
            Assert.IsTrue(subModel.Ugg.Values.Contains(_target));
        }

        [Test]
        public void Related_things_refer_to_each_other()
        {
            var dog = _target.Collection.Things[0];
            var cat = _target.Collection.Things[1];
            Assert.AreEqual(dog.RelatedThing, cat);
            Assert.AreEqual(cat.RelatedThing, dog);
        }

        [Test]
        public void Things_category_refers_to_root_objects_category()
        {
            var dog = _target.Collection.Things[0];
            var category = dog.Categories.Single();
            Assert.AreEqual(category, _target.Categories.Values.Single());
        }

        [Test]
        public void Derived_object_in_collection_is_correct_type()
        {
            var subThing = _target.Collection.Things[3] as SubThing;
            Assert.IsNotNull(subThing);
        }

        [Test]
        public void Derived_object_in_collection_has_correct_fields()
        {
            var subThing = _target.Collection.Things[3] as SubThing;
            Assert.AreEqual(3, subThing.Id);
            Assert.AreEqual("Fish", subThing.Name);
            Assert.AreEqual("Woohoo!", subThing.DingDong);
        }


        [Test]
        public void Things_public_readonly_fields_are_assigned()
        {
            Assert.AreEqual(1, _target.Collection.Things.First().Id);
            Assert.AreEqual("Dog", _target.Collection.Things.First().Name);
        }

        [Test]
        public void Non_serialized_field_is_ignored()
        {
            Assert.AreEqual(0, (_target.Collection.Things[3] as SubThing).Answer);
        }

        [Test, TestCaseSource("Formatters")]
        public void Benchmark(IFormatter formatter)
        {
            var ms = new MemoryStream();
            Console.WriteLine(formatter.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 5000; i++)
            {
                formatter.Serialize(ms, _graph);
                ms.Position = 0;
            }
            Console.WriteLine("Size: " + ms.Length);
            Console.WriteLine("Serialization: " + sw.Elapsed);
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 5000; i++)
            {
                formatter.Deserialize(ms);
                ms.Position = 0;
            }
            Console.WriteLine("Deserialization: " + sw.Elapsed);
        }

        private static IFormatter[] Formatters =
        {
            new BinaryFormatter(),
            new JsonNetFormatter(),
        };
    }
}
