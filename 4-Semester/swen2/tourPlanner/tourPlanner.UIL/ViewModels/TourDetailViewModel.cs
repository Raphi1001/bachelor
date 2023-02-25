using tourPlanner.Models.Tour;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tourPlanner.BL;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using tourPlanner.DAL.Mapquest;
using tourPlanner.BL.Managers.ToursManagers;
using tourPlanner.Models.TourLog;
using tourPlanner.Models.Enums;

namespace tourPlanner.UIL.ViewModels
{
    public class TourDetailViewModel : BaseViewModel
    {
        private readonly IToursManager toursManager;

        private TourInternal? item;
        public TourInternal? Item
        {
            get => item;
            set
            {
                SetItem(value);
            }
        }

        private ChildFriendlyness? childFriendly;
        public ChildFriendlyness? ChildFriendly
        {
            get => childFriendly;
            set
            {
                childFriendly = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TourLogInternal> Logs { get; set; } = new List<TourLogInternal>();

        public void SetTourLogs(IEnumerable<TourLogInternal> tourLogs)
        {
            Logs = tourLogs;
            OnPropertyChanged("Logs");
            ComputeChildFriendlyness();
        }        
        public void AddTourLog(TourLogInternal tourLog)
        {
            Logs = Logs.Append(tourLog);
            OnPropertyChanged("Logs");
            ComputeChildFriendlyness();
        }        
        public void RemoveTourLog(TourLogInternal tourLog)
        {
            var logsList = Logs.ToList();
            logsList.Remove(tourLog);
            Logs = logsList;
            OnPropertyChanged("Logs");
            ComputeChildFriendlyness();
        }

        public void ComputeChildFriendlyness()
        {
            if(Item == null)
            {
                return;
            }

            double value = Item.Route.Distance / 10;

            foreach (var log in Logs)
            {
                value +=(int) log.TourDifficulty * 2; //max 10
                value += log.TimeTakenH;
            }

            value /= Logs.Count();

            if(value > 15)
            {
                ChildFriendly = ChildFriendlyness.veryUnfriendly;
            }
            else if(value > 12)
            {
                ChildFriendly = ChildFriendlyness.unfriendly;

            }
            else if (value > 10)
            {
                ChildFriendly = ChildFriendlyness.medium;

            }
            else if (value > 7)
            {              
                ChildFriendly = ChildFriendlyness.friendly;
            }
            else if (value > 5)
            {
                ChildFriendly = ChildFriendlyness.veryFriendly;
            }

        }

        private async void SetItem(TourInternal? tour)
        {
            if (tour is not null)
            {
                await toursManager.EnsureTourImage(tour);
            }

            item = tour;
            OnPropertyChanged("Item");
        }
        public TourDetailViewModel(IToursManager toursManager)
        {
            this.toursManager = toursManager;
        }
    }
}
