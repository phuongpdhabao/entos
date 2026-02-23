using System;
using System.Collections.Generic;
using System.Linq;

namespace ENTOS.Module.Services
{
    public partial class MediaService
    {
        private void CalculateTextWord(Media media)
        {
            if (media.Text == null)
                return;

            media.Quantity = media.Text
                .Split(new[] { ' ', '\t', '\n', '\r' },
                       StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }

        private void CalculateSameGroup(
            Media media,
            Func<Media, List<Media>> getRelatedMedias)
        {
            if (media.UpperMedia == null)
                return;

            var list = getRelatedMedias(media.UpperMedia);
            media.Quantity = list.Count;
        }

        private void CalculateChildElement(
            Media media,
            Func<Media, List<Media>> getRelatedMedias)
        {
            if (media.MediaType != MediaType.Group)
                return;

            var list = getRelatedMedias(media);
            media.Quantity = list.Count;
        }

        private void CalculateChildTextbox(
            Media media,
            Func<Media, List<Media>> getRelatedMedias)
        {
            if (media.MediaType != MediaType.Group)
                return;

            var list = getRelatedMedias(media);

            media.Quantity = list.Count(x =>
                x.MediaType == MediaType.TextBox);
        }
    }
}
