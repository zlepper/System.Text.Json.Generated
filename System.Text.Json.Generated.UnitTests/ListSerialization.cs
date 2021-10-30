using Microsoft.VisualStudio.Composition;
using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class ListSerialization : BaseTests
    {

        [Test]
        public void SerializedIntList()
        {
            var code = GetCode("MyInt", "int", "42");
            
            var expected = GetExpected("MyInt", GetListWriteCall("MyInt", "MyClass", "WriteNumber"));
            
            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        
    }
}
