using ENTOS.Domain.Interfaces;
using ENTOS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Application.Features.TextToSpeech
{
    public interface ITextToSpeechServiceFactory
    {
        ITextToSpeechDataService GetService(Application.DTOs.DataServiceDto provider);
    }

}
