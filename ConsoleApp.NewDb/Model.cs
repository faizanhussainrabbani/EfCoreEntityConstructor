using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ConsoleApp.NewDb
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .Property(e => e.Designation)
                .HasConversion(
                    v => v.ToString(),
                    v => (Desgination)Enum.Parse(typeof(Desgination), v));
        }
    }

    public class Employee
    {
        #region Simple Constructor
        public Employee(string name, Desgination designation)
        {
            Name = name;
            Designation = designation;
        }
        #endregion

        #region Simple Model properties
        [Key]
        public int EmpId { get; set; }
        public string Name { get; set; }

        public Desgination Designation { get; set; }
        
        #endregion

        #region Constructor with DbContext & Lazy loading
        private EmployeeContext Context { get; set; }
        private ILazyLoader LazyLoader { get; set; }
        private Employee(EmployeeContext context, ILazyLoader lazyLoader)
        {
            Context = context;
            LazyLoader = lazyLoader;
        }
        #endregion

        #region DbContext utilization
        public int DepartmentCount
            => Departments?.Count
               ?? Context?.Set<Department>().Count(d => d.Id == EF.Property<int?>(d, "EmployeeEmpId"))
               ?? 0;
        #endregion

        #region Lazy loading utilization
        private ICollection<Department> _departments;
        public ICollection<Department> Departments
        {
            get => LazyLoader.Load(this, ref _departments);
            set => _departments = value;
        }
        #endregion
    }

    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public Employee Employee { get; set; }
    }

    public enum Desgination
    {
        CEO,
        Manager,
        Superviser,
        Engineer,
        Intern
    }
}