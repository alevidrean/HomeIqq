using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace HomeIQ.ViewModels
{
    public class SecondPageViewModel
    {
        public class ProgramModel
        {
            public string Name { get; set; }
            public List<IntervalModel> Intervals { get; set; }
        }

        public class IntervalModel
        {
            public string TimeRange { get; set; }
            public double Temperature { get; set; }
        }
        public ICommand NavigateCommand => new Command<string>(async (route) =>
        {
            if (!string.IsNullOrEmpty(route))
                await Shell.Current.GoToAsync($"//{route}", true);
        });

        public ObservableCollection<ProgramModel> Programs { get; } = new()
    {
        new ProgramModel { Name = "Program Week-end", Intervals = new List<IntervalModel> {
            new IntervalModel { TimeRange = "08:00-10:00", Temperature = 21 },
            new IntervalModel { TimeRange = "10:00-12:00", Temperature = 22 }
        }},
        new ProgramModel { Name = "Program Week-day", Intervals = new List<IntervalModel> {
            new IntervalModel { TimeRange = "09:00-11:00", Temperature = 20 }
        }},
        new ProgramModel { Name = "Program Concediu", Intervals = new List<IntervalModel> {
            new IntervalModel { TimeRange = "07:00-09:00", Temperature = 19 }
        }},
    };

        public ICommand ShowProgramDetailsCommand => new Command<ProgramModel>(async (program) =>
        {
            if (program != null)
            {
                string details = string.Join("\n", program.Intervals.Select(i => $"{i.TimeRange}: {i.Temperature}°C"));
                await Application.Current.MainPage.DisplayAlert(program.Name, details, "OK");
            }
        });
    }
}