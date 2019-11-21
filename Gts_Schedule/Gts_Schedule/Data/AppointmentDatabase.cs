using Gts_Schedule.AppointmentEditor.Model;
using Gts_Schedule.Dependency;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gts_Schedule.Data
{
    public class AppointmentDatabase
    {
        readonly SQLiteAsyncConnection database;//SQLiteConnection database;
        SQLiteCommand sqlite_cmd;
        //sqlite_cmd.CommandText = "INSERT INTO chang9 (Seq, Field, Desc, Len, Dec, Typ, Percnt, Pop, Alzero, MaxLen) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
        //sqlite_cmd.CommandText = " DROP Table 'chang9'";
        //sqlite_cmd.ExecuteNonQuery();
        public AppointmentDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Todo>().Wait();
     
        }

        public async Task ClearDatabase(string dbPath)
        {
            //database.DropTableAsync<Todo>().Wait();
            using (var connection = new SQLiteConnection(dbPath, true))
                await Task.Run(() => connection.DropTable<Todo>()).ConfigureAwait(false);
        }

        public Task<List<Todo>> GetItemsAsync()
        {
            return database.Table<Todo>().ToListAsync();
        }


        public Task<List<Todo>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Todo>("SELECT * FROM [Todo] WHERE [Done] = 'false'");
        }

        public Task<Todo> GetItemAsync(int id)
        {
            return database.Table<Todo>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Todo item)
        {
            if (item.id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Todo item)
        {
            return database.DeleteAsync(item);
        }
    }

}
