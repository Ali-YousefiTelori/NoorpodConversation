using NoorpodConversation.DataBase.Configurations;
using NoorpodConversation.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoorpodConversation.DataBase.Context
{
    public class NoorpodContext : DbContext
    {
        static NoorpodContext()
        {
            string current = AppDomain.CurrentDomain.BaseDirectory;
            System.AppDomain.CurrentDomain.SetData("DataDirectory", current);
            
        }

        public NoorpodContext() : base("Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\NoorpodsDB.mdf;User Instance=True;Integrated Security=True;MultipleActiveResultSets=True")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<NoorpodContext>());
            Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<NoorpodContext, NoorpodConfiguration>());
        }

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<NotPermissionInfo> NotPermissions { get; set; }
        public DbSet<RoomMessage> RoomMessages { get; set; }
        public DbSet<RoomSubject> RoomSubjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
