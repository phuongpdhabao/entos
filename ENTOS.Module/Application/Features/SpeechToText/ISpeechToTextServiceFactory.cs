using ENTOS.Domain.Interfaces;
using ENTOS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Application.Features.SpeechToText
{
    public interface ISpeechToTextServiceFactory
    {
        ISpeechToTextDataService GetService(Application.DTOs.DataServiceDto provider);
    }

}
