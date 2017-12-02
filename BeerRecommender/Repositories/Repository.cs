using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender.Entities;

namespace BeerRecommender.Repositories
{
    public abstract class Repository<T> where T : Entity {
        protected DbSet<T> entities;
        protected AppDbContext context;

        public int Create(T obj) {
            entities.Add(obj);
            context.SaveChanges();
            
            return obj.Id;
        }

        public T RetrieveById(int id) {
            return !entities.Any(e => e.Id == id) ? null : entities.First(e => e.Id == id);
        }

        public List<T> RetrieveAll() {
            return entities.ToList();
        }

        public void Update(T obj) {
            entities.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id) {
            entities.Remove(RetrieveById(id));
            context.SaveChanges();
        }
    }
}
