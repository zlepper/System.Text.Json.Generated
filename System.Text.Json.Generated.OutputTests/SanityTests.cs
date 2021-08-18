using System.IO;
using NUnit.Framework;

namespace System.Text.Json.Generated.OutputTests
{
    [TestFixture]
    public class SanityTests
    {
        [Test]
        public void HasSerializeMethod()
        {
            var c = new MyClass();

            var correct = SerializeUsingStdLib(c);
            var generated = SerializeUsingGenerated(c);

            Assert.That(generated, Is.EqualTo(correct));
        }

        private string SerializeUsingGenerated<T>(T serializable)
            where T : IJsonSerializable
        {
            using var ms = new MemoryStream(1 << 16);
            var writer = new Utf8JsonWriter(ms);
            serializable.SerializeToJson(writer);
            writer.Flush();

            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        private string SerializeUsingStdLib<T>(T serializable)
            where T : IJsonSerializable
        {
            return JsonSerializer.Serialize(serializable);
        }
    }

    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public int Int1 { get; set; } = 42;
        public bool Bool1 { get; set; }
        public string String1 { get; set; } = "Hello";
    }
}