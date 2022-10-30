using System;
using BonusStore.DataAccess.Data;
using BonusStore.DataAccess.Repository.IRepository;
using BonusStore.Model;

namespace BonusStore.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

