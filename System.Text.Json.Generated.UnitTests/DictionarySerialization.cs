using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Generated.Generator.Models;
using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class DictionarySerialization : BaseTests
    {
        private const string DictionaryTypeName = "global::System.Collections.Generic.Dictionary";
        
        private string WriteDictionary(string propertyName)
        {
            var body = $@"writer.WritePropertyName(MyClassSerializerConstants.{propertyName}PropertyName);
            ForeignTypeSerializer.SerializeToJson(MyDict, writer);";

            return GetExpected(propertyName, body);
        }

        private IEnumerable<IWellKnownType> CreateWellKnownDictionary(params string[] typeNames)
        {
            IWellKnownType outer = new WellKnownValueType(typeNames.Last());

            foreach (var type in typeNames.Skip(1).SkipLast(1).Reverse())
            {
                outer = new WellKnownDictionary(type, DictionaryTypeName, outer);
                yield return outer;
            }

            yield return new WellKnownDictionary(typeNames.First(), DictionaryTypeName, outer);
        }
        
        [Test]
        public void HandleStringInt()
        {
            var code = GetCode("Dictionary<string, int>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("string", "int"));
        }
        [Test]
        public void HandleIntString()
        {
            var code = GetCode("Dictionary<int, string>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("int", "string"));
        }
        [Test]
        public void HandleStringString()
        {
            var code = GetCode("Dictionary<string, string>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("string", "string"));
        }
        [Test]
        public void HandleStringBool()
        {
            var code = GetCode("Dictionary<string, bool>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("string", "bool"));
        }

        [Test]
        public void NestedDictionaries()
        {
            var code = GetCode("Dictionary<string, Dictionary<string, Dictionary<string, int>>>", "MyDict", "new()");
            
            var expected = WriteDictionary("MyDict");
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("string", "string", "string", "int"));
        }
        
        [Test]
        public void NestedIntKeyDictionaries()
        {
            var code = GetCode("Dictionary<int, Dictionary<int, Dictionary<int, int>>>", "MyDict", "new()");
            
            var expected = WriteDictionary("MyDict");
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownDictionary("int", "int", "int", "int"));
        }
        
        [Test]
        public void HandleTopLevelNested()
        {
            var code = GetCode("Dictionary<string, NestedClass>", "MyDict", "new()");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var expected = WriteDictionary("MyDict");
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");

            VerifyMainGenerator.RunSimpleTest(new []{code, nestedClass}, new []{expected, expectedNested}, new []{"MyCode.MyClass", "MyCode.NestedClass"}, new WellKnownDictionary("string", DictionaryTypeName, new SerializableValueType("global::MyCode.NestedClass")));
        }
        
        [Test]
        public void HandleNestedObjectInNestedDictionary()
        {
            var code = GetCode("Dictionary<string, Dictionary<int, NestedClass>>", "MyDict", "new()");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var expected = WriteDictionary("MyDict");
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");

            VerifyMainGenerator.RunSimpleTest(new []{code, nestedClass}, new []{expected, expectedNested}, new []{"MyCode.MyClass", "MyCode.NestedClass"}, new WellKnownDictionary("string", DictionaryTypeName, new WellKnownDictionary("int", DictionaryTypeName, new SerializableValueType("global::MyCode.NestedClass"))));
        }
    }
}
