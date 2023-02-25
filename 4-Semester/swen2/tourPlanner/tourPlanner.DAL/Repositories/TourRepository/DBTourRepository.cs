using Npgsql;
using System.Data;
using tourPlanner.Logging;
using tourPlanner.Models.Tour;
using tourPlanner.Models.Route;
using tourPlanner.DAL.Repositories;
using tourPlanner.DAL.Configuration;
using tourPlanner.DAL.Exceptions;

namespace tourPlanner.DAL.TourRepository
{
    public class DBTourRepository : Repository, ITourRepository
    {
        private const string CreateTableCommand = @"CREATE TABLE IF NOT EXISTS tours 
                                                    (
                                                        tour_id                 uuid        primary key, 
                                                        name                    varchar(50), 
                                                        description             varchar(250), 
                                                        start_point             varchar(75), 
                                                        end_point               varchar(75), 
                                                        distance_km             numeric, 
                                                        transport_type          integer, 
                                                        planned_duration_s      integer, 
                                                        route_information_path  varchar(250), 
                                                        creation_date           date,
                                                        ul_lat                  numeric,
                                                        ul_lng                 numeric,
                                                        lr_lat                  numeric,
                                                        lr_lng                 numeric
                                                    );";

        private const string DeleteTourCommand      = "DELETE FROM tours WHERE tour_id=@tour_id";
        private const string SelectTourCommand      = "SELECT tour_id, name, description, start_point, end_point, distance_km, transport_type, planned_duration_s, route_information_path, creation_date, ul_lat, ul_lng, lr_lat, lr_lng FROM tours WHERE tour_id=@tour_id";
        private const string InsertTourCommand      = "INSERT INTO tours(tour_id, name, description, start_point, end_point, distance_km, transport_type, planned_duration_s, route_information_path, creation_date, ul_lat, ul_lng, lr_lat, lr_lng) VALUES (@tour_id, @name, @description, @start_point, @end_point, @distance_km, @transport_type, @planned_duration_s, @route_information_path, @creation_date, @ul_lat, @ul_lng, @lr_lat, @lr_lng) ON CONFLICT (tour_id) DO NOTHING";
        private const string UpdateTourCommand      = "UPDATE tours SET name=@name, description=@description, start_point=@start_point, end_point=@end_point, distance_km=@distance_km, transport_type=@transport_type, planned_duration_s=@planned_duration_s, route_information_path=@route_information_path, creation_date=@creation_date, ul_lat=@ul_lat, ul_lng=@ul_lng, lr_lat=@lr_lat, lr_lng=@lr_lng  WHERE tour_id=@tour_id";
        private const string SelectAllToursCommand  = "SELECT tour_id, name, description, start_point, end_point, distance_km, transport_type, planned_duration_s, route_information_path, creation_date, ul_lat, ul_lng, lr_lat, lr_lng FROM tours";

        public DBTourRepository(IDatabaseConfiguration configuration, ILogManager logManager) : base(configuration, logManager)
        {
            EnsureTables();
        }

        protected override void EnsureTables()
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(CreateTableCommand, connection);
                _logger.Debug("Tours-Table ensured");
                return cmd.ExecuteNonQuery();
            });
        }

        public IEnumerable<TourTransfere> GetAllTours()
        {
            return ExecuteWithConnection(connection =>
            {
                IEnumerable<TourTransfere> tours = new List<TourTransfere>();

                using (var cmd = new NpgsqlCommand(SelectAllToursCommand, connection))
                {
                    // take all rows, if any
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TourTransfere currentTour = ReadTour(reader);
                        tours = tours.Append(currentTour);
                    }
                }

                return tours;
            });
        }

        public TourTransfere? GetTourById(Guid id)
        {
            TourTransfere? tour = null;

            return ExecuteWithConnection(connection =>
            {
                using (var cmd = new NpgsqlCommand(SelectTourCommand, connection))
                {
                    cmd.Parameters.AddWithValue("tour_id", id);

                    // take the first row, if any
                    using var reader = cmd.ExecuteReader();

                    if (reader.Read())
                        tour = ReadTour(reader);

                }

                return tour;
            });
        }

        public bool InsertTour(TourInternal tour)
        {       
            return ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(InsertTourCommand, connection);
                cmd.Parameters.AddWithValue("tour_id", tour.Id);
                cmd.Parameters.AddWithValue("name", tour.Name);
                cmd.Parameters.AddWithValue("description", tour.Description);
                cmd.Parameters.AddWithValue("start_point", tour.Route.From);
                cmd.Parameters.AddWithValue("end_point", tour.Route.To);
                cmd.Parameters.AddWithValue("distance_km", tour.Route.Distance);
                cmd.Parameters.AddWithValue("transport_type", (int) tour.Route.RouteType);
                cmd.Parameters.AddWithValue("planned_duration_s", tour.Route.PlannedDurationS);
                cmd.Parameters.AddWithValue("route_information_path", tour.ImagePath);
                cmd.Parameters.AddWithValue("creation_date", tour.CreationDate);
                cmd.Parameters.AddWithValue("ul_lat", tour.Route.Ul.Latitude);
                cmd.Parameters.AddWithValue("ul_lng", tour.Route.Ul.Longitude);
                cmd.Parameters.AddWithValue("lr_lat", tour.Route.Lr.Latitude);
                cmd.Parameters.AddWithValue("lr_lng", tour.Route.Lr.Longitude);

                var result = cmd.ExecuteNonQuery();
                
                if (result > 0)
                {
                    _logger.Debug($"Tour [{tour.Id}] was added to Tours-Table");
                    return true;
                }

                _logger.Debug($"Tour [{tour.Id}] was unable to be added to Tours-Table");
                return false;
            });
        }
        public void DeleteTour(Guid id)
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(DeleteTourCommand, connection);
                cmd.Parameters.AddWithValue("tour_id", id);

                _logger.Debug($"Tour [{id}] was removed from Tours-Table");
                return cmd.ExecuteNonQuery();
            });
        }

        public bool UpdateTour(TourInternal tour)
        {     
            return ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(UpdateTourCommand, connection);
                cmd.Parameters.AddWithValue("tour_id", tour.Id);
                cmd.Parameters.AddWithValue("name", tour.Name);
                cmd.Parameters.AddWithValue("description", tour.Description);
                cmd.Parameters.AddWithValue("start_point", tour.Route.From);
                cmd.Parameters.AddWithValue("end_point", tour.Route.To);
                cmd.Parameters.AddWithValue("distance_km", tour.Route.Distance);
                cmd.Parameters.AddWithValue("transport_type", (int)tour.Route.RouteType);
                cmd.Parameters.AddWithValue("planned_duration_s", tour.Route.PlannedDurationS);
                cmd.Parameters.AddWithValue("route_information_path", tour.ImagePath);
                cmd.Parameters.AddWithValue("creation_date", tour.CreationDate);
                cmd.Parameters.AddWithValue("ul_lat", tour.Route.Ul.Latitude);
                cmd.Parameters.AddWithValue("ul_lng", tour.Route.Ul.Longitude);
                cmd.Parameters.AddWithValue("lr_lat", tour.Route.Lr.Latitude);
                cmd.Parameters.AddWithValue("lr_lng", tour.Route.Lr.Longitude);

                var result = cmd.ExecuteNonQuery();

                return result > 0;
            });
        }


        private TourTransfere ReadTour(IDataRecord record)
        {
            try
            {
                RouteTransfere route = new RouteTransfere(
                    record["end_point"].ToString(),
                    record["start_point"].ToString(),
                    Convert.ToDouble(record["distance_km"]),
                    record["transport_type"].ToString(),
                    Convert.ToInt32(record["planned_duration_s"]),
                    Convert.ToDouble(record["ul_lng"]),
                    Convert.ToDouble(record["ul_lat"]),
                    Convert.ToDouble(record["lr_lng"]),
                    Convert.ToDouble(record["lr_lat"])

                );

                return new TourTransfere()
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    Id = string.IsNullOrEmpty(record["tour_id"].ToString()) ? null : Guid.Parse(record["tour_id"].ToString()), /* warning is wrong here */
#pragma warning restore CS8604 // Possible null reference argument.

                    Name = record["name"].ToString(),
                    Description = record["description"].ToString(),
                    Route = route,
                    CreationDate = DateOnly.FromDateTime(DateTime.Parse(record["creation_date"].ToString())),
                    ImagePath = record["route_information_path"].ToString()
                };
            }
            catch (Exception e)
            {
                _logger.Error($"Database Error. ErrMsg: [{e.Message}]");
                throw new DatabaseErrorException();
            }
        }

    }
}