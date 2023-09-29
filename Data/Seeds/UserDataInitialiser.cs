using HelmoBilite.Models;
using Bogus;
using Bogus.DataSets;
using HelmoBilite.Controllers;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Data.Seeds;

public static class UserDataInitialiser
{
    public static async Task AddDefaultUser(UserManager<BasicUser> userManager, ApplicationDbContext context)
    {
        if (userManager.Users.Count() != 0)
            return;
        
        for (int i = 0; i < 5; i++)
        {
            var person = new Person("nl_BE");
            var lastName = person.LastName;
            var firstName = person.FirstName;
            var email = $"{firstName}.{lastName}@helmo.be";
            var mat = Randomizer.Seed.Next(100000, 1000000).ToString();
            var driverLicense = Randomizer.Seed.Next(0, 3);
            var driver = new Driver()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = email,
                Email = email,
                Mat = "D" + mat,
                DriverLicense = (DriverLicence)driverLicense
            };
            var result = userManager.CreateAsync(driver, "P@ssw0rd!Driver").Result;
            if (result.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(driver, "Driver").Result;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            var person = new Person("fr");
            var lastName = person.LastName;
            var firstName = person.FirstName;
            var email = $"{firstName}.{lastName}@helmo.be";
            var mat = Randomizer.Seed.Next(100000, 1000000).ToString();
            var diploma = Randomizer.Seed.Next(0, 3);
            var dispa = new Dispa()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                Mat = "D" + mat,
                Diploma = (Diploma)diploma
            };
            
            var result = userManager.CreateAsync(dispa, "P@ssw0rd!Dispa").Result;
            if (result.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(dispa, "Dispatcher").Result;
            }
        }
        

        
        for (int i = 0; i < 5; i++)
        {
            var person = new Person("fr_CA");
            var content = new Commerce();
            
            var lastName = person.LastName;
            var firstName = person.FirstName;
            var companyName = new Company().CompanyName();
            var email = person.Email;
            Console.Error.WriteLine(email);
            var companyAddress = person.Address.Street + ", " + person.Address.City + ", " + person.Address.ZipCode + " " +person.Address.State ;
            var client = new Client()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = email,
                Email = email,
                CompanyName = companyName,
                CompanyAddress = companyAddress
            };
            
            var result = userManager.CreateAsync(client, "P@ssw0rd!Client").Result;
            if (result.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(client, "Client").Result;

                if (result2.Succeeded)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        var delivery = new Delivery()
                        {
                            Client = client,
                            Truck = null,
                            Driver = null,
                            Weight = Randomizer.Seed.Next(1, 10000),
                            WantedStartDate = DateTime.Now.AddDays(Randomizer.Seed.Next(1, 5)), 
                            WantedEndDate = DateTime.Now.AddDays(Randomizer.Seed.Next(6, 25)),
                            PickUpAddress = person.Address.Street + ", " + person.Address.City + ", " + person.Address.ZipCode + " " +person.Address.State,
                            DeliveryAddress = person.Address.Street + ", " + person.Address.City + ", " + person.Address.ZipCode + " " +person.Address.State,
                            Content = content.ProductName(),
                            Status = Status.Unassigned,
                            Comment = null,
                            CancelReason = null
                        };

                        context.Add(delivery);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        if (userManager.FindByEmailAsync("john.wick@helmo.be").Result == null)
        {
            var admin = new Dispa()
            {
                FirstName = "John",
                LastName = "Wick",
                Email = "john.wick@helmo.be",
                UserName = "john.wick@helmo.be",
                Mat = "D00001",
                Diploma = Diploma.Master
            };
            var result1 = userManager.CreateAsync(admin, "P@ssw0rd!Admin").Result;
            if (result1.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(admin, "Admin").Result;
            }  
        }
    }
}