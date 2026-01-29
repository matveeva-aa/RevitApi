using System.Windows.Input;
using Task_08.Abstractions;
using Task_08.Models;
using Task_08;

namespace Task_08.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IWallSelectionService _selectionService;
        private readonly IWallGeometryService _geometryService;

        private WallInfo _wallInfo;
        private double _thicknessLimitMm = 300;

        public MainWindowViewModel(IWallSelectionService selectionService, IWallGeometryService geometryService)
        {
            _selectionService = selectionService;
            _geometryService = geometryService;

            SelectWallCommand = new RelayCommand(_ => SelectWall());
        }

        public ICommand SelectWallCommand { get; }

        public WallInfo WallInfo
        {
            get => _wallInfo;
            set
            {
                _wallInfo = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ThicknessOk));
            }
        }

        public double ThicknessLimitMm
        {
            get => _thicknessLimitMm;
            set
            {
                _thicknessLimitMm = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ThicknessOk));
            }
        }

        public bool? ThicknessOk
        {
            get
            {
                if (WallInfo == null) return null;
                return WallInfo.ThicknessMm <= ThicknessLimitMm;
            }
        }

        private void SelectWall()
        {
            var wall = _selectionService.PickWall();
            if (wall == null) return;

            WallInfo = _geometryService.GetWallInfo(wall);
        }
    }
}
