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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbBook _db;
        public ProductRepository(ApplicationDbBook db) :base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            //
            //_db.ProductDb.Update(obj);

            var objFromDb=_db.ProductDb.FirstOrDefault(u=>u.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;                    
                objFromDb.Price = obj.Price;
                objFromDb.Price50= obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100= obj.Price100;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
