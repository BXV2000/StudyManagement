using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManagement.Data
{
    public class StudyManagementDbContext : IdentityDbContext<Users>
    {
        public StudyManagementDbContext(DbContextOptions<StudyManagementDbContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
    }
}
