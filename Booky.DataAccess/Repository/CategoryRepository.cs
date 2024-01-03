using Booky.DataAccess.Data;
using Booky.DataAccess.Repository.IRepository;
using Booky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Booky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbBook _db;
        public CategoryRepository(ApplicationDbBook db) :base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            _db.CategoryDb.Update(obj);
        }
    }
}
