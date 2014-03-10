Modules.JsonNetFormatter
=====================
An IFormatter implementation based on Newtonsoft.Json serialization.

Intended as a replacement for `BinaryFormatter` used by OrigoDB for snapshots, the command journal, requests and response.
OrigoDB snapshots are arbitrarily complex object graphs. Deserialization needs to recreate an identical object graph as the one serialized. 

## Why?
The major benefits are readability, interoperability and maintainability. We ran a single test on a
small (8.6k binary serialized, 3.6k json serialized) but complex graph. Serialization speed was slightly
slower but deserialization was about 25% faster. Size is about 60% smaller which not only requires
less storage for snapshots and journal but also means less i/o. Test is included.

Note! This is based on NET 4.5. Using NET 4.0 serialization/deserialization speed is about twice as fast as BinaryFormatter.

See more about OrigoDB on the project page: http://devrexlabs.github.io/

## Specification
* All types must be marked with `SerializableAttribute`
* Operates on instance fields unless marked with `NonSerializedAttribute`
* Includes inherited fields
* Includes compiler generated fields
* Writes unformatted Json (no indentation) or Bson, your choice

