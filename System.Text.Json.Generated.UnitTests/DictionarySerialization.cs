using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class DictionarySerialization : BaseTests
    {
        private string WriteDictionary(string propertyName, string methodName)
        {
            var body = $@"writer.WriteStartObject(MyClassSerializerConstants.{propertyName}PropertyName);
            foreach(var keyValuePair1 in MyDict)
            {{
                writer.{methodName}(keyValuePair1.Key, keyValuePair1.Value);
            }}
            writer.WriteEndObject();";

            return GetExpected(propertyName, body);
        }
        
        [Test]
        public void HandleStringInt()
        {
            var code = GetCode("Dictionary<string, int>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict", "WriteNumber");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleIntString()
        {
            var code = GetCode("Dictionary<int, string>", "MyDict", "new()");

            var body = @"writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair1 in MyDict)
            {
                writer.WriteString(keyValuePair1.Key.ToString(CultureInfo.InvariantCulture), keyValuePair1.Value);
            }
            writer.WriteEndObject();";

            var expected = GetExpected("MyDict", body);

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleStringString()
        {
            
            var code = GetCode("Dictionary<string, string>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict", "WriteString");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleStringBool()
        {
            var code = GetCode("Dictionary<string, bool>", "MyDict", "new()");

            var expected = WriteDictionary("MyDict", "WriteBoolean");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }

        [Test]
        public void NestedDictionaries()
        {
            var code = GetCode("Dictionary<string, Dictionary<string, Dictionary<string, int>>>", "MyDict", "new()");
            
            var body = @"writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair1 in MyDict)
            {
                writer.WriteStartObject(keyValuePair1.Key);
                foreach(var keyValuePair2 in keyValuePair1.Value)
                {
                    writer.WriteStartObject(keyValuePair2.Key);
                    foreach(var keyValuePair3 in keyValuePair2.Value)
                    {
                        writer.WriteNumber(keyValuePair3.Key, keyValuePair3.Value);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();";

            var expected = GetExpected("MyDict", body);
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        
        [Test]
        public void NestedIntKeyDictionaries()
        {
            var code = GetCode("Dictionary<int, Dictionary<int, Dictionary<int, int>>>", "MyDict", "new()");
            
            var body = @"writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair1 in MyDict)
            {
                writer.WriteStartObject(keyValuePair1.Key.ToString(CultureInfo.InvariantCulture));
                foreach(var keyValuePair2 in keyValuePair1.Value)
                {
                    writer.WriteStartObject(keyValuePair2.Key.ToString(CultureInfo.InvariantCulture));
                    foreach(var keyValuePair3 in keyValuePair2.Value)
                    {
                        writer.WriteNumber(keyValuePair3.Key.ToString(CultureInfo.InvariantCulture), keyValuePair3.Value);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();";

            var expected = GetExpected("MyDict", body);
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        
        [Test]
        public void HandleTopLevelNested()
        {
            var code = GetCode("Dictionary<string, NestedClass>", "MyDict", "new()");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var body = @"writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair1 in MyDict)
            {
                writer.WritePropertyName(keyValuePair1.Key);
                keyValuePair1.Value.SerializeToJson(writer);
            }
            writer.WriteEndObject();";
            
            var expected = GetExpected("MyDict", body);
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");

            VerifyMainGenerator.RunSimpleTest(new []{code, nestedClass}, new []{expected, expectedNested}, new []{"MyCode.MyClass", "MyCode.NestedClass"});
        }
        
        [Test]
        public void HandleNestedObjectInNestedDictionary()
        {
            var code = GetCode("Dictionary<string, Dictionary<int, NestedClass>>", "MyDict", "new()");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var body = @"writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair1 in MyDict)
            {
                writer.WriteStartObject(keyValuePair1.Key);
                foreach(var keyValuePair2 in keyValuePair1.Value)
                {
                    writer.WritePropertyName(keyValuePair2.Key.ToString(CultureInfo.InvariantCulture));
                    keyValuePair2.Value.SerializeToJson(writer);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();";
            
            var expected = GetExpected("MyDict", body);
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");

            VerifyMainGenerator.RunSimpleTest(new []{code, nestedClass}, new []{expected, expectedNested}, new []{"MyCode.MyClass", "MyCode.NestedClass"});
        }
    }
}
