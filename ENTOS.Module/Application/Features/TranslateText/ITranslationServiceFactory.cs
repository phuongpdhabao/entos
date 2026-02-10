using ENTOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Application.Features.TranslateText
{
    public interface ITranslationServiceFactory
    {
        ITranslationDataService GetService(Application.DTOs.DataServiceDto provider);
    }
}
