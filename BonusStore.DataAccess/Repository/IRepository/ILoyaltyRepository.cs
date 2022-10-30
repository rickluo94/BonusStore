using System;
using BonusStore.Model;

namespace BonusStore.DataAccess.Repository.IRepository
{
    public interface ILoyaltyRepository : IRepository<Loyalty>
    {
        void Update(Loyalty obj);
    }
}

