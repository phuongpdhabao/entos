using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class LanguageDto :  IEquatable<LanguageDto>
    {

  //
        public Guid Oid { get; set; }
               

		public string Name { get; set; }            
       
		public string EnglishName { get; set; }            
       
		public string OriginName { get; set; }            
       
		public string Code { get; set; }            
       
		public string LocaleCode { get; set; }            
       
		public int? Speaker { get; set; }            
       
		public System.Guid? CountryOid { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as LanguageDto);

        public bool Equals(LanguageDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
