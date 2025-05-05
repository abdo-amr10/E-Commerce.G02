
using System.Text.Json;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Presistence.Data
{
    public class DbIntializer : IDbIntializer
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<User> _userManager;

        public DbIntializer
            (
              ApplicationDbContext dbContext,
              RoleManager<IdentityRole> roleManger,
              UserManager<User> userManager
            )
        {
            _dbContext = dbContext;
            _roleManger = roleManger;
            _userManager = userManager;
        }


        public async Task IntializeAsync()
        {
            try
            {
                if(_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();

                if (!_dbContext.ProductTypes.Any())
                {
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructrure\Presistence\Data\Seeding\types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types != null)
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(types);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructrure\Presistence\Data\Seeding\brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands != null)
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(brands);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructrure\Presistence\Data\Seeding\products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products != null)
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                        await _dbContext.SaveChangesAsync();
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task IntializeIdentityAsync()
        {
            if (!_roleManger.Roles.Any())
            {
                await _roleManger.CreateAsync(new IdentityRole("Admin"));
                await _roleManger.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789",
                };

                var superAdminUser = new User
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                };

                await _userManager.CreateAsync(adminUser, "Passw0rd");
                await _userManager.CreateAsync(superAdminUser, "Passw0rd");

                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(adminUser, "SuperAdmin");
            }
        }
    }
}
