using System;
using BonusStore.DataAccess.Data;
using BonusStore.DataAccess.Repository.IRepository;
using BonusStore.Model;

namespace BonusStore.DataAccess.Repository
{
    public class LoyaltyRepository : Repository<Loyalty>, ILoyaltyRepository
    {
        private ApplicationDbContext _db;
        public LoyaltyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Loyalty obj)
        {
            _db.Loyalties.Update(obj);
        }
    }
}

