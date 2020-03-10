using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLite;

namespace GemeindeJubiläen.Models
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            //Establishing the conection
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Item>().Wait();
        }

        // Show the registers
        public Task<List<Item>> GetPeopleAsync()
        {
            return _database.Table<Item>().ToListAsync();
        }

        // Save registers
        public Task<int> SavePersonAsync(Item contact)
        {
            return _database.InsertAsync(contact);
        }

        // Delete registers
        public Task<int> DeletePersonAsync(Item contact)
        {
            return _database.DeleteAsync(contact);
        }

        // Save registers
        public Task<int> UpdatePersonAsync(Item contact)
        {
            return _database.UpdateAsync(contact);
        }


    }
}
