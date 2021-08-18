using System.Collections.Generic;
using NUnit.Framework;

namespace System.Text.Json.Generated.OutputTests
{
    [TestFixture]
    public class SanityTests : BaseTest
    {
        [Test]
        public void HasSerializeMethod()
        {
            var c = new MySimpleClass();

            VerifyOutputMatchesStandard(c);
        }

        [Test]
        public void SerializesDictionaryProperty()
        {
            var c = new MySimpleDictionaryContainer();
            
            VerifyOutputMatchesStandard(c);
        }
    }

    [GenerateJsonSerializer]
    public partial class MySimpleClass
    {
        public int Int1 { get; set; } = 42;
        public bool Bool1 { get; set; }
        public string String1 { get; set; } = "Hello";
    }

    [GenerateJsonSerializer]
    public partial class MySimpleDictionaryContainer
    {
        public Dictionary<string, int> MyDict { get; set; } = new() {{"hello", 13}, {"world", 42}};
    }
}
