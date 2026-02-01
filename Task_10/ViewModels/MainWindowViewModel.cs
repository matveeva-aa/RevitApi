using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using System.Collections.ObjectModel;
using Task_10.Abstractions;
using Task_10.Models;

namespace Task_10.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IPlacementService _placementService;
        private TreeType _selectedTreeType;
        private int _count;
        private string _statusMessage;

        public MainWindowViewModel(IPlacementService placementService)
        {
            PlaceCommand = new RelayCommand(PlaceTree);

            TreeTypes = new ObservableCollection<TreeType>
            {
                TreeType.Birch,
                TreeType.Apple,
                TreeType.Cherry
            };

            _placementService = placementService;
        }

        public ObservableCollection<TreeType> TreeTypes { get; }
        public TreeType SelectedTreeType

        { get => _selectedTreeType;
          set => SetProperty(ref _selectedTreeType, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
            
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand PlaceCommand {get;}

        private void PlaceTree()
        {
            CSharpFunctionalExtensions.Result result = _placementService.Place(SelectedTreeType, Count);
            if (result.IsSuccess)
            {
                StatusMessage = $"Размещено {Count} экземпляров деревьев";
                TaskDialog.Show("Размещение деревьев", StatusMessage);
            }
            else
            {
                StatusMessage = $"Ошибка {result.Error}";
                TaskDialog.Show("Размещение деревьев", result.Error);
            }
             

        }
    }
}
