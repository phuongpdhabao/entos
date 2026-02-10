using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class VoiceDto :  IEquatable<VoiceDto>
    {

  //
        public Guid Oid { get; set; }
               

		public string Code { get; set; }            
       
		public Gender Gender { get; set; }            
       
		public System.Guid? LanguageOid { get; set; }            
       
		public decimal VowelSpeed { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as VoiceDto);

        public bool Equals(VoiceDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
