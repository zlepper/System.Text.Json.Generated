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

        [Test]
        public void SerializesNestedObjects()
        {
            var c = new ContainerClass();
            
            VerifyOutputMatchesStandard(c);
        }

        [Test]
        public void SerializedNestedDictionaries()
        {
            var c = new NestedDictionary();
            
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
        public Dictionary<string, int> MyDict1 { get; set; } = new() {{"hello", 13}, {"world", 42}};
        public Dictionary<string, string> MyDict2 { get; set; } = new() {{"hello", "13"}, {"world", "42"}};
        public Dictionary<string, bool> MyDict3 { get; set; } = new() {{"hello", true}, {"world", false}};
        public Dictionary<int, string> MyDict4 { get; set; } = new() {{13, "hello"}, {42, "world"}};
    }

    [GenerateJsonSerializer]
    public partial class ContainerClass
    {
        public NestedClass Nested { get; set; } = new();
    }

    [GenerateJsonSerializer]
    public partial class NestedClass
    {
        public int Int1 { get; set; } = 42;
        public bool Bool1 { get; set; }
        public string String1 { get; set; } = "Hello";
    }

    [GenerateJsonSerializer]
    public partial class NestedDictionary
    {
        public Dictionary<string, Dictionary<int, Dictionary<string, Dictionary<int, string>>>> MyDict { get; set; } =
            new Dictionary<string, Dictionary<int, Dictionary<string, Dictionary<int, string>>>>
            {
                {
                    "1", new Dictionary<int, Dictionary<string, Dictionary<int, string>>>
                    {
                        {
                            2, new Dictionary<string, Dictionary<int, string>>
                            {
                                {
                                    "3", new Dictionary<int, string>
                                    {
                                        { 4, "5" }
                                    }
                                }
                            }
                        }
                    }
                }
            };
    }

    [GenerateJsonSerializer]
    public partial class TopLevelDictionaryWithObject
    {
        public Dictionary<string, NestedClass> MyDict { get; set; } = new Dictionary<string, NestedClass>
        {
            { "foo", new NestedClass() }
        };
    }
}
