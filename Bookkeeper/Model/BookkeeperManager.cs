using System;
using System.Collections.Generic;
using SQLite;

namespace Bookkeeper.Model
{
    public class BookkeeperManager
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string fullPath = path + "/database.db";

        /*public List<Entry> Entries { get; set; }
        public List<Account> ExpenseAccounts { get; set; }
        public List<Account> IncomeAccounts { get; set; }
        public List<Account> MoneyAccounts { get; set; }
        public List<TaxRate> TaxRates { get; set; }*/

        private static BookkeeperManager instance;

        public static BookkeeperManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookkeeperManager();
                }
                return instance;
            }
        }

        public BookkeeperManager()
        {
            using (var db = new SQLiteConnection(fullPath))
            {
                db.CreateTable<Entry>();
                db.CreateTable<Account>();
                db.CreateTable<TaxRate>();

                if(db.Table<Account>().Count() == 0)
                {
                    db.Insert(new Account("Computer", 585, Account.Type.Expense));
                    db.Insert(new Account("Supplies", 631, Account.Type.Expense));
                    db.Insert(new Account("Labour & Welfare", 597, Account.Type.Expense));

                    db.Insert(new Account("Rental", 400, Account.Type.Income));
                    db.Insert(new Account("Interest", 420, Account.Type.Income));
                    db.Insert(new Account("Sales", 440, Account.Type.Income));

                    db.Insert(new Account("Assets", 211, Account.Type.Money));
                    db.Insert(new Account("Founds", 224, Account.Type.Money));
                    db.Insert(new Account("Project", 245, Account.Type.Money));
                }
                if(db.Table<TaxRate>().Count() == 0)
                {
                    db.Insert(new TaxRate(0.06));
                    db.Insert(new TaxRate(0.12));
                    db.Insert(new TaxRate(0.20));
                    db.Insert(new TaxRate(0.25));
                }
                db.Close();
            }

            /*ExpenseAccounts = new List<Account> { new Account("Computer", 585, Account.Type.Expense),
                                                  new Account("Supplies", 631, Account.Type.Expense),
                                                  new Account("Labour & Welfare", 597, Account.Type.Expense) };

            IncomeAccounts = new List<Account> { new Account("Rental", 400, Account.Type.Income),
                                                 new Account("Interest", 420, Account.Type.Income),
                                                 new Account("Sales", 440, Account.Type.Income) };

            MoneyAccounts = new List<Account> { new Account("Assets", 211, Account.Type.Money),
                                                new Account("Founds", 224, Account.Type.Money),
                                                new Account("Project", 245, Account.Type.Money) };

            TaxRates = new List<TaxRate> { new TaxRate(0.06), new TaxRate(0.12), new TaxRate(0.20), new TaxRate(0.25) };

            Entries = new List<Entry>();*/
        }

        public void addEntry(Entry e)
        {
            using (var db = new SQLiteConnection(fullPath))
            {
                db.Insert(e);
                db.Close();
            }            
        }

        public List<Account> GetAccounts(Account.Type type)
        {
            List<Account> returnList = new List<Account>();
            using (var db = new SQLiteConnection(fullPath))
            {
                var list = db.Table<Account>().Where(a => a.AccountType.Equals(type));
                returnList = ConvertToList(list);
                db.Close();
            }
            return returnList;
        }

        public List<TaxRate> GetTaxRates()
        {
            List<TaxRate> returnList = new List<TaxRate>();
            using (var db = new SQLiteConnection(fullPath))
            {
                var list = db.Table<TaxRate>();
                returnList = ConvertToList(list);
                db.Close();
            }
            return returnList;
        }

        private List<T> ConvertToList<T>(IEnumerable<T> list)
        {
            List<T> returnList = new List<T>();
            foreach (var v in list)
            {
                returnList.Add(v);
            }
            return returnList;
        }

        public TaxRate GetTaxRate(int id)
        {
            TaxRate taxRate = new TaxRate();
            using (var db = new SQLiteConnection(fullPath))
            {
                taxRate = db.Get<TaxRate>(id);
                db.Close();
            }
            return taxRate;
        }
    }
}