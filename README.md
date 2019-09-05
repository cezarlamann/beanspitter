# BeanSpitter
## A Simple, Stream-Oriented XML Parser for .NET

### This XML Parsing Library aims to simplify parsing operations for files which:
- Have an associated XML Schema Definition (XSD);
- Have classes generated from its XSD file (from `xsd.exe`, for example);
- You don't want to create the parsing code by hand, going through `XElement`, dealing with strings and etc., especially for complex XML Schemas.

### What does this library offers:
- It is built over regular `XmlReader` and `System.Xml` infrastructure, so no "out of the world" code.
- It is Multi-Platform: `net45` and `netstandard2.0`
- Stream-oriented, Memory friendly (around 120 MB of Total Memory Consumption while running tests with over 4GB XML files with Visual Studio), asynchronous XML Validation and Parsing (using `Stream` and `Task`);
- Schema Loading, XML Validation and Parsing methods with overloads for Byte Arrays, Files and Streams;
- Parsing by type, with header type support (like those XML Files that have Envelope, Header and Payload schemas, e.g. ISO 20022 XML Messages - this library was initially made for these);
- Async Event-raising for nodes read, errors occurred and when it finishes parsing and validating. Kind of SAX inspired, but it raises POCO-like objects;

### Usage and Documentation:
- Still a work in progress, but you can find code samples in the [SAMPLES folder](https://github.com/cezarlamann/beanspitter/tree/master/SAMPLES) and a very basic usage example can be found [here](https://github.com/cezarlamann/beanspitter/blob/master/SAMPLES/SampleXmlParsingConsoleApp/SampleXmlParsingConsoleApp/Program.cs)

#### Caveats:
- When using .NET Core, you must add a `new XmlUrlResolver()` to your `XmlSchemaSet.XmlResolver` property (reference [here](https://stackoverflow.com/a/54764593/1475630)) or do [this](https://github.com/dotnet/corefx/wiki/ApiCompat#systemxmlschema).

#### Other remarks:
- It has only one dependency, for better testability, which is [System.IO.Abstractions](https://github.com/System-IO-Abstractions/System.IO.Abstractions);
- The idea is not to "hog" your `ThreadPool`: It will set the `TaskScheduler` to `Environment.ProcessorCount * 2` by default. [Reference from here](https://devblogs.microsoft.com/premier-developer/limiting-concurrency-for-faster-and-more-responsive-apps/).
- It is modular: Even though the defaults work OK, if you fancy something different, the object constructors from the library get Interfaces for almost everything, so you can replace even the `EventRiser` implementation if you would like.

#### Misc
- It is my first "real" opensource project, so bear with me :). Suggestions and contributions are welcome.
- It is not yet a fully tested solution (you can help to improve that), but it is good enough for what I meant and the current tests cover quite complex scenarios out-of-the-box
- To contribute, clone it, create a branch, do your stuff (with tests, please :)) and submit a pull request.
