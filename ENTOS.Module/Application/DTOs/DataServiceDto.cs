using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class DataServiceDto :  IEquatable<DataServiceDto>
    {

  //
        public Guid Oid { get; set; }
               

		public string Address { get; set; }            
       
		public string ServiceCode { get; set; }            
       
		public ApiMethodType ApiMethodType { get; set; }            
       
		public string APIKey { get; set; }            
       
		public int? MaxConcurrency { get; set; }            
       
		
		public System.Collections.Generic.List<Application.DTOs.DataServiceParameterDto> DataServiceParameterList { get; set; }     
       


        public override bool Equals(object obj)
        => Equals(obj as DataServiceDto);

        public bool Equals(DataServiceDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
