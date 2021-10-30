using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Generated.Generator.Models;
using Microsoft.VisualStudio.Composition;
using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class ListSerialization : BaseTests
    {
        private const string ListTypeName = "global::System.Collections.Generic.List";

        private IEnumerable<IWellKnownType> CreateWellKnownList(int levels, string endType)
        {
            IWellKnownType outer = new WellKnownValueType(endType);

            foreach (var type in Enumerable.Range(0, levels))
            {
                outer = new WellKnownList(ListTypeName, outer);
                yield return outer;
            }
        }

        private string WriteList(string propertyName)
        {
            var body = GetSerializeToJsonWriteCall(propertyName);

            return GetExpected(propertyName, body);
        }

        [Test]
        public void SerializedIntList()
        {
            var code = GetCode("List<int>", "MyInts", "new()");

            var expected = WriteList("MyInts");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(1, "int"));
        }

        [Test]
        public void SerializedStringList()
        {
            var code = GetCode("List<string>", "MyStrings", "new()");

            var expected = WriteList("MyStrings");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(1, "string"));
        }


        [Test]
        public void SerializedNestedStringList()
        {
            var code = GetCode("List<List<List<string>>>", "MyStrings", "new()");

            var expected = WriteList("MyStrings");

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(3, "string"));
        }

        [Test]
        public void HandleNestedObjectInNestedList()
        {
            var code = GetCode("List<List<NestedClass>>", "MyList", "new()");
            var nestedClass = GetCode("int", "Int1", "42", "NestedClass");

            var expected = WriteList("MyList");
            var expectedNested = SimpleWriteCall("Int1", "WriteNumber", "NestedClass");

            VerifyMainGenerator.RunSimpleTest(new[] { code, nestedClass }, new[] { expected, expectedNested },
                new[] { "MyCode.MyClass", "MyCode.NestedClass" },
                new WellKnownList(ListTypeName,
                    new WellKnownList(ListTypeName, new SerializableValueType("global::MyCode.NestedClass"))));
        }
    }
}
