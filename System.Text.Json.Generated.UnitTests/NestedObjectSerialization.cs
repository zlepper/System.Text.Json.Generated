using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class NestedObjectSerialization : BaseTests
    {
        [Test]
        public void ConvertsNestedObject()
        {
            var parentClass = GetCode("NestedClass", "Nested", "new()", "ParentClass");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var expectedParent = GetExpected("Nested", GetNestedObjectSerializationCall("Nested", "ParentClass"), "ParentClass");
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");
            
            VerifyMainGenerator.RunSimpleTest(new []{parentClass, nestedClass}, new []{expectedParent, expectedNested}, new []{"MyCode.ParentClass", "MyCode.NestedClass"});

        }
    }
}
