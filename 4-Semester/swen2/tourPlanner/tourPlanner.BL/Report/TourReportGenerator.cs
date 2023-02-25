using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.IO.Font.Constants;

using tourPlanner.Logging;
using tourPlanner.Models.Tour;
using tourPlanner.Models.TourLog;
using tourPlanner.DAL.Configuration;
using tourPlanner.BL.Managers.TourLogsManagers;

namespace tourPlanner.BL.Report
{
    public class TourReportGenerator : ITourReportGenerator
    {
        protected readonly ILogger _logger;
        private readonly string REPORT_PATH;
        private readonly string GENERAL_PATH;
        private readonly ITourLogsManager _tourManager;

        public TourReportGenerator(ITourLogsManager tourManager, IDirectoryConfiguration config, ILogManager logManager)
        {
            _tourManager = tourManager;
            _logger = logManager.GetLogger<TourReportGenerator>();

            GENERAL_PATH = config.FileDirectory;
            REPORT_PATH = $"{GENERAL_PATH}\\Reports";

            if (!Directory.Exists($"{REPORT_PATH}"))
                Directory.CreateDirectory($"{REPORT_PATH}");
        }

        public void GenerateTourReport(TourInternal tour)
        {
            PdfWriter writer = new PdfWriter($"{REPORT_PATH}\\TourReport_{tour.Id}.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            /* Header + Description */
            document.Add(new Paragraph($"{tour.Name}-{tour.Id}")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(18)
                .SetBold()
                .SetFontColor(ColorConstants.CYAN));

            document.Add(new Paragraph(tour.Description));

            /* Details */
            document.Add(new Paragraph($"{tour.Name} Details")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(15)
                .SetBold()
                .SetFontColor(ColorConstants.CYAN));

            List list = new List()
                .SetSymbolIndent(12)
                .SetListSymbol("\u2022")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD));
            list.Add(new ListItem($"From: {tour.Route.From}"))
                .Add(new ListItem($"To: {tour.Route.To}"))
                .Add(new ListItem($"Route Type: {tour.Route.RouteType}"))
                .Add(new ListItem($"Distance: {tour.Route.Distance}"))
                .Add(new ListItem($"Planned Duration: {tour.Route.PlannedDurationH}"))
                .Add(new ListItem($"Created: {tour.CreationDate}"));

            document.Add(list);

            /* Image */
            Paragraph imageHeader = new Paragraph($"{tour.Name} Static Map")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18)
                .SetBold()
                .SetFontColor(ColorConstants.CYAN);
            document.Add(imageHeader);
            ImageData imageData = ImageDataFactory.Create(tour.ImagePath);
            document.Add(new Image(imageData));
            document.Close();

            _logger.Debug($"Report of Tour[{tour.Id}] is generated.");
        }

        public void GenerateSummarizeReport(TourInternal tour)
        {
            PdfWriter writer = new PdfWriter($"{REPORT_PATH}\\SummarizeReport_{tour.Id}.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            IEnumerable<TourLogInternal> logs = _tourManager.GetTourLogs(tour.Id);

            if (logs.Count() < 1)
            {
                _logger.Error($"Summarize Report for tour[{tour.Id}] couldn't be " +
                    $"created because it doesn't have any logs.");
                return;
            }

            /* Header + Description */
            document.Add(new Paragraph($"{tour.Name}-{tour.Id}: Statistical Analysis")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(18)
                .SetBold()
                .SetFontColor(ColorConstants.MAGENTA));

            /* Averages */
            document.Add(new Paragraph($"{tour.Name} Stats")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(15)
                .SetBold()
                .SetFontColor(ColorConstants.MAGENTA));

            var avgTime = CalculateAvgTime(logs.ToList());
            var avgRating = CalculateAvgRating(logs.ToList());
            var avgDifficulty = CalculateAvgDifficulty(logs.ToList());

            List list = new List()
                .SetSymbolIndent(12)
                .SetListSymbol("\u2022")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD));
            list.Add(new ListItem($"Average Time: {avgTime}"))
                .Add(new ListItem($"Average Rating: {avgRating}"))
                .Add(new ListItem($"Average Difficulty: {avgDifficulty}"));
            document.Add(list);
            document.Close();

            _logger.Debug($"Summarise Report of Tour[{tour.Id}] is generated.");
        }

        // TODO Generic method
        static double CalculateAvgTime(List<TourLogInternal> val)
        {
            int count = 0;
            double totalTime = 0;

            foreach (TourLogInternal log in val)
            {
                totalTime += log.TimeTakenH;
                count++;
            }

            return totalTime / count;
        }

        static int CalculateAvgRating(List<TourLogInternal> val)
        {
            int count = 0;
            int totalDiff = 0;

            foreach (TourLogInternal log in val)
            {
                totalDiff += (int)log.TourRating;
                count++;
            }

            return totalDiff / count;
        }

        static int CalculateAvgDifficulty(List<TourLogInternal> val)
        {
            int count = 0;
            int totalDiff = 0;

            foreach (TourLogInternal log in val)
            {
                totalDiff += (int)log.TourDifficulty;
                count++;
            }

            return totalDiff / count;
        }
    }
}
