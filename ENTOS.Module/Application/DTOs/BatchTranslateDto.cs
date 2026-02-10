using System;
using System.Linq;using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Application.DTOs
{

    public partial class BatchTranslateDto :  IEquatable<BatchTranslateDto>
    {

  //
        public Guid Oid { get; set; }
               

		public string Content { get; set; }            
       


        public override bool Equals(object obj)
        => Equals(obj as BatchTranslateDto);

        public bool Equals(BatchTranslateDto other)
            => other != null && Oid == other.Oid;

        public override int GetHashCode()
            => Oid.GetHashCode();

	}
}
