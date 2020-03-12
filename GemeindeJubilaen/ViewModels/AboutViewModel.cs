using System;

namespace GemeindeJubiläen.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public String Pfad { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public AboutViewModel()
        {
            Title = "About";
        }
    }
}