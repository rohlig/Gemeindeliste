using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GemeindeJubiläen.Models;
using GemeindeJubiläen.Views;
using GemeindeJubiläen.ViewModels;

namespace GemeindeJubiläen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    [DesignTimeVisible(false)]
    public partial class MonthView : ContentPage
    {
        enum Monate
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
        MonthViewModel viewModel;
        public MonthView()
        {
            InitializeComponent();
            BindingContext = viewModel = new MonthViewModel();
            viewModel.Month = DateTime.Now.Month - 1;
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
            if (viewModel.Month == -1) viewModel.Month = 11;
            Monatsname.Text = ((Monate)viewModel.Month).ToString();
            await viewModel.ExecuteLoadItemsCommand();
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            viewModel.Month = (viewModel.Month + 1) % 12;
            Monatsname.Text = ((Monate)viewModel.Month).ToString();
            await viewModel.ExecuteLoadItemsCommand();
        }
    }
}