using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class AudioDto :  IEquatable<AudioDto>
    {

  //
        public Guid Oid { get; set; }
               

		public TimeSpan? Start { get; set; }            
       
		public TimeSpan? End { get; set; }            
       
		public string Content { get; set; }            
       
		public string Subtitle { get; set; }            
       
		
		public System.Collections.Generic.List<System.Guid?> TermLocationList { get; set; }     
       
		
		public System.Collections.Generic.List<Application.DTOs.MediaDto> MediaList { get; set; }     
       
		public System.Guid? VideoOid { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as AudioDto);

        public bool Equals(AudioDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
