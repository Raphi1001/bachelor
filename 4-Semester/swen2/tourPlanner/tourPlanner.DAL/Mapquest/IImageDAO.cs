using tourPlanner.Models.Tour;

namespace tourPlanner.DAL.Mapquest
{
    public interface IImageDAO
    {
        string SaveImage(byte[]? byteStream);
        void DeleteImage(string imgPath);
        bool ImageExists(TourInternal tour);

    }
}