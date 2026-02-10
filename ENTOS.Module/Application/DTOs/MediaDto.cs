using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class MediaDto :  IEquatable<MediaDto>
    {

  //
        public Guid Oid { get; set; }
               

		public TimeSpan? Start { get; set; }            
       
		public string Content { get; set; }            
       
		public TimeSpan? MediaStart { get; set; }            
       
		public int? ShapeId { get; set; }            
       
		public string ShapeName { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as MediaDto);

        public bool Equals(MediaDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
