using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;//object linked embeded DB
using Billingg.Database;
using Billingg.Repository;

namespace Billingg.Seed
{
    class Program
    {
        static BillingContext context = new BillingContext();
        static string sourceRoot = @"C:\Billingg\Billing.xls";
        static void Main(string[] args)
        {
            getTowns();
            getAgents();

            Console.ReadKey();
        }

        static void getTowns()
        {
            Console.Write("Towns: ");
            //otvaramo repo koji se odnosi na taj entitet
            IBillingRepository<Town> towns = new BillingRepository<Town>(context);
            //dohvacamo dataset iz excela, sheeta Towns
            DataTable rawData = Help.OpenExcel(sourceRoot, "Towns");//<- treba nam ova metoda
            int N = 0;//broj ucitanih slogova
            foreach (DataRow row in rawData.Rows)//za svaki row u toj tabeli "oledb", kreiramo novi slog Town
            {
                Town town = new Town()
                {
                    Zip = Help.getString(row, 0),
                    Name = Help.getString(row, 1),
                    Region = (Region)Help.getInteger(row, 2)
                };
                N++;
                towns.Insert(town);
            }
            towns.Commit();
            Console.WriteLine(N);
        }

        static void getAgents()
        {
            Console.Write("Agents: ");
            //ova dva repo ce biti uvezani
            IBillingRepository<Agent> agents = new BillingRepository<Agent>(context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(context);

            DataTable rawData = Help.OpenExcel(sourceRoot, "Agents");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                Agent agent = new Agent()
                {
                    Name = Help.getString(row, 1)
                };
                N++;
                string[] Zone = Help.getString(row, 2).Split(',');
                foreach (string Z in Zone)
                {
                    Region R = (Region)Convert.ToInt32(Z);
                    var area = towns.Get().Where(x => x.Region == R).ToList();
                    foreach (var city in area)
                    {
                        agent.Towns.Add(city);
                    }
                }
                agents.Insert(agent);
            }
            agents.Commit();
            Console.WriteLine(N);
        }

    }
}
