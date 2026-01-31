using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Task_09.Abstractions;
using System.Threading.Tasks;

namespace Task_09.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {

        private readonly ISelectionService _selectionService;
        private readonly ISectionService _sectionService;
        private readonly RevitTask _revitTask;
        private string _sectionName = "Разрез 1";
        private string _sectionName2 = "Разрез 2";
        private string _sectionName3 = "Разрез 3";
        private double _widthOffsetMm = 100;
        private double _depthOffsetMm = 100;
        private double _heightOffsetMm = 100;

        public MainWindowViewModel(
            ISelectionService selectionServise,
            ISectionService sectionService,
            RevitTask revitTask
            )
        {
            CreateSectionCommand = new AsyncRelayCommand(OnCreateSelectionComandExecute);
            _selectionService = selectionServise;
            _sectionService = sectionService;
            _revitTask = revitTask;
        }

        public string SectionName
        {
            get => _sectionName;
            set => SetProperty(ref _sectionName, value);
        }

        public string SectionName2
        {
            get => _sectionName2;
            set => SetProperty(ref _sectionName2, value);
        }

        public string SectionName3
        {
            get => _sectionName3;
            set => SetProperty(ref _sectionName3, value);
        }

        public double WidthOffsetMm
        {
            get => _widthOffsetMm;
            set => SetProperty(ref _widthOffsetMm, value);
        }

        public double DepthOffsetMm
        {
            get => _depthOffsetMm;
            set => SetProperty(ref _depthOffsetMm, value);
        }

        public double HeightOffsetMm
        {
            get => _heightOffsetMm;
            set => SetProperty(ref _heightOffsetMm, value);
        }

        public AsyncRelayCommand CreateSectionCommand { get; }

        private async Task OnCreateSelectionComandExecute()
        {
            FamilyInstance familyInstance = _selectionService.PickFamilyInstance();
            if (familyInstance == null)
            {
                return;
            }

            bool isCreated = await _revitTask.Run<bool>(AppearanceAssetElement =>
            _sectionService.CreateSection(
                familyInstance,
                WidthOffsetMm,
                DepthOffsetMm,
                HeightOffsetMm,
                SectionName,
                SectionName2,
                SectionName3
                ));
            if (!isCreated)
            {
                TaskDialog.Show("Ошибка", "Что-то пошло не так");
            }
            else 
            {
                TaskDialog.Show("Успех", "Все пошло так");
            }
        }
    }
}