using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HTask.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SectorsContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SectorsContext>>()))
            {
                if (context.Sector.Any())
                {
                    return;   // DB has been seeded
                }

                context.Sector.AddRange(
                    new Sector { Name = "Manufacturing" },
                    new Sector { Name = "Other" },
                    new Sector { Name = "Service" }
                );
                context.SaveChanges();

                context.Sector.AddRange(
                    new Sector
                    {
                        Name = "Construction materials",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    {
                        Name = "Electronics and Optics",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector
                    {
                        Name = "Food and Beverage",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Furniture",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Machinery",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Metalworking",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Plastic and Rubber",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Printing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Textile and Clothing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },
                    new Sector 
                    { 
                        Name = "Wood",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Manufacturing")
                    },

                    new Sector 
                    { 
                        Name = "Creative industries",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Other")
                    },
                    new Sector 
                    { 
                        Name = "Energy technology",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Other")
                    },
                    new Sector 
                    { 
                        Name = "Environment",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Other")
                    },

                    new Sector 
                    { 
                        Name = "Business services",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    },
                    new Sector 
                    { 
                        Name = "Engineering",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    },
                    new Sector 
                    { 
                        Name = "Information Technology and Telecommunications",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    },
                    new Sector 
                    { 
                        Name = "Tourism",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    },
                    new Sector 
                    { 
                        Name = "Translation services",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    },
                    new Sector 
                    { 
                        Name = "Transport and Logistics",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Service")
                    }
                );
                context.SaveChanges();

                context.AddRange(
                    new Sector 
                    { 
                        Name = "Bakery & confectionery products",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Beverages",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Fish & fish products",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Meat & meat products",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Milk & dairy products",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Other",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },
                    new Sector 
                    { 
                        Name = "Sweets & snack food",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Food and Beverage")
                    },

                    new Sector 
                    { 
                        Name = "Bathroom/sauna",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Bedroom",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Children’s room",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Kitchen",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Living room",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Office",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Other (Furniture)",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Outdoor",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },
                    new Sector 
                    { 
                        Name = "Project furniture",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Furniture")
                    },

                    new Sector 
                    { 
                        Name = "Machinery components",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Machinery equipment/tools",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Manufacture of machinery",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Maritime",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Metal structures",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Other",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },
                    new Sector 
                    { 
                        Name = "Repair and maintenance service",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Machinery")
                    },

                    new Sector 
                    { 
                        Name = "Construction of metal structures",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metalworking")
                    },
                    new Sector 
                    { 
                        Name = "Houses and buildings",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metalworking")
                    },
                    new Sector 
                    { 
                        Name = "Metal products",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metalworking")
                    },
                    new Sector 
                    { 
                        Name = "Metal works",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metalworking")
                    },

                    new Sector 
                    { 
                        Name = "Packaging",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic and Rubber")
                    },
                    new Sector 
                    { 
                        Name = "Plastic goods",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic and Rubber")
                    },
                    new Sector 
                    { 
                        Name = "Plastic processing technology",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic and Rubber")
                    },
                    new Sector 
                    { 
                        Name = "Plastic profiles",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic and Rubber")
                    },

                    new Sector 
                    { 
                        Name = "Advertising",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Printing")
                    },
                    new Sector 
                    { 
                        Name = "Book/Periodicals printing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Printing")
                    },
                    new Sector 
                    { 
                        Name = "Labelling and packaging printing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Printing")
                    },

                    new Sector 
                    { 
                        Name = "Clothing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Textile and Clothing")
                    },
                    new Sector 
                    { 
                        Name = "Textile",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Textile and Clothing")
                    },

                    new Sector 
                    { 
                        Name = "Other (Wood)",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Wood")
                    },
                    new Sector 
                    { 
                        Name = "Wooden building materials",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Wood")
                    },
                    new Sector 
                    { 
                        Name = "Wooden houses",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Wood")
                    },

                    new Sector 
                    { 
                        Name = "Data processing, Web portals, E-marketing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Information Technology and Telecommunications")
                    },
                    new Sector 
                    { 
                        Name = "Programming, Consultancy",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Information Technology and Telecommunications")
                    },
                    new Sector 
                    { 
                        Name = "Software, Hardware",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Information Technology and Telecommunications")
                    },
                    new Sector 
                    { 
                        Name = "Telecommunications",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Information Technology and Telecommunications")
                    },

                    new Sector 
                    { 
                        Name = "Air",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Transport and Logistics")
                    },
                    new Sector 
                    { 
                        Name = "Rail",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Transport and Logistics")
                    },
                    new Sector 
                    { 
                        Name = "Road",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Transport and Logistics")
                    },
                    new Sector 
                    { 
                        Name = "Water",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Transport and Logistics")
                    }
                );
                context.SaveChanges();

                context.AddRange(
                    new Sector 
                    { 
                        Name = "Aluminium and steel workboats",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Maritime")
                    },
                    new Sector 
                    { 
                        Name = "Boat/Yacht building",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Maritime")
                    },
                    new Sector 
                    { 
                        Name = "Ship repair and conversion",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Maritime")
                    },

                    new Sector 
                    { 
                        Name = "CNC-machining",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metal works")
                    },
                    new Sector 
                    { 
                        Name = "Forgings, Fasteners",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metal works")
                    },
                    new Sector 
                    { 
                        Name = "Gas, Plasma, Laser cutting",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metal works")
                    },
                    new Sector 
                    { 
                        Name = "MIG, TIG, Aluminum welding",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Metal works")
                    },

                    new Sector 
                    { 
                        Name = "Blowing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic processing technology")
                    },
                    new Sector 
                    { 
                        Name = "Moulding",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic processing technology")
                    },
                    new Sector 
                    { 
                        Name = "Plastics welding and processing",
                        Parent = context.Sector.FirstOrDefault(x => x.Name == "Plastic processing technology")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
