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

        public bool CreateTable(string name, string address, int taxpayer, int postalcode)
        {
            try
            {
                if (!_context.TaxPayers.Any(o => o.Id == taxpayer))
                {
                    var taxp = new TaxPayer
                    {
                        Name = name,
                        Address = address,
                        Id = taxpayer,
                        PostalCode = postalcode
                    };
                    _context.TaxPayers.Add(taxp);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<TaxPayer> queryDatabase()
        {
            return _context.TaxPayers.ToList();
        }


    }
}
