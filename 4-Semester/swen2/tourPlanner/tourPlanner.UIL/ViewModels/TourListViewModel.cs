using tourPlanner.Models.Tour;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using tourPlanner.BL.Managers.ToursManagers;
using System.Threading;

namespace tourPlanner.UIL.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
        public event EventHandler<TourInternal?>? SelectedItemChanged;
        
        private readonly IToursManager toursManager;
        public ObservableCollection<TourInternal> Items { get; } = new ObservableCollection<TourInternal>();

        private TourInternal? selectedItem;
        public TourInternal? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged();
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
            }
        }
        public void SetItems(IEnumerable<TourInternal> tours)
        {
            Items.Clear();
            tours.ToList().ForEach(j => Items.Add(j));
        }

        public void RemoveItem(TourInternal tour)
        {
            Items.Remove(tour);
        }
        public void RemoveItemById(Guid id)
        {
            try
            {
                Items.Remove(Items.First(item => item.Id.Equals(id)));
            }
            catch (InvalidOperationException)
            {
                //id not found in list, we continue without trying to remove, no further exception handeling necessary  
            }
        }
        public void UpdateItem(TourInternal tour)
        {
            try
            {
                Items[Items.IndexOf(Items.First(item => item.Id.Equals(tour.Id)))] = tour;
            }
            catch (InvalidOperationException)
            {
                //id not found in list, we continue without trying to update, no further exception handeling necessary  
            }
        }
        public void AddItem(TourInternal tour)
        {
            Items.Add(tour);
        }
        public void DeleteTour(TourInternal? tour)
        {
            if (tour is null)
            {
                return;
            }
            RemoveItem(tour);

            if (SelectedItem?.Id == tour.Id)
            {
                SelectedItem = null;
            }
            toursManager.DeleteTour(tour);       
        }
        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }

        public void GetItems()
        {
            SetItems(this.toursManager.GetTours());
        }

        public TourListViewModel(IToursManager toursManager)
        {
            
            this.toursManager = toursManager;
            GetItems();      
        }
    }
}
