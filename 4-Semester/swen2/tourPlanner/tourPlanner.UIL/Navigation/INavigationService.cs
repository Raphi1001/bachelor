using tourPlanner.UIL.ViewModels;

namespace tourPlanner.UIL.Navigation
{
    public enum NavigationMode
    {
        Modal,
        Modeless
    }

    public interface INavigationService
    {
        bool? NavigateTo<TViewModel>(TViewModel viewModel, NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel;
        bool? NavigateTo<TViewModel>(NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel;
    }
}
