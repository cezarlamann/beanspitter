using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BeanSpitter.Tests")]

namespace BeanSpitter.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;

    public static class XmlSchemaObjectCollectionUtils
    {
        public static Dictionary<string, HashSet<string>> GetTagsByType(this ICollection<XmlSchemaObject> schemaObjects)
        {
            var result = new Dictionary<string, HashSet<string>>();

            if (schemaObjects == null)
            {
                return result;
            }

            if (schemaObjects.Count == 0)
            {
                return result;
            }

            var xmlElements = schemaObjects
                .Where(w => w.GetType() == typeof(XmlSchemaElement))
                .ToArray();
            var types = schemaObjects
                .Where(w => w.GetType() == typeof(XmlSchemaComplexType))
                .ToArray();

            foreach (var item in xmlElements)
            {
                var el = (XmlSchemaElement)item;

                if (!ThisXmlSchemaElementIsValid(el))
                {
                    continue;
                }

                if (!result.ContainsKey(el.SchemaTypeName.Name))
                {
                    result.Add(el.SchemaTypeName.Name, new HashSet<string> { el.Name });
                }
                else
                {
                    result[el.SchemaTypeName.Name].Add(el.Name);
                }
            }

            foreach (var type in types)
            {
                var t = (XmlSchemaComplexType)type;

                if (!ThisXmlSchemaComplexTypeIsValid(t))
                {
                    continue;
                }

                var particle = (XmlSchemaGroupBase)t.Particle;

                var items = particle.Items
                    .OfType<XmlSchemaObject>()
                    .ToArray();

                var res = GetTagsByType(items);

                foreach (var item in res.Keys)
                {
                    if (result.ContainsKey(item))
                    {
                        foreach (var r in res[item])
                        {
                            result[item].Add(r);
                        }
                    }
                    else
                    {
                        result.Add(item, res[item]);
                    }
                }
            }

            return result;
        }

        internal static bool ThisXmlSchemaElementIsValid(XmlSchemaElement el)
        {
            if (el == null)
            {
                return false;
            }
            return !(string.IsNullOrEmpty(el.Name) ||
                    el.SchemaTypeName == null ||
                    string.IsNullOrEmpty(el.SchemaTypeName.Name));
        }

        internal static bool ThisXmlSchemaComplexTypeIsValid(XmlSchemaComplexType type)
        {
            if (type == null)
            {
                return false;
            }
            if (type.Particle == null)
            {
                return false;
            }
            if (!type.Particle.GetType().IsSubclassOf(typeof(XmlSchemaGroupBase)))
            {
                return false;
            }

            var particle = (XmlSchemaGroupBase)type.Particle;

            // just checking for the possibility of the "Items" property from the particle being null.
            // so far, I don't know about any type of particle which have null "Items" property by default,
            // but just in case...
            if (particle.Items == null)
            {
                return false;
            }

            if (particle.Items.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
