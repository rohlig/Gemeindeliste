using System;
using System.Linq;
using Xamarin.Forms;
using Gemeindeliste.Models;
using Gemeindeliste.ViewModels;
using GemeindeJubilaen.Services;
using Xamarin.Essentials;
using System.IO;

namespace Gemeindeliste.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    public enum Monate
    {
        Januar,
        Februar,
        März,
        April,
        Mai,
        Juni,
        Juli,
        August,
        September,
        Oktober,
        November,
        Dezember
    }
    public partial class MonthView : ContentPage
    {
       
        MonthViewModel viewModel;
        public MonthView()
        {
            InitializeComponent();
            BindingContext = viewModel = new MonthViewModel();
            viewModel.Month = (Monate)(DateTime.Now.Month - 1);
            Monatsname.Text = ((Monate)viewModel.Month).ToString();
            reloadInTime(2000);
        }
        private async void reloadInTime(int msec)
        {
            System.Threading.Thread.Sleep(msec);
            await viewModel.ExecuteLoadItemsCommand();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GeburtstagsListView.BeginRefresh();
            if (viewModel.Geburtstagskinder.Count == 0 && viewModel.Taufkinder.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

       
        private async void Löschen_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Löschen bestätigen", "Möchten Sie den Kontakt " + ((sender as Button).BindingContext as Item).Name + " wirklich löschen\nLöschen kann nicht rückgängig gemacht werden", "Ja", "Nein");
           // string x = await this Löschen bestätigen", "Möchten Sie den Kontakt " + ((sender as Button).BindingContext as Item).Name + " wirklich löschen","Ja","Abbrechen",null,-1,null,"");
            if (answer)
            {
                await viewModel.DataStore.DeleteItemAsync(((sender as Button).BindingContext as Item).Id);
                await viewModel.ExecuteLoadItemsCommand();
            }
        }

        private async void Previous_Clicked(object sender, EventArgs e)
        {
            viewModel.Month = (viewModel.Month - 1);
            if ((int)viewModel.Month == -1) viewModel.Month = Monate.Dezember;
            Monatsname.Text = viewModel.Month.ToString();
            await viewModel.ExecuteLoadItemsCommand();
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            viewModel.Month = (Monate)(((int)viewModel.Month + 1) % 12);
            Monatsname.Text = ((Monate)viewModel.Month).ToString();
            await viewModel.ExecuteLoadItemsCommand();
        }

        private async void Export_Clicked(object sender, EventArgs e)
        {
            FileInfo file = new PDFExport().ExportMonthAsPDF(viewModel.Geburtstagskinder.ToList(), viewModel.Taufkinder.ToList(), viewModel.Month) ;
            
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(file.FullName)
            });
        }
    }
}