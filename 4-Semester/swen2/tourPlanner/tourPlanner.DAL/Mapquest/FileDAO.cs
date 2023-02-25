using Newtonsoft.Json;
using tourPlanner.Logging;
using tourPlanner.Models.Tour;
using tourPlanner.DAL.Exceptions;
using tourPlanner.DAL.Configuration;

namespace tourPlanner.DAL.Mapquest
{
    public class FileDAO : IFileDAO
    {
        private readonly string EXPORT_PATH;
        private readonly string IMPORT_PATH;
        private readonly string GENERAL_PATH;

        private readonly ILogger logger;

        public FileDAO(IDirectoryConfiguration config, ILogManager logManager)
        {
            logger = logManager.GetLogger<FileDAO>();

            GENERAL_PATH = config.FileDirectory;

            EXPORT_PATH = $"{GENERAL_PATH}\\Exports";

            if (!Directory.Exists($"{EXPORT_PATH}"))
                Directory.CreateDirectory($"{EXPORT_PATH}");

            IMPORT_PATH = $"{GENERAL_PATH}\\Imports";

            if (!Directory.Exists($"{EXPORT_PATH}"))
                Directory.CreateDirectory($"{IMPORT_PATH}");
        }

        public async void ExportTour(TourInternal tour)
        {
            var dto = tour.ToTransfere();
           
            string file = $"{EXPORT_PATH}\\TourExport_{dto.Id}.json";
            string filePath = Path.GetFullPath(file);

            string output = JsonConvert.SerializeObject(dto, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, output);
        }

        public TourTransfere ImportTour(string filePath)
        {
            if(File.Exists(filePath))
            {
                using StreamReader f = File.OpenText(filePath);
                string test = f.ReadToEnd();

                TourTransfere? tour = JsonConvert.DeserializeObject<TourTransfere>(test);

                if (tour is null)
                    throw new RouteImportException("No tours could be deserialized");

                return tour;
            }
            else
            {
                logger.Error($"Error Import: File [{filePath}] doesn't exist.");
                throw new InvalidImportFileException();
            }
        }
    }
}
