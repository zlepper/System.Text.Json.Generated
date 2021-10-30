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
                outer = new WellKnownList( ListTypeName, outer);
                yield return outer;
            }
        }
        
        [Test]
        public void SerializedIntList()
        {
            var code = GetCode("List<int>", "MyInts", "new()");
            
            var expected = GetExpected("MyInts", GetSerializeToJsonWriteCall("MyInts", "MyClass"));
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(1, "int"));
        }
        
        [Test]
        public void SerializedStringList()
        {
            var code = GetCode("List<string>", "MyStrings", "new()");
            
            var expected = GetExpected("MyStrings", GetSerializeToJsonWriteCall("MyStrings", "MyClass"));
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(1, "string"));
        }
        
        
        [Test]
        public void SerializedNestedStringList()
        {
            var code = GetCode("List<List<List<string>>>", "MyStrings", "new()");
            
            var expected = GetExpected("MyStrings", GetSerializeToJsonWriteCall("MyStrings", "MyClass"));
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass", CreateWellKnownList(3, "string"));
        }
        
    }
}
