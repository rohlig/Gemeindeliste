using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using GemeindeJubiläen.Models;
using GemeindeJubilaen.Services;
using GemeindeJubiläen.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GemeindeJubiläen.Views
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
                b.FontSize = 15;
                MonatsGrid.Children.Add(b, i % 3, i / 3);
                buttonList.Add(b);
            }
            for (int i = 0; i < Enum.GetValues(typeof(Status)).Length; i++)
            {
                Button b = new Button();
                b.Text = ((Status)(i)).ToString();
                b.Clicked += Button_Clicked;
                b.BackgroundColor = Color.LightGreen;
                b.FontSize = 15;
                StatusGrid.Children.Add(b, i % 3, i / 3);
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
            if(monate.Count == 0)
            {
                await this.DisplayAlert("Keinen Monat ausgewählt", "Für den Export muss mindestens ein Monat ausgewählt sein", "OK");
                return;
            }else if(status.Count == 0)
            {
                await this.DisplayAlert("Keinen Status ausgeählt", "Für den Export muss mindestens ein Status ausgewählt sein", "OK");
                return;
            }
            foreach (var item in await DataStore.GetItemsAsync())
            {
                if(status.Contains(item.realStatus))
                items.Add(item);
            }

            FileInfo file = new PDFExport().ExportAsPDF(items, monate, exportGeb, exportTauf);

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(file.FullName)
            });
        }
    }
}