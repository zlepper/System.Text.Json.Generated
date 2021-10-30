using System.IO;
using NUnit.Framework;

namespace System.Text.Json.Generated.OutputTests
{
    public abstract class BaseTest
    {
        
        protected string SerializeUsingGenerated<T>(T serializable)
            where T : IJsonSerializable
        {
            using var ms = new MemoryStream(1 << 16);
            var writer = new Utf8JsonWriter(ms);
            serializable.SerializeToJson(writer);
            writer.Flush();

            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        protected string SerializeUsingStdLib<T>(T serializable)
            where T : IJsonSerializable
        {
            return JsonSerializer.Serialize(serializable, serializable.GetType());
        }

        protected void VerifyOutputMatchesStandard<T>(T serializable)
        where T: IJsonSerializable
        {
            var correct = SerializeUsingStdLib(serializable);
            var generated = SerializeUsingGenerated(serializable);

            Assert.That(generated, Is.EqualTo(correct));
        } 
    }
}
