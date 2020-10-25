using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace DanbooruDownloader3.Utils
{
    public class LocalXhtmlXmlResolver : XmlUrlResolver
    {
        private const string ResourcePrefix = "DanbooruDownloader3.";

        private static readonly Dictionary<string, string> _knownDtds = new Dictionary<string, string>
        {
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", ResourcePrefix + "xhtml1-strict.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", ResourcePrefix + "xhtml1-transitional.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd", ResourcePrefix + "xhtml1-frameset.dtd" },
            { "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", ResourcePrefix + "xhtml11.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Latin 1 for XHTML//EN", ResourcePrefix + "xhtml-lat1.ent" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Special for XHTML//EN", ResourcePrefix + "xhtml-special.ent" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Symbols for XHTML//EN", ResourcePrefix + "xhtml-symbol.ent" }
        };

        private static readonly Dictionary<string, Uri> _knownUris = new Dictionary<string, Uri>
        {
            { "-//W3C//DTD XHTML 1.0 Strict//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd") },
            { "-//W3C XHTML 1.0 Transitional//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
            { "-//W3C//DTD XHTML 1.0 Transitional//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
            { "-//W3C XHTML 1.0 Frameset//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd") },
            { "-//W3C//DTD XHTML 1.1//EN", new Uri("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd") }
        };

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return _knownUris.ContainsKey(relativeUri) ? _knownUris[relativeUri] : base.ResolveUri(baseUri, relativeUri);
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }

            if (_knownDtds.ContainsKey(absoluteUri.OriginalString))
            {
                string resourceName = _knownDtds[absoluteUri.OriginalString];
                Assembly assembly = Assembly.GetAssembly(typeof(LocalXhtmlXmlResolver));
                return assembly.GetManifestResourceStream(resourceName);
            }

            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}