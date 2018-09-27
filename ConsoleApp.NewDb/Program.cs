using System;
using System.Collections.Generic;

namespace ConsoleApp.NewDb
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EmployeeContext())
            {
                var emp1 = new Employee("New Emp", Desgination.Engineer)
                {
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            DepartmentName = "HR"
                        },
                        new Department
                        {
                            DepartmentName = "Management"
                        }
                    }
                };
                db.Employees.Add(emp1);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
                Console.WriteLine();
                Console.WriteLine("All employees in database:");
                foreach (var employee in db.Employees)
                {
                    Console.WriteLine("Employee Id - {0}, Employee Name - {1}, No. of Depts works in - {2}", employee.EmpId, employee.Name, employee.DepartmentCount);

                    foreach (var dept in employee.Departments)
                    {
                        Console.WriteLine("Employee Id - {0} belongs to this Department - {1}", employee.EmpId,
                            dept.DepartmentName);
                    }
                }
                
            }
        }
    }
}