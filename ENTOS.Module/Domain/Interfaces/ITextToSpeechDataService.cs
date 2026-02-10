using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Domain.Interfaces
{
    public interface ITextToSpeechDataService : IDataServiceHandle
    {
        /// <returns>Byte array chứa file âm thanh (ví dụ .mp3)</returns>
        //Task<byte[]> SynthesizeAsync(Application.DTOs.DataServiceDto dataServiceDto, string text, string voice, string language);

        Task TextToSpeechObjectsAsync(Application.DTOs.DataServiceDto dataServiceDto, IList<Application.Features.TextToSpeech.TextToSpeechCommand> items, Module.SystemObjects.LongTaskContext context = null, int? currentItem = null, int? total = null);

    }
}
