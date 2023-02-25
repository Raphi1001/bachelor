using tourPlanner.Models.Route;

namespace tourPlanner.Models.Tour
{
    public class TourTransfere
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RouteTransfere? Route {get; set;} 
        public DateOnly? CreationDate { get; set; }
        public string? ImagePath { get; set; } = "";

        public TourInternal ToInternal()
        {
            return new TourInternal(Id, Name, Description, Route?.ToInternal(), CreationDate, ImagePath);
        }
    }
}