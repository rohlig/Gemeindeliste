using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace GemeindeJubiläen.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public String Pfad { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public AboutViewModel()
        {
            Title = "Import/Export";
        }
    }
}