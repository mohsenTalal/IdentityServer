using ImageGallery.Model;
using System.Collections.Generic;

namespace TextGallery.Client
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<Image> Images { get; private set; }
            = new List<Image>();

        public GalleryIndexViewModel(IEnumerable<Image> images)
        {
           Images = images;
        }
    }
}
