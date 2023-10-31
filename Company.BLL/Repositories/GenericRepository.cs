using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyDbContext _dbContext;

        public GenericRepository(CompanyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task Add(T item)
         => await _dbContext.AddAsync(item);
        

        public void Delete(T item)
        =>  _dbContext.Remove(item);
       

        public async Task< IEnumerable<T>> GetAll()
        {
            if(typeof(T)== (typeof(Employee)))
            {
                return(IEnumerable<T>) await _dbContext.Employees.Include(E=>E.Department).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        => await _dbContext.Set<T>().FindAsync(id);

        public void Update(T item)
        =>  _dbContext.Update(item);

    }
}
