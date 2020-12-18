//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.1.21.0 (Newtonsoft.Json v11.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------
using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Elements.Geometry.Solids;
using Elements.Properties;
using Elements.Validators;
using Elements.Serialization.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace Elements
{
    #pragma warning disable // Disable all warnings

    /// <summary>An element with a geometric representation.</summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class GeometricElement : Element
    {
        [Newtonsoft.Json.JsonConstructor]
        public GeometricElement(Transform @transform, IList<Representation> @representations, bool @isElementDefinition, System.Guid @id, string @name)
        	: base(id, name)
        {
        	var validator = Validator.Instance.GetFirstValidatorForType<GeometricElement>();
        	if(validator != null)
        	{
        		validator.PreConstruct(new object[]{ @transform, @representations, @isElementDefinition, @id, @name});
        	}
        
        	this.Transform = @transform;
        	this.Representations = @representations;
        	this.IsElementDefinition = @isElementDefinition;
        	
        	if(validator != null)
        	{
        		validator.PostConstruct(this);
        	}
        }
    
        /// <summary>The element's transform.</summary>
        [Newtonsoft.Json.JsonProperty("Transform", Required = Newtonsoft.Json.Required.AllowNull)]
        public Transform Transform { get; set; }
    
        /// <summary>The element's representation.</summary>
        [Newtonsoft.Json.JsonProperty("Representations", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public IList<Representation> Representations { get; set; } = new List<Representation>();
    
        /// <summary>When true, this element will act as the base definition for element instances, and will not appear in visual output.</summary>
        [Newtonsoft.Json.JsonProperty("IsElementDefinition", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsElementDefinition { get; set; } = false;
    
    
    }
}