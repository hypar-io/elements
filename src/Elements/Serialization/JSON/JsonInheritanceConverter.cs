#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Serialization.JSON
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.24.0 (Newtonsoft.Json v9.0.0.0)")]
    public class JsonInheritanceConverter : Newtonsoft.Json.JsonConverter
    {
        internal static readonly string DefaultDiscriminatorName = "discriminator";

        private readonly string _discriminator;

        [System.ThreadStatic]
        private static bool _isReading;

        [System.ThreadStatic]
        private static bool _isWriting;

        private Dictionary<string, Type> _typeCache;

        [System.ThreadStatic]
        private static Dictionary<Guid, Element> _identifiables;   // = new Dictionary<Guid, Identifiable>();
        
        public static Dictionary<Guid, Element> Identifiables
        {
            get
            {
                if(_identifiables == null)
                {
                    _identifiables = new Dictionary<Guid, Element>();
                }
                return _identifiables;
            }
        }

        public JsonInheritanceConverter()
        {
            _discriminator = DefaultDiscriminatorName;
            _typeCache = BuildUserElementTypeCache();
        }

        public JsonInheritanceConverter(string discriminator)
        {
            _discriminator = discriminator;
            _typeCache = BuildUserElementTypeCache();
        }

        private Dictionary<string, Type> BuildUserElementTypeCache()
        {
            var typeCache = new Dictionary<string, Type>();

            // Build the user element type cache
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var asm in asms)
            {
                try
                {
                    var userTypes = asm.GetTypes().Where(t=>t.GetCustomAttributes(typeof(UserElement), true).Length > 0);
                    foreach(var ut in userTypes)
                    {
                        typeCache.Add(ut.FullName, ut);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return typeCache;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            try
            {
                _isWriting = true;

                // Operate on all identifiables with a path less than Entities.xxxxx
                // This will get all properties.
                if(value is Element && writer.Path.Split('.').Length == 1)
                {
                    var ident = (Element)value;
                    writer.WriteValue(ident.Id);
                }
                else
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.FromObject(value, serializer);
                    jObject.AddFirst(new Newtonsoft.Json.Linq.JProperty(_discriminator, GetSubtypeDiscriminator(value.GetType())));
                    writer.WriteToken(jObject.CreateReader());
                }
            }
            finally
            {
                _isWriting = false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (_isWriting)
                {
                    _isWriting = false;
                    return false;
                }
                return true;
            }
        }

        public override bool CanRead
        {
            get
            {
                if (_isReading)
                {
                    _isReading = false;
                    return false;
                }
                return true;
            }
        }

        public override bool CanConvert(System.Type objectType)
        {
            return true;
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, System.Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(typeof(Element).IsAssignableFrom(objectType) && reader.Path.Split('.').Length == 1)
            {
                var id = Guid.Parse(reader.Value.ToString());
                return Identifiables[id];
            }

            var jObject = serializer.Deserialize<Newtonsoft.Json.Linq.JObject>(reader);
            if (jObject == null)
                return null;

            var discriminator = Newtonsoft.Json.Linq.Extensions.Value<string>(jObject.GetValue(_discriminator));
            var subtype = GetObjectSubtype(objectType, discriminator);
            
            var objectContract = serializer.ContractResolver.ResolveContract(subtype) as Newtonsoft.Json.Serialization.JsonObjectContract;
            if (objectContract == null || System.Linq.Enumerable.All(objectContract.Properties, p => p.PropertyName != _discriminator))
            {
                jObject.Remove(_discriminator);
            }

            try
            {
                _isReading = true;
                var obj = serializer.Deserialize(jObject.CreateReader(), subtype);
                
                // Write the id to the cache so that we can retrieve it next time
                // instead of de-serializing it again.
                if(typeof(Element).IsAssignableFrom(objectType) && reader.Path.Split('.').Length > 1)
                {
                    var ident = (Element)obj;
                    if(!Identifiables.ContainsKey(ident.Id))
                    {
                        Identifiables.Add(ident.Id, ident);
                    }
                }

                return obj;
            }
            finally
            {
                _isReading = false;
            }
        }

        private System.Type GetObjectSubtype(System.Type objectType, string discriminator)
        {
            // Check the existing inheritance attributes.
            // This block will be hit when a type contains
            // an inheritance attribute specifying one of its subtypes.
            foreach (var attribute in System.Reflection.CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(System.Reflection.IntrospectionExtensions.GetTypeInfo(objectType), true))
            {
                if (attribute.Key == discriminator)
                    return attribute.Type;
            }

            // If the inheritance attributes is not supplied, as in the case
            // of a user-provided type, then we use the type cache of all
            // types with the UserElementType attribute.
            if(_typeCache.ContainsKey(discriminator))
            {
                return _typeCache[discriminator];
            }

            throw new Exception($"The type with discriminator, {discriminator}, could not be found. The entity will not be deserialized.");

            // The default behavior for this converter, as provided by nJSONSchema
            // is to return the base objectType if a derived type can't be found.
            // return objectType;
        }

        private string GetSubtypeDiscriminator(System.Type objectType)
        {
            foreach (var attribute in System.Reflection.CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(System.Reflection.IntrospectionExtensions.GetTypeInfo(objectType), true))
            {
                if (attribute.Type == objectType)
                    return attribute.Key;
            }
            
            return objectType.FullName;
        }
    }
}