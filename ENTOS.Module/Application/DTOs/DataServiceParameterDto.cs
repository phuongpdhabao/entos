using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class DataServiceParameterDto :  IEquatable<DataServiceParameterDto>
    {

  //
        public Guid Oid { get; set; }
               

		public DataServiceParameterType Type { get; set; }            
       
		public string Name { get; set; }            
       
		public string Value { get; set; }            
       
		public DataServiceParameterOption DataServiceParameterOption { get; set; }            
       
		public int? Order { get; set; }            
       
		public bool InActive { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as DataServiceParameterDto);

        public bool Equals(DataServiceParameterDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
