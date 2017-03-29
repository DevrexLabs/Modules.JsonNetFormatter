using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Modules.JsonNet
{
    /// <summary>
    /// IFormatter implementation based on Newtonsoft.Json serialization.
    /// Requires the Serializable attribute and optionally the  ISerializable interface.
    /// Reads/writes fields, including inherited and compiler generated.
    /// Reconstructs an identical object graph on deserialization.
    /// </summary>
    public class JsonNetFormatter : IFormatter
    {
        private readonly JsonSerializer _serializer;

        public SerializationBinder Binder{ get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }

        public JsonNetFormatter()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = false,
                    SerializeCompilerGeneratedMembers = true
                },
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            _serializer = JsonSerializer.Create(settings);
        }

        public object Deserialize(Stream serializationStream)
        {
            var reader = new JsonTextReader(new StreamReader(serializationStream));
            return _serializer.Deserialize(reader);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            var writer = new JsonTextWriter(new StreamWriter(serializationStream));
            _serializer.Serialize(writer, graph);
            writer.Flush();
        }
    }
}
