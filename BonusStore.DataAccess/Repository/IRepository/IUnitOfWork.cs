using System;
namespace BonusStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get;}
        ILoyaltyRepository Loyalty { get; }

        void Save();
    }
}

