using LeaveApplication.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LeaveApplication.Context
{
    public class LeaveAppDbContext : DbContext
    {
        public DbSet<Reason> Reasons { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LType> LTypes { get; set; }

        public DbSet<Leave> Leaves { get; set; }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Auditable && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = HttpContext.Current != null && HttpContext.Current.User != null
                                    ? HttpContext.Current.User.Identity.Name
                                    : "Anonymous";

            foreach (var entity in entities)
            {
                DateTime Now = DateTime.Now;

                if (entity.State == EntityState.Added)
                {
                    ((Auditable)entity.Entity).DateCreated = Now;
                    ((Auditable)entity.Entity).UserCreated = currentUsername;
                }

                ((Auditable)entity.Entity).DateModified = Now;
                ((Auditable)entity.Entity).UserModified = currentUsername;
            }

            return base.SaveChanges();
        }

        //public System.Data.Entity.DbSet<LeaveApplication.Models.Role> RoleViewModels { get; set; }

        //public System.Data.Entity.DbSet<LeaveApplication.Models.User> Users { get; set; }
    }
}