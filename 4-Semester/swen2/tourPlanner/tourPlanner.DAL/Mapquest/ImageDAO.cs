using tourPlanner.Logging;
using tourPlanner.DAL.Exceptions;
using tourPlanner.DAL.Configuration;
using tourPlanner.Models.Tour;

namespace tourPlanner.DAL.Mapquest
{
    public class ImageDAO : IImageDAO
    {
        protected readonly ILogger _logger;
        private readonly string IMAGE_PATH;
        private readonly string GENERAL_PATH;

        public ImageDAO(IDirectoryConfiguration config, ILogManager logManager)
        {
            _logger = logManager.GetLogger<ImageDAO>();

            GENERAL_PATH = config.FileDirectory;
            IMAGE_PATH = $"{GENERAL_PATH}\\Images";

            if (!Directory.Exists($"{IMAGE_PATH}"))
                Directory.CreateDirectory($"{IMAGE_PATH}");
        }

        public string SaveImage(byte[]? byteStream)
        {
            FileStream? fs = null;
            Guid guid = Guid.NewGuid();

            while (Directory.Exists($"{IMAGE_PATH}\\{guid}.png"))
                guid = Guid.NewGuid();

            try
            {
                fs = File.Create($"{IMAGE_PATH}\\{guid}.png");
                fs.Write(byteStream);
                _logger.Debug($"Image [{guid}.png] was created.");
            }
            catch (Exception e)
            {
                _logger.Error($"TourPlanner.DAL.Mapquest - Error: {e.Message}.");
                throw new ImageCreationFailedException(e.Message);
            }
            finally
            {
                if(fs is not null)
                    fs.Close();
            }

            return $"{IMAGE_PATH}\\{guid}.png";
        }

        public void DeleteImage(string imgPath)
        {
            
            if(File.Exists(imgPath))
            {
                //try
               // {
                    File.Delete(imgPath);
              //  }
              //  catch(Exception)
             //   {
                    _logger.Error($"Image [{imgPath}] cannot be removed.");

            //    }
            }
            else
            {
                _logger.Debug($"Image [{imgPath}] does not exist.");
            }
        }

        public bool ImageExists(TourInternal tour)
        {
            return File.Exists(tour.ImagePath);
        }
    }
}
