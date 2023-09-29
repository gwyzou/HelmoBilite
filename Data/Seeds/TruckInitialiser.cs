using HelmoBilite.Models;
using SQLitePCL;

namespace HelmoBilite.Data.Seeds;

public class TruckInitialiser
{
    public static async Task AddDefaultTruck(ApplicationDbContext context)
    {

        var eurocargo = new Truck
        {
            Brand = TruckBrand.Iveco,
            Model = "Eurocargo",
            Plate = "1-ABC-123",
            Type = DriverLicence.C,
            MaxWeight = 10000f,
            Picture = "iveco_eurocargo.png"

        };

        var stralis = new Truck
        {
            Brand = TruckBrand.Iveco,
            Model = "Stralis",
            Plate = "1-ABC-124",
            Type = DriverLicence.Ce,
            MaxWeight = 30000f,
            Picture = "Iveco_stralis.png"

        };

        var tgx_one = new Truck
        {
            Brand = TruckBrand.Mann,
            Model = "TGX",
            Plate = "1-DEF-125",
            Type = DriverLicence.Ce,
            MaxWeight = 26000f,
            Picture = "mann_tgx.png"

        };

        var tgx_two = new Truck
        {
            Brand = TruckBrand.Mann,
            Model = "TGX",
            Plate = "1-DEF-126",
            Type = DriverLicence.Ce,
            MaxWeight = 26000f,
            Picture = "mann_tgx.png"

        };

        var actros = new Truck
        {
            Brand = TruckBrand.Mercedes,
            Model = "Actros F",
            Plate = "1-GHI-127",
            Type = DriverLicence.C,
            MaxWeight = 67000f,
            Picture = "merce_actrosF.png"

        };

        var actros_two = new Truck
        {
            Brand = TruckBrand.Mercedes,
            Model = "Actros L",
            Plate = "1-GHI-128",
            Type = DriverLicence.C,
            MaxWeight = 70000f,
            Picture = "mercedes_actrosL.png"

        };

        var truck_t = new Truck
        {
            Brand = TruckBrand.Renault,
            Model = "Truck T",
            Plate = "1-JKL-129",
            Type = DriverLicence.Ce,
            MaxWeight = 32000f,
            Picture = "renault_truckT.png"

        };

        var truck_t_two = new Truck
        {
            Brand = TruckBrand.Volvo,
            Model = "Truck T",
            Plate = "1-JKL-130",
            Type = DriverLicence.C,
            MaxWeight = 32000f,
            Picture = "volvo_truckT.png"

        };

        var fh = new Truck
        {
            Brand = TruckBrand.Volvo,
            Model = "FH",
            Plate = "1-MNO-131",
            Type = DriverLicence.Ce,
            MaxWeight = 44000f,
            Picture = "volvo_fh.png"

        };

        var fl = new Truck
        {
            Brand = TruckBrand.Volvo,
            Model = "FL",
            Plate = "1-MNO-132",
            Type = DriverLicence.C,
            MaxWeight = 12000f,
            Picture = "volvo_fl.png"
        };

        if (!context.Trucks.Any())
        {
            await context.Trucks.AddRangeAsync(eurocargo, stralis, tgx_one, tgx_two, actros, actros_two, truck_t,
                truck_t_two, fh, fl);
            await context.SaveChangesAsync();
        }
    }
}