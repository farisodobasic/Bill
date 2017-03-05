using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class BillingContext:DbContext
    {
        public BillingContext() : base("name=Billing")
        {
            //ovaj ctor sluzi da otvori konekciju      
        }
        //dbset town je mali context jer ima u sebi 4 dbseta tj liste
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Procurement> Procurements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Town> Towns { get; set; }

        //treba da mapiramo flag deleted koji je u svim modelima i da pokazemo namjenu
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //mapiranje zahtjeva da polje deleted ima vrjednost false, u suprotnom nece se uzimati u obzir
            //ignorise se Basic klasa, iako ima Deleted flag nju ne treba tretirati
            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Agent>()
                .Map<Agent>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>(); 
            modelBuilder.Entity<Category>().Map<Category>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            //model builder entity agent je mapiran tako da zahtjeva da je Deleted false, ignorisemo polje deleted -> nije dostupno iz koda!
            //svaki entitet preko polja Deleted cemo mapirati/uvezati da ima vrijednost false!
            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Customer>()
                .Map<Customer>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Invoice>()
                .Map<Invoice>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Item>()
                .Map<Item>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Procurement>()
                .Map<Procurement>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Product>()
                .Map<Product>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Shipper>()
                .Map<Shipper>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Stock>()
                .Map<Stock>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Supplier>()
                .Map<Supplier>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);

            modelBuilder.Ignore<Basic>();
            modelBuilder.Entity<Town>()
                .Map<Town>(x => { x.Requires("Deleted").HasValue(false); })
                .Ignore(x => x.Deleted);
        }

        public override int SaveChanges()
        {

            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted))
            { 
                SoftDelete(entry);
            }
            //changeTracker -> u EFu je wathcer posmatra koji su slogovi promjene
            //SaveChange -> tracker uzme sve posljednje promjene i kreira SQL upite na osnovu tih promjena
            //odradi nesto nase(ovo gore) pa onda svoje (ovo dolje)
            return base.SaveChanges();//vrati savechanes baszne klase

        }

        private void SoftDelete(DbEntityEntry entry) //DbEntityEntry objekt domenske klase
        {
            
            Type entryEntityType = entry.Entity.GetType();//tip skalarani-polje ili navigacioni-tabela
            string tableName = GetTableName(entryEntityType);
            string primaryKeyName = GetPrimaryKeyName(entryEntityType);
            string sql =
                string.Format(
                       "UPDATE {0} SET Deleted = 1 WHERE {1} = @id",//ne preko EF vec direktno SQL; @Id od entry-ja koji smo dobili
                       tableName, primaryKeyName);
            //da bi mogli raditi s SQLom treba nam koja je tabela i koji je prim kljuc

            //izvrsavanje SQL komande
            Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));
            
            // prevent hard delete

            entry.State = EntityState.Detached;

        }
        //metaDB je DB koja opsijue nasu DB. SELECT * nesto -> ode se prvo u meta bazu i pogleda sta je * (koje tabele)
        //opisuje strukturu nase baze, kljucevi itd...

        private static Dictionary<Type, EntitySetBase> _mappingCache =
                new Dictionary<Type, EntitySetBase>();//Type - tabela tj domenska klasa (Agent, Product...) 

        //dohvatamo kompletan skup entiteta i propertija nekog seta
        private EntitySetBase GetEntitySet(Type type)
        {

            if (!_mappingCache.ContainsKey(type))
            {
                //bazni kontekst
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;//vratit ce nam BillingContext

                string typeName = ObjectContext.GetObjectType(type).Name;//vratit ime
                //iz metaDB dohvaca kolekciju svih Itema, komplektno opisan entitet
                var es = octx.MetadataWorkspace
                    .GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .SelectMany(c => c.BaseEntitySets
                            .Where(e => e.Name == typeName))
                    .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);//vracamo nazad taj objekt
            }
            return _mappingCache[type];
        }


        private string GetTableName(Type type)
        {
            //Schema -> dbo.*
            EntitySetBase es = GetEntitySet(type);//iz seta es izvlacimo metadataProp Schema i Table
            return string.Format("[{0}].[{1}]",
            es.MetadataProperties["Schema"].Value, 
            es.MetadataProperties["Table"].Value);
        }
        

        private string GetPrimaryKeyName(Type type)
        {
            //vraca KeyMembers na nultom indexu array kljuceva, primary(na prvom mjestu) i onda foreign keys
            EntitySetBase es = GetEntitySet(type);
            return es.ElementType.KeyMembers[0].Name;
        }


    }
}
