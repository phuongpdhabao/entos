using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Application.Features.TextToSpeech
{
    public class TextToSpeechCommand
    {
        public string Text { get; set; }

        public float VowelSpeed { get; set; }

        public string Voice { get; set; }

        public string Language { get; set; }


        public string ResultUrl { get; set; }

        public byte[] ResultContent { get; set; }
        public Application.DTOs.DataServiceDto Provider { get; set; }

        public bool IsSuccess { get; set; } = false;
    }

}
