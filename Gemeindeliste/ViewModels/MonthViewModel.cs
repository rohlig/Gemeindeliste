using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Gemeindeliste.Models;
using Gemeindeliste.Views;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gemeindeliste.ViewModels
{
    public class MonthViewModel : BaseViewModel
    {
        public Item newItem { get; set; }
        public ObservableCollection<Item> Geburtstagskinder { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> Taufkinder { get; set; } = new ObservableCollection<Item>();
        public Command LoadItemsCommand { get; set; }
        public Monate Month = Monate.Januar;
        public MonthViewModel()
        {
            Title = "Browse";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            newItem = new Item();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Taufkinder.Clear();
                Geburtstagskinder.Clear();
                var items = await DataStore.GetItemsAsync(true);
                var tempList = new List<Item>();
                foreach (var item in items)
                {
                    tempList.Add(item);
                }
                var gebJub = tempList.FindAll(x => x.Geburtsdatum.Length == 10 && x.Geb.Month == (int)Month+1).OrderBy(x => x.Geb.Day);
                var taufJub = tempList.FindAll(x => x.Taufdatum.Length == 10 && x.Tauf.Month == (int)Month+1).OrderBy(x => x.Tauf.Day);
                foreach(var v in gebJub)
                {
                    Geburtstagskinder.Add(v);
                }
                foreach(var v in taufJub)
                {
                    Taufkinder.Add(v);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}