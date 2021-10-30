using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class SimplePropertySerialization : BaseTests
    {
        [Test]
        public void TestStringProperty()
        {
            var code = GetCode("string", "String1", @"""Hello""");

            var expected = SimpleWriteCall("String1", "WriteString");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", GetWellKnownType());
        }
        
        [Test]
        public void TestIntProperty()
        {
            var code = GetCode("int", "Int1", "42");

            var expected = SimpleWriteCall("Int1", "WriteNumber");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", GetWellKnownType());
        }
        
        [Test]
        public void TestBoolProperty()
        {
            var code = GetCode("bool", "Bool1", "true");

            var expected = SimpleWriteCall("Bool1", "WriteBoolean");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", GetWellKnownType());
        }
    }
}
