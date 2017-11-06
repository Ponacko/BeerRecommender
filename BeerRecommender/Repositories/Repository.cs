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

        protected void Create(T obj) {
            using (var context = new AppDbContext()) {
                entities.Add(obj);
                context.SaveChanges();
            }
        }

        protected T RetrieveById(int id) {
            return entities.First(e => e.Id == id);
        }

        protected List<T> RetrieveAll() {
            return entities.ToList();
        }

        protected void Update(T obj) {
            using (var context = new AppDbContext()) {
                entities.AddOrUpdate(obj);
                context.SaveChanges();
            }
        }

        protected void Delete(int id) {
            using (var context = new AppDbContext()) {
                entities.Remove(RetrieveById(id));
                context.SaveChanges();
            }

        }

    }
}
