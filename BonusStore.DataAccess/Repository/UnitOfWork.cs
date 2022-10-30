using System;
using BonusStore.DataAccess.Data;
using BonusStore.DataAccess.Repository.IRepository;

namespace BonusStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ILoyaltyRepository Loyalty { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

