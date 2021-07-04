using Hahn.ApplicatonProcess.February2021.Data.Infrastructure;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;

namespace Hahn.ApplicatonProcess.February2021.Web.Classes
{
    public class DataSeeder
    {
        public static void SeedCountries(DataContext context)
        {
            if (!context.Assets.Any())
            {
                var rand = new Random();

                DateTimeOffset randomDate()
                {
                    return DateTimeOffset.UtcNow.Date.AddDays(Math.Ceiling(rand.NextDouble() * -100));
                };

                var assets = new List<Asset>
                {
                    new Asset {
                        AssetName = "Office furniture",
                        CountryOfDepartment ="DEU",
                        Department = Domain.Enums.Business.enDepartment.HQ,
                        EMailAdressOfDepartment = "hq@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Machinery" ,
                        CountryOfDepartment ="DEU",
                        Department = Domain.Enums.Business.enDepartment.Store1,
                        EMailAdressOfDepartment = "store1@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Buildings" ,
                        CountryOfDepartment ="DEU",
                        Department = Domain.Enums.Business.enDepartment.Store2,
                        EMailAdressOfDepartment = "store2@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Lands" ,
                        CountryOfDepartment ="DEU",
                        Department = Domain.Enums.Business.enDepartment.Store3,
                        EMailAdressOfDepartment = "store3@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Equipment" ,
                        CountryOfDepartment ="PRT",
                        Department = Domain.Enums.Business.enDepartment.MaintenanceStation,
                        EMailAdressOfDepartment = "maintenancestation@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Inventory"  ,
                        CountryOfDepartment ="BRA",
                        Department = Domain.Enums.Business.enDepartment.Store1,
                        EMailAdressOfDepartment = "Store1@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Corporate stock"  ,
                        CountryOfDepartment ="USA",
                        Department = Domain.Enums.Business.enDepartment.HQ,
                        EMailAdressOfDepartment = "hq@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    },
                    new Asset {
                        AssetName = "Savings accounts"  ,
                        CountryOfDepartment ="USA",
                        Department = Domain.Enums.Business.enDepartment.HQ,
                        EMailAdressOfDepartment = "hq@hahn-softwareentwicklung.de",
                        PurchaseDate = randomDate()
                    }
                };


                var extraList = new List<Asset>();

                for (int i = 1; i < 5; i++)
                {
                    var newList = System.Text.Json.JsonSerializer.Deserialize<IList<Asset>>(System.Text.Json.JsonSerializer.Serialize(assets));
                    foreach (var item in newList)
                    {
                        item.AssetName += $" - {i}";
                    }
                    extraList.AddRange(newList);
                }
                assets.AddRange(extraList);

                context.AddRange(assets);
                context.SaveChanges();
            }
        }
    }
}
