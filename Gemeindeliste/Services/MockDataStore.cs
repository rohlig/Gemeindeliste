using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gemeindeliste.Models;

namespace Gemeindeliste.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;
        readonly Database db;
        public MockDataStore()
        {
            items = new List<Item>()
            {
            };
            db = new Database(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\items.db");
            readFromDB();
        }

        private async void readFromDB()
        {
            items.Clear();
            var readItems = await db.GetPeopleAsync();
            foreach(var item in readItems)
            {
                items.Add(item);
            }
        }
        public async Task<bool> AddItemAsync(Item item)
        {
            var id = 1;
            if(items.Count > 0)
            {
                id += items.Max(x => x.Id);
            }
            item.Id = id;
            items.Add(item);
            await db.SavePersonAsync(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            await db.UpdatePersonAsync(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            await db.DeletePersonAsync(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}