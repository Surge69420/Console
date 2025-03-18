using Data.Data;
using Data.Models;

namespace Data.Services
{
    public class DbService
    {
        private readonly serverDbContext _context;
        public DbService(serverDbContext context)
        {

            _context = context;
        }

        public string CreateTable(string name, string address, int taxpayer, int postalcode)
        {
            try
            {
                if (_context.TaxPayers.Find(taxpayer) != null)
                {
                    return "Tax Payer Already Exists";
                }
                var taxp = new TaxPayer
                {
                    Name = name,
                    Address = address,
                    Id = taxpayer,
                    PostalCode = postalcode
                };
                _context.TaxPayers.Add(taxp);
                _context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed" + ex.Message;
            }
        }

        public TaxPayer[] queryDatabase()
        {
            return _context.TaxPayers.ToArray();
        }
        public string DeleteTable(TaxPayer taxpayer)
        {
            try
            {
                _context.TaxPayers.Remove(taxpayer);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed" + ex.Message;
            }
        }
        public bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateTable(TaxPayer taxpayer, string key,string value)
        {
            try
            {
                var Type = taxpayer.GetType().GetProperty(key).PropertyType;
                var val = Convert.ChangeType(value, Type);
                taxpayer.GetType().GetProperty(key).SetValue(taxpayer, val);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
