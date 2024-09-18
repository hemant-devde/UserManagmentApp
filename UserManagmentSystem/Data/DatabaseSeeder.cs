using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UserManagmentSystem.Models.Entities;


namespace UserManagmentSystem.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedData(AppDbContext context, UserManager<Employee> userManager)
        {
            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            if (!context.Departments.Any())
            {
                await SeedDepartments(context);
            }

            if (!context.Roles.Any())
            {
                await SeedRoles(context);
            }

            if (!context.Employees.Any())
            {
                await SeedAdminUser(context, userManager);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedDepartments(AppDbContext context)
        {
            var departments = new[]
            {
                new Department { DepartmentName = "Administration", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Human Resources", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Information Technology", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            await context.Departments.AddRangeAsync(departments);
        }

        private static async Task SeedRoles(AppDbContext context)
        {
            var roles = new[]
            {
                new Role { RoleName = "System Admin" },
                new Role { RoleName = "HR Manager" },
                new Role { RoleName = "Project Manager" },
                new Role { RoleName = "Team Lead" },
                new Role { RoleName = "Employee" }
            };

            await context.Roles.AddRangeAsync(roles);
        }

        private static async Task SeedAdminUser(AppDbContext context, UserManager<Employee> userManager)
        {
            var adminDepartment = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == "Administration");

            if (adminDepartment == null)
            {
                throw new Exception("Admin department not found. Ensure departments are seeded first.");
            }
            // Assign the System Admin role
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "System Admin");
            if (adminRole != null)
            {
               
                // For example: await context.UserRoles.AddAsync(new UserRole { UserId = adminUser.Id, RoleId = adminRole.RoleId });
            }
            var adminUser = new Employee
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@example.com",
               // UserName = "admin@example.com", // UserName is required for Identity
                UserType = UserType.Admin,
                MobileNumber = "9876543210",
                DepartmentId = adminDepartment.DepartmentId,
                RoleId  = adminRole.RoleId,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StreetAddress = "123 Admin St",
                City = "Indore",
                State = "Madhya Pradesh",
                PostalCode = "452001",
                Country = "India"
            };

            var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user: {errors}");
            }

           
        }
    }
}