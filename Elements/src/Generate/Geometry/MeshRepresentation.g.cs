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

namespace Elements.Geometry
{
    #pragma warning disable // Disable all warnings

    /// <summary>A representation containing a mesh.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MeshRepresentation : Representation
    {
        [Newtonsoft.Json.JsonConstructor]
        public MeshRepresentation(Mesh @mesh, Material @material, System.Guid @id, string @name)
        	: base(material, id, name)
        {
        	var validator = Validator.Instance.GetFirstValidatorForType<MeshRepresentation>();
        	if(validator != null)
        	{
        		validator.PreConstruct(new object[]{ @mesh, @material, @id, @name});
        	}
        
        	this.Mesh = @mesh;
        	
        	if(validator != null)
        	{
        		validator.PostConstruct(this);
        	}
        }
    
        /// <summary>A mesh.</summary>
        [Newtonsoft.Json.JsonProperty("Mesh", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Mesh Mesh { get; set; } = new Mesh();
    
    
    }
}