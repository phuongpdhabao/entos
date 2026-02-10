using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class CountryDto : Application.DTOs.SpaceDto, IEquatable<CountryDto>
    {

               

		public string OriginCode { get; set; }            
       
		public string CallingCode { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as CountryDto);

        public bool Equals(CountryDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
