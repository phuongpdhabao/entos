using ENTOS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Domain.Interfaces
{
    public interface IDataServiceHandle
    {
        bool CanHandle(Application.DTOs.DataServiceDto dataServiceDto);

    }
}
