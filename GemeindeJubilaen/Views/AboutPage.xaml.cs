using GemeindeJubiläen.Models;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemeindeJubiläen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void Export_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();
                var tempView = new ViewModels.BaseViewModel();
                var items = await tempView.DataStore.GetItemsAsync();
                var list = new List<Item>();
                foreach (var i in items)
                {
                    list.Add(i);
                }
                // To write to a file, create a StreamWriter object.  
                XmlSerializer mySerializer = new XmlSerializer(typeof(List<Item>));
                StreamWriter myWriter = new StreamWriter(file.GetStream());
                mySerializer.Serialize(myWriter, list);
                myWriter.Close();
            }
            catch
            {
                await this.DisplayAlert("Etwas ist schiefgelaufen", "Datei konnte nicht geseichert werden","Ok");
            }
           /* if (file != null)
            {
                lbl.Text = file.FileName;
            }
            var savePicker = new Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";*/
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as Entry).Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}