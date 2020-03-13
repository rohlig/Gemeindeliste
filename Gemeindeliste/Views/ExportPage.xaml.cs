using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Gemeindeliste.Models;
using GemeindeJubilaen.Services;
using Gemeindeliste.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Gemeindeliste.Views
{
    [DesignTimeVisible(false)]
    public partial class ExportPage : ContentPage
    {
        
        private List<Button> buttonList;
        public ExportPage()
        {
            InitializeComponent();
            buttonList = new List<Button>();
            for (int i = 0; i < 12; i++)
            {
                Button b = new Button();
                b.Text = ((Monate)(i)).ToString();
                b.Clicked += Button_Clicked;
                MonatsGrid.Children.Add(b, i / 3, i % 3);
                buttonList.Add(b);
            }
            for (int i = 0; i < Enum.GetValues(typeof(Status)).Length; i++)
            {
                Button b = new Button();
                b.Text = ((Status)(i)).ToString();
                b.Clicked += Button_Clicked;
                b.BackgroundColor = Color.LightGreen;
                StatusGrid.Children.Add(b, i % 4, i / 4);
                buttonList.Add(b);
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if ((sender as Button).BackgroundColor == Color.LightGreen)
            {
                (sender as Button).BackgroundColor = Color.LightGray;
            }
            else
            {
                (sender as Button).BackgroundColor = Color.LightGreen;

            }
        }

        private void InhaltButton_Clicked(object sender, EventArgs e)
        {
            GebButton.BackgroundColor = TaufButton.BackgroundColor = BeidButton.BackgroundColor = Color.LightGray;
            (sender as Button).BackgroundColor = Color.LightGreen;
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            List<Monate> monate = new List<Monate>();
            List<Status> status = new List<Status>();
            IDataStore<Item> DataStore = DependencyService.Get<IDataStore<Item>>();
            List<Item> items = new List<Item>();
            Boolean exportGeb = (TaufButton.BackgroundColor != Color.LightGreen);
            Boolean exportTauf = (GebButton.BackgroundColor != Color.LightGreen);
            for (int i = 0; i < buttonList.Count; i++)
            {

                if (buttonList[i].BackgroundColor == Color.LightGreen)
                {
                    if (i < 12)
                    {
                        monate.Add((Monate)i);
                    }
                    else
                    {
                        status.Add((Status)(i-12));
                    }
                }
            }
            foreach (var item in await DataStore.GetItemsAsync())
            {
                if (status.Contains(item.realStatus))
                    items.Add(item);
            }
            FileInfo file = null;
            if (MonatsButton.BackgroundColor == Color.LightGreen)
            {
                if (monate.Count == 0)
                {
                    await this.DisplayAlert("Keinen Monat ausgewählt", "Für den Export muss mindestens ein Monat ausgewählt sein", "OK");
                    return;
                }
                else if (status.Count == 0)
                {
                    await this.DisplayAlert("Keinen Status ausgeählt", "Für den Export muss mindestens ein Status ausgewählt sein", "OK");
                    return;
                }
                file = new PDFExport().ExportAsPDF(items, monate, exportGeb, exportTauf);
            }
            else
            {
                file = new PDFExport().ExportAsPDF(items, (GemVorButton.BackgroundColor == Color.LightGreen));
            }
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(file.FullName)
            });
        }

        private void ExportArt_Clicked(object sender, EventArgs e)
        {
            MonatsButton.BackgroundColor = GemVorButton.BackgroundColor = GemNachButton.BackgroundColor = Color.LightGray;
            (sender as Button).BackgroundColor = Color.LightGreen;
            if ((sender as Button) == MonatsButton)
            {
                for (int i = 0; i < 12; i++)
                {
                    buttonList[i].IsVisible = true;
                }
                GebButton.IsVisible = TaufButton.IsVisible = BeidButton.IsVisible = MonatsLabel.IsVisible = InhaltsLabel.IsVisible = true;
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    buttonList[i].IsVisible = false;
                    GebButton.IsVisible = TaufButton.IsVisible = BeidButton.IsVisible = MonatsLabel.IsVisible = InhaltsLabel.IsVisible = false;

                }
            }
        }

        private void About_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new AboutPage());
        }
    }
}