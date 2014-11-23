Modules.JsonNetFormatter
=====================
An IFormatter implementation based on Newtonsoft.Json serialization intended as an alternative to the default `BinaryFormatter` used by OrigoDB for messages and data. It can also be used independently.

OrigoDB snapshots are arbitrarily complex object graphs. Deserialization needs to recreate an identical object graph as the one serialized.

## Why?
The major benefits are readability, interoperability and maintainability. But performance in general is probably better too. You should run benchmarks based on your own data.

## Performance
Here's some test output:
```
Release
Modules.JsonNet.JsonNetFormatter
Size: 1736
Serialization: 00:00:00.5210129
Deserialization: 00:00:00.5948599

System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
Size: 4953
Serialization: 00:00:00.9770694
Deserialization: 00:00:01.2388305
```

## Specification
* All types must be marked with `SerializableAttribute`
* Operates on instance fields unless marked with `NonSerializedAttribute`
* Includes inherited fields
* Includes compiler generated fields
* Writes unformatted Json (no indentation)

## Usage

```csharp
IFormatter formatter = new JsonNetFormatter();
MemoryStream ms = new MemoryStream(); // or any other kind of stream...
formatter.Serialize(ms, someObjectRef);
ms.Position = 0;
object clone = formatter.Deserialize(ms);
```
## Download / install
Nuget: https://www.nuget.org/packages/OrigoDB.JsonNetFormatter/
Binary: https://github.com/DevrexLabs/Modules.JsonNetFormatter/releases