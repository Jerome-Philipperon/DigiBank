using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    static class BankInitializer
    {

        public static void Initialization(this BankContext context, bool dataCreateAlways)
        {
            if(dataCreateAlways)
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            if (!context.Clients.Any())
                return;

            //Todo : Implement this

            #region Clients

            #endregion

            #region Employees

            #endregion

            #region Managers

            #endregion

            #region Deposits

            #endregion

            #region Saving

            #endregion

            #region Cards

            #endregion

            context.SaveChanges();
        }
    }
}
