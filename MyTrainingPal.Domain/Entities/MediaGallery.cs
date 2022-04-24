using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.Domain.Entities
{
    public class MediaGallery : BaseEntity
    {
        public string GalleryName { get; private set; }
        public List<string> Files { get; private set; } = new List<string>();

        public MediaGallery(string galleryName)
        {
            GalleryName = galleryName;
        }

        public Result AddExternalFile(Uri fileAbsoluteURL)
        {
            if (fileAbsoluteURL == null)
                return Result.Fail("Url must be provided.");

            if (!fileAbsoluteURL.IsAbsoluteUri)
                return Result.Fail("Must be absolute url.");

            if (fileAbsoluteURL.Scheme != "https")
                return Result.Fail("HTTPS required for the url.");

            return Result.Ok();
        }

        public Result AddExternalFiles(List<Uri> listOfAbsoluteURLs)
        {           
            if(listOfAbsoluteURLs == null || listOfAbsoluteURLs.Count == 0) 
                return Result.Fail("A list of URLs must be provided.");

            Dictionary<bool, List<Uri>> result = new Dictionary<bool, List<Uri>>();
            result.Add(true, new List<Uri>());
            result.Add(false, new List<Uri>());
            listOfAbsoluteURLs.ForEach(url => result[AddExternalFile(url).IsSuccess].Add(url));

            if (result[false].Count > 0 && result[true].Count == 0)
                return Result.Fail("None of the provided urls were added");

            if (result[false].Count > 0 && result[true].Count > 0)
            {
                string message = "Some of the provided urls were not added: ";
                result[false].ForEach(url => message += $"\n- {url} ");
                message += "\nPlease check that they match the conditions requested.";
                return Result.Fail(message);
            }

            return Result.Ok();
        }
    }
}
