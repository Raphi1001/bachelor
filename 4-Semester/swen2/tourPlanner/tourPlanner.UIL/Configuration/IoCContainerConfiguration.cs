using System;
using tourPlanner.BL;
using System.Net.Http;
using tourPlanner.Logging;
using tourPlanner.Log4Net;
using tourPlanner.UIL.Views;
using tourPlanner.BL.Report;
using tourPlanner.BL.Mapquest;
using tourPlanner.DAL.Mapquest;
using tourPlanner.UIL.Navigation;
using tourPlanner.UIL.ViewModels;
using tourPlanner.DAL.Configuration;
using tourPlanner.DAL.TourRepository;
using tourPlanner.DAL.TourLogRepository;
using Microsoft.Extensions.Configuration;
using tourPlanner.BL.Managers.ApiManagers;
using tourPlanner.BL.Managers.ToursManagers;
using Microsoft.Extensions.DependencyInjection;
using tourPlanner.BL.Managers.TourLogsManagers;

namespace tourPlanner.UIL.Configuration
{
    internal class IoCContainerConfiguration
    {
        private readonly IServiceProvider serviceProvider;

        public IoCContainerConfiguration()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>((_) =>
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
            });

            /* CONFIGURATION SETUP */
            services.AddSingleton<AppConfiguration>();
            services.AddSingleton<IDatabaseConfiguration>(s => s.GetRequiredService<AppConfiguration>());
            services.AddSingleton<IGeneratorConfigurator>(s => s.GetRequiredService<AppConfiguration>());
            services.AddSingleton<IDirectoryConfiguration>(s => s.GetRequiredService<AppConfiguration>());

            /* LOGGER SETUP */
            services.AddSingleton<ILogManager, LogManager>();
            services.AddSingleton<ILoggerFactory, Log4NetFactory>();

            /* DATA LAYER SETUP */
            services.AddSingleton<IFileDAO, FileDAO>();
            services.AddSingleton<IImageDAO, ImageDAO>();
            services.AddSingleton<ITourRepository,DBTourRepository>();
            services.AddSingleton<ITourLogRepository, DBTourLogRepository>();

            /* BUSINESS LAYER SETUP */
            services.AddTransient<HttpClient>();
            services.AddSingleton<IToursManager, ToursManager>();
            services.AddSingleton<IRouteManager, RouteManager>();
            services.AddSingleton<IRouteGenerator, RouteGenerator>();
            services.AddSingleton<ITourLogsManager, TourLogsManager>();           
            services.AddSingleton<IStaticMapManager, StaticMapManager>();
            services.AddSingleton<IStaticMapGenerator, StaticMapGenerator>();            
            services.AddSingleton<ITourReportGenerator, TourReportGenerator>();

            /* USER INTERFACE SETUP */
            services.AddSingleton<INavigationService, NavigationService>(s =>
            {
                var navigationService = new NavigationService(s);
                navigationService.RegisterNavigation<AboutViewModel, AboutDialog>();
                navigationService.RegisterNavigation<ErrorViewModel, ErrorDialog>();
                navigationService.RegisterNavigation<AddTourViewModel, AddTourDialog>();
                navigationService.RegisterNavigation<AddRouteViewModel, AddRouteDialog>();
                navigationService.RegisterNavigation<AddTourLogViewModel, AddTourLogDialog>();
                navigationService.RegisterNavigation<EditTourViewModel, EditTourDialog>();
                navigationService.RegisterNavigation<EditTourLogViewModel, EditTourLogDialog>();

                navigationService.RegisterNavigation<MainViewModel, MainWindow>((viewModel, window) =>
                {
                    window.SearchBar.DataContext = viewModel.SearchViewModel;
                    window.MenuBar.DataContext = viewModel.MenuViewModel;
                    window.TourList.DataContext = viewModel.TourListViewModel;
                    window.TourDetail.DataContext = viewModel.TourDetailViewModel;
                    window.TourLogs.DataContext = viewModel.TourLogsViewModel;
                });
      
                return navigationService;
            });            

            services.AddTransient<TourListViewModel>();
            services.AddTransient<TourDetailViewModel>();
            services.AddTransient<TourLogsViewModel>();

            services.AddTransient<MenuViewModel>();
            services.AddTransient<SearchViewModel>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<ErrorViewModel>();
            services.AddTransient<AddTourViewModel>();
            services.AddTransient<AddRouteViewModel>();
            services.AddTransient<AddTourLogViewModel>();
            services.AddTransient<EditTourViewModel>();
            services.AddTransient<EditTourLogViewModel>();

            // finished
            serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => serviceProvider.GetRequiredService<MainViewModel>();
        public MenuViewModel MenuViewModel => serviceProvider.GetRequiredService<MenuViewModel>();
        public SearchViewModel SearchViewModel => serviceProvider.GetRequiredService<SearchViewModel>();
        public AddTourViewModel AddTourViewModel => serviceProvider.GetRequiredService<AddTourViewModel>();
        public AddRouteViewModel AddRouteViewModel => serviceProvider.GetRequiredService<AddRouteViewModel>();
        public EditTourViewModel EditTourViewModel => serviceProvider.GetRequiredService<EditTourViewModel>();
        public TourListViewModel TourListViewModel => serviceProvider.GetRequiredService<TourListViewModel>();
        public TourLogsViewModel TourLogsViewModel => serviceProvider.GetRequiredService<TourLogsViewModel>();
        public TourDetailViewModel TourDetailViewModel => serviceProvider.GetRequiredService<TourDetailViewModel>();
        public AddTourLogViewModel AddTourLogViewModel => serviceProvider.GetRequiredService<AddTourLogViewModel>();
        public EditTourLogViewModel EditTourLogViewModel => serviceProvider.GetRequiredService<EditTourLogViewModel>();
        public ErrorViewModel ErrorViewModel => serviceProvider.GetRequiredService<ErrorViewModel>();

        public INavigationService NavigationService => serviceProvider.GetRequiredService<INavigationService>();
    }
}
