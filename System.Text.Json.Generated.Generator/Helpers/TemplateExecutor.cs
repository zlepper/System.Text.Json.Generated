using System.IO;
using System.Linq;
using System.Reflection;
using Scriban;
using Scriban.Runtime;

namespace System.Text.Json.Generated.Generator.Helpers
{
    public class TemplateExecutor
    {
        private readonly TemplateContext _context;
        private readonly Template _template;

        public TemplateExecutor(string templateName)
        {
            var caller = Assembly.GetCallingAssembly();

            var callerName = caller.GetName().Name;

            var filename = $"{callerName}.Templates.{templateName}.template.txt";
            using var stream = caller.GetManifestResourceStream(filename);
            if (stream == null) throw new Exception($"Could not find file '{filename}' in caller assembly");
            using var reader = new StreamReader(stream);
            var txt = reader.ReadToEnd();

            _template = Template.Parse(txt, templateName);

            if (_template.HasErrors)
            {
                var exceptions = _template.Messages
                    .Select(m => new Exception($"{m.Span} {m.Message}"));

                throw new AggregateException("Scriban parser error", exceptions);
            }

            _context = new TemplateContext
            {
                StrictVariables = true,
                EnableRelaxedIndexerAccess = false,
                EnableRelaxedTargetAccess = false,
                EnableRelaxedMemberAccess = false
            };
        }

        public string Render(object data)
        {
            var so = new ScriptObject();
            so.Import(data);

            _context.PushGlobal(so);

            var output = _template.Render(_context);

            _context.PopGlobal();

            return output;
        }
    }
}