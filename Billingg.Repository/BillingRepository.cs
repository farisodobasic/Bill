using Billingg.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Repository
{
    public class BillingRepository<Entity>: IBillingRepository<Entity> where Entity : class
    {
        protected BillingContext context; //treba nam kontekst s kojim radimo,
        protected DbSet<Entity> dbSet;// da bi znali s kojom domenskom klasom radimo treba nam dbSet

        public  BillingRepository(BillingContext _context)//ctor sluzi da ove 2 varijable inicijaliziramo
        {
            context = _context;
            dbSet = context.Set<Entity>();//na bazi gore inic. konteksta odredjuje koji je dbSet
            //dbSetom manipulisemo preko contexta
        }

        public IQueryable<Entity> Get()
        {
            return dbSet;
        }

        public Entity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(Entity entity)//virtual da bi je mogli override nekad ako bude potrebe za razlicitim implementacijama
        {
            dbSet.Add(entity);
        }

        public virtual void Update(Entity entity, int id)//isto kao Insert
        {
            Entity oldEntity = Get(id);
            context.Entry(oldEntity).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Entity oldEntity = Get(id);
            dbSet.Remove(oldEntity);
        }

        public bool Commit()
        {
            return (context.SaveChanges() > 0); //SaveChanges vraca broj uspjesnih transakcija
        }

        
            
    }
}
