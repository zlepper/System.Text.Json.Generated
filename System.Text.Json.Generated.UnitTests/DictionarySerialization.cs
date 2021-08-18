﻿using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class DictionarySerialization : BaseTests
    {
        private string WriteDictionary(string propertyName, string methodName)
        {
            var body = $@"writer.WriteStartObject(MyClassSerializerConstants.{propertyName}PropertyName);
            foreach(var keyValuePair in MyDict)
            {{
                writer.{methodName}(keyValuePair.Key, keyValuePair.Value);
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
            foreach(var keyValuePair in MyDict)
            {
                writer.WriteString(keyValuePair.Key.ToString(CultureInfo.InvariantCulture), keyValuePair.Value);
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
    }
}
