using System.Text.Json.Generated.Generator;
using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class VerifyTemplates
    {
        [Test]
        public void TemplatesAreValid()
        {
            MainGenerator.ValidateTemplates();
        }
    }
}
