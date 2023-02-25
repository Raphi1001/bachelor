using tourPlanner.Models.Exceptions;
using tourPlanner.Models.Route;

namespace tourPlanner.Models.Tour
{
    public class TourInternal
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateOnly CreationDate { get; private set; }

        private RouteInternal route;
        public RouteInternal Route
        {
            get => route;
            set
            {
                route = value is null ? throw new InvalidParameterException($"The Paramter Route should not be null") : value;
            }
        }

        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value is null ? throw new InvalidParameterException($"The Paramter ImagePath should not be null") : value;
            }                  
        }

        public TourInternal(Guid? id, string? name, string? description, RouteInternal? route, DateOnly? creationDate, string? imagePath)
        {
            Id = id is null ? throw new InvalidParameterException($"The Paramter Id should not be null") : (Guid)id;
            Name = name is null ? throw new InvalidParameterException($"The Paramter Name should not be null") : name;
            Description = description is null ? throw new InvalidParameterException($"The Paramter Description should not be null") : description;
            this.route = route is null ? throw new InvalidParameterException($"The Paramter Route should not be null") : route;
            CreationDate = creationDate is null ? throw new InvalidParameterException($"The Paramter CreationDate should not be null") : (DateOnly) creationDate;
            this.imagePath = imagePath is null ? throw new InvalidParameterException($"The Paramter ImagePath should not be null") : imagePath;
        }

        public TourTransfere ToTransfere()
        {
            return new TourTransfere()
            {
                Id = Id,
                Name = Name,
                Route = Route.ToTransfere(),
                Description = Description,
                CreationDate = CreationDate,
                ImagePath = ImagePath
            };
        }
   }
}