using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App4
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Data>().Wait();
        }

        //Insert or Update record
        public async Task<Data> SaveItemAsync(Data electricity)
        {
            if (electricity.EMPID != 0)
            {
                await db.UpdateAsync(electricity);
            }
            else
            {
                await db.InsertAsync(electricity);
            }

            // Retrieve the saved Data object with empNo populated
            Data savedData = await db.Table<Data>().Where(d => d.EMPID == electricity.EMPID).FirstOrDefaultAsync();

            return savedData;
        }

        public async Task<Data> UpdateItemAsync(Data updatedData, int empNo)
        {
            // Retrieve the existing record based on empNo
            Data existingData = await ReadItemAsync(empNo);

            if (existingData != null)
            {
                 
        // Update the existing record with the new data
                existingData.EMPNAME = updatedData.EMPNAME;
                existingData.HOURSWORK = updatedData.HOURSWORK;
                existingData.GROSSINCOME = updatedData.GROSSINCOME;
                existingData.BONUS = updatedData.BONUS;
                existingData.DEDUCTION = updatedData.DEDUCTION;
                existingData.NETINCOME = updatedData.NETINCOME;
                

                // Update the record in the database
                await db.UpdateAsync(existingData);

                return existingData; // Return the updated record
            }
            else
            {
                // Handle the case when the record is not found
                return null;
            }
        }




        //Delete Record
        public Task<int> DeleteItemAsync(int empNo)
        {
            return db.DeleteAsync<Data>(empNo);
        }

        //Read all Item
        public Task<List<Data>> ReadAllItemAsync()
        {
            return db.Table<Data>().ToListAsync();
        }

        //Read Item
        public Task<Data> ReadItemAsync(int meterNo)
        {
            return db.Table<Data>().Where(i => i.EMPID == meterNo).FirstOrDefaultAsync();
        }

        // GETTING THE LATEST DATA
        public async Task<Data> ReadLatestItemAsync()
        {
            var data = await db.Table<Data>()
                .OrderByDescending(i => i.EMPID)
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
