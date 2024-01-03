﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Booky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T -Will be Category or any other Table we will perform CRUD Operation
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> Filter, string? includeProperties = null);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);  
    }
}
