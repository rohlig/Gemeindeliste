using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using GemeindeJubiläen.Models;
using GemeindeJubiläen.ViewModels;

namespace GemeindeJubiläen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Hinzufügen_Clicked(object sender, EventArgs e)
        {
            NewName.Text = NewName.Text.Trim();
            if (NewName.Text == "")
            {
                await this.DisplayAlert("Name fehlt", "Bitte einen Namen Eintragen", "OK");
                return;
            }
            else if (viewModel.newItem.Id == -1 && viewModel.Items.FirstOrDefault(x => x.Name.Equals(NewName.Text)) != null)
            {
                await this.DisplayAlert("Name Existiert bereits", "Der Gewünschte Name existiert bereits!!!", "OK");
                return;
            }
            try
            {
                if (NewGeb.Text.Length > 0)
                {
                    var geb = DateTime.ParseExact(NewGeb.Text, "dd.MM.yyyy", null);
                }
                if (NewTauf.Text.Length > 0)
                {
                    var tauf = DateTime.ParseExact(NewTauf.Text, "dd.MM.yyyy", null);
                }
            }
            catch (Exception)
            {
                await this.DisplayAlert("Falsches Datumsformat", "Datum konnte nicht erkannt werden. \n richtiges Format: TT.MM.JJJJ", "OK");
                return;
            }
            if (viewModel.newItem.Id == -1)
            {
                await viewModel.DataStore.AddItemAsync(viewModel.newItem);
            }
            else
            {
                await viewModel.DataStore.UpdateItemAsync(viewModel.newItem);
            }
            ItemsListView.BeginRefresh();
            Zurücksetzen_Clicked(sender, e);
        }

        private void Zurücksetzen_Clicked(object sender, EventArgs e)
        {
            viewModel.newItem = new Item();
            NewGeb.Text = NewTauf.Text = NewName.Text = "";
            NewGemeindeglied.Text = viewModel.newItem.Status;
        }

        private void Bearbeiten_Clicked(object sender, EventArgs e)
        {
            var item = viewModel.newItem = (Item)(((sender as Button).BindingContext as Item).Clone());
            NewGeb.Text = item.Geburtsdatum;
            NewTauf.Text = item.Taufdatum;
            NewName.Text = item.Name;
            NewGemeindeglied.Text = item.Status;
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

        private void StatusClicked(object sender, EventArgs e)
        {
            viewModel.newItem.realStatus = (Status)(((int)viewModel.newItem.realStatus + 1) % Enum.GetValues(typeof(Status)).Length); ;
            NewGemeindeglied.Text = viewModel.newItem.Status;
        }
    }
}