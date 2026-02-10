using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class SpaceDto :  IEquatable<SpaceDto>
    {

  //
        public Guid Oid { get; set; }
               

		public string Code { get; set; }            
       
		public string Name { get; set; }            
       
		public string NativeName { get; set; }            
       
		public string DomainName { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as SpaceDto);

        public bool Equals(SpaceDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
