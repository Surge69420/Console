using Data.Data;
using Data.Models;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace ConsoleTax
{
    internal class Program
    {
        static DbService _service;
        static void Line()
        {
            Console.WriteLine("-----------------------------------------------------------");
        }
        static void ClearConsole()
        {
            Console.Clear();
            Line();
        }
        static string CheckInput(bool Line)
        {
            if (Line)
            {
                return Console.ReadLine();
            }
            else
            {
                var key = Console.ReadKey(true).KeyChar.ToString();
                if (key == "q")
                {
                    return "exit";
                }
                else
                {
                    return key;
                }
            }
        }
        static void ListOrInsert()
        {
            ClearConsole();
            Console.WriteLine($"1) Insert Customer \n2) List Customers \nQ) exit");
            int num;
            try
            {
                var key = CheckInput(false);
                if (key == "exit")
                {
                    return;
                }
                num = Convert.ToInt32(key);
                Line();
                switch (num)
                {
                    case 1:
                        ClearConsole();
                        Console.WriteLine("Type Exit to Cancel\n");
                        Console.WriteLine("Qual o nome?");
                        string name = CheckInput(true);
                        if (name == "exit") { ListOrInsert(); return; }
                        Line();
                        Console.WriteLine("Qual a morada?");
                        string address = CheckInput(true);
                        if (address == "exit") { ListOrInsert(); return; }
                        Line();
                        Console.WriteLine("Qual o contribuinte?");
                        string taxpayer = CheckInput(true);
                        if (taxpayer == "exit") { ListOrInsert(); return; }
                        Line();
                        Console.WriteLine("Qual o código postal?");
                        string pcode = CheckInput(true);
                        if (pcode == "exit") { ListOrInsert(); return; }
                        var r = _service.CreateTable(name, address, Convert.ToInt32(taxpayer), Convert.ToInt32(pcode));
                        Console.WriteLine("\n"+r);
                        if(r == "success") { System.Threading.Thread.Sleep(500);  } else { System.Threading.Thread.Sleep(3000);  }
                        break;
                    case 2:
                        queryPayers();
                        break;
                    default:
                        Console.WriteLine(Environment.NewLine + "incorrect");
                        break;
                }
                ClearConsole();
                ListOrInsert();
            }
            catch (Exception e)
            {
                ClearConsole();
                Console.WriteLine($"\nEnter a Number \n" + e.Message);
                ListOrInsert();
            }
        }

        static void queryPayers()
        {
            ClearConsole();
            Console.WriteLine("Press Q to Exit\n");
            var payers = _service.queryDatabase();
            for (int i = 0; i < payers.Length; i++)
            {
                var payer = payers[i];
                Console.WriteLine($"{i + 1} | {payer.Name} | {payer.PostalCode}");
            }
            var key = CheckInput(false);
            if (key == "exit") { ListOrInsert(); return; }
            var EditVar = Convert.ToInt32(key) - 1;
            if (EditVar <= payers.Length)
            {
                UpdateTable(payers[EditVar]);
            }

        }
        static void UpdateTable(TaxPayer payer)
        {
            ClearConsole();
            Console.WriteLine("1 | Alterar nome\n2 | Alterar morda\n3 | Alterar codigo Postal\n4 | Apagar\nQ | Exit");
            var key = CheckInput(false);
            if (key == "exit") { queryPayers(); return; }
            int ReadLine = Convert.ToInt32(key);
            switch (ReadLine)
            {
                case 1:
                    Console.WriteLine("\nQual o nome?");
                    string name = CheckInput(true);
                    if (name == "exit") { UpdateTable(payer); return; }
                    _service.UpdateTable(payer, "Name", name);
                    break;
                case 2:
                    Console.WriteLine("\nQual a morada?");
                    string address = CheckInput(true);
                    if (address == "exit") { UpdateTable(payer); return; }
                    _service.UpdateTable(payer, "Address", address);
                    break;
                case 3:
                    Console.WriteLine("\nQual o código postal?");
                    string postalCode = CheckInput(true);
                    if (postalCode == "exit") { UpdateTable(payer); return; }
                    _service.UpdateTable(payer, "PostalCode", postalCode.ToString());
                    break;
                case 4:
                    _service.DeleteTable(payer);
                    Console.WriteLine(Environment.NewLine + "Excluído");
                    break;
                default:
                    Console.WriteLine("Incorreto");
                    break;
            }
            _service.SaveChanges();

        }
        static void Main(string[] args)
        {

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddScoped<serverDbContext>();
            builder.Services.AddScoped<DbService>();
            var ServiceProvider = builder.Services.BuildServiceProvider();
            _service = ServiceProvider.GetRequiredService<DbService>();
            ListOrInsert();

        }
    }
}
