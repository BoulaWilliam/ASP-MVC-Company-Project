using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using System;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext dbContext;

        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set; }

        public UnitOfWork(CompanyDbContext dbContext)// Ask For Obj From dbcontext
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            this.dbContext = dbContext;
        }

        public async Task<int> Complete()
        => await dbContext.SaveChangesAsync();

        public void Dispose()
        => dbContext.Dispose();
    }
}
