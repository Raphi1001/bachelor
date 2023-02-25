using tourPlanner.Models.Tour;

namespace tourPlanner.BL.Report
{
    public interface ITourReportGenerator
    {
        void GenerateTourReport(TourInternal tour);
        void GenerateSummarizeReport(TourInternal tour);
    }
}