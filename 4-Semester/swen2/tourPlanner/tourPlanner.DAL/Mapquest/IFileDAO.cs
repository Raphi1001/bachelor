using tourPlanner.Models.Tour;

namespace tourPlanner.DAL.Mapquest
{
    public interface IFileDAO
    {
        void ExportTour(TourInternal dto);
        TourTransfere ImportTour(string filePath);
    }
}