using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using GemeindeJubiläen.Models;
using GemeindeJubiläen.Views;
using System.Collections.Generic;
using System.Linq;

namespace GemeindeJubiläen.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Item newItem { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
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
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                var unorderdItems = new List<Item>();
                foreach (var item in items)
                {
                    unorderdItems.Add(item);
                }
                foreach(var item in unorderdItems.OrderBy(x => x.Name))
                {
                    Items.Add(item);
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