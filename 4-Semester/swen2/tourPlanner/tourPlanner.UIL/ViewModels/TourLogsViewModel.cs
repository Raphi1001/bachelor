using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tourPlanner.BL;
using System.Windows.Input;
using tourPlanner.Models.TourLog;
using tourPlanner.BL.Managers.TourLogsManagers;
using tourPlanner.Models.Enums;

namespace tourPlanner.UIL.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        private readonly ITourLogsManager tourLogsManager;

        public ICommand AddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public Guid? TourId { get; set; }

        public event EventHandler<TourLogInternal?>? SelectedItemChanged;
        public ObservableCollection<TourLogInternal> Items { get; } = new ObservableCollection<TourLogInternal>();

        private TourLogInternal? selectedItem;
        public TourLogInternal? SelectedItem
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
            }
        }

        public IEnumerable<TourLogInternal> GetItems(Guid? tourId)
        {
            TourId = tourId;
            IEnumerable<TourLogInternal> tourLogs = new List<TourLogInternal>();
            if (tourId is not null)
            {
                tourLogs = tourLogsManager.GetTourLogs((Guid) tourId);
            }

            SetItems(tourLogs);

            return Items;
        }
        public void SetItems(IEnumerable<TourLogInternal> tourLogs)
        {
            Items.Clear();
            tourLogs.ToList().ForEach(j => Items.Add(j));
        }

        public void RemoveItem(TourLogInternal tourLog)
        {
            Items.Remove(tourLog);
        }
        public void UpdateItem(TourLogInternal tourLog)
        {
            try
            {
                Items[Items.IndexOf(Items.First(item => item.Id.Equals(tourLog.Id)))] = tourLog;
            }
            catch (InvalidOperationException)
            {
                //id not found in list, we continue without trying to update, no further exception handeling necessary  
            }
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


        public void AddItem(TourLogInternal tourLog)
        {
            if (TourId == tourLog.TourId)
            {
                Items.Add(tourLog);
            }
        }

        public void DeleteTourLog(Guid? id)
        {
            if (id is null)
            {
                return;
            }
            tourLogsManager.DeleteTourLog((Guid)id);
            RemoveItemById((Guid)id);
            if (SelectedItem?.Id == id)
            {
                SelectedItem = null;
            }
        }

        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }

        public TourLogsViewModel(ITourLogsManager tourLogsManager)
        {
            this.tourLogsManager = tourLogsManager;

            AddTourLogCommand = new RelayCommand((_) =>
            {
                IsBusy = true;
                //IEnumerable<TourLogInternal> tours = tourLogsManager.();

                IsBusy = false;
            }, (_) => IsBusy == false);

            DeleteTourLogCommand = new RelayCommand((_) =>
            {
                DeleteTourLog(SelectedItem?.Id);            
            }, (_) => SelectedItem != null);
        }

    }
}
