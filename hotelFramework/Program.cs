using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

//Developed with Kowatschek Samuel

var ft = new HotelContextFactory();
var ctxt = ft.CreateDbContext();

if (args[0].ToLower() == "add")
{
    Console.WriteLine("Hotels added");
    await AddData();
}
else if (args[0].ToLower() == "query")
{
    await QueryData();
}




async Task AddData()
{
}

async Task QueryData()
{
}


#region Model
enum Special
{
    Spa,
    Sauna,
    DogFriendly,
    IndoorPool,
    OutdoorPool,
    BikeRental,
    ECarChargingStation,
    VegetarianCuisine,
    OrganicFood
}

class Hotel
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Address { get; set; }

    public List<HotelSpecial> Specials { get; set; } = new();

    public List<RoomType> RoomTypes { get; set; } = new();
}

class HotelSpecial
{
    public int Id { get; set; }

    public Special Special { get; set; }

    public List<Hotel> Hotels { get; set; }
}

class RoomType
{
    public int Id { get; set; }

    public Hotel Hotel { get; set; }

    public int HotelId { get; set; }

    [MaxLength(50)]
    public string Title { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    public int Size { get; set; }

    public bool DisabilityAccessible { get; set; } = false;

    public int RoomsAvailable { get; set; }
}

class RoomPrice
{
    public int Id { get; set; }

    public RoomType RoomType { get; set; }

    public int RoomTypeId { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidUntil { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceEurPerNight { get; set; }
}
#endregion

#region DataContext
class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> options) : base(options)
    { }

    public DbSet<Hotel> Hotels { get; set; }

    public DbSet<HotelSpecial> HotelSpecials { get; set; }

    public DbSet<RoomType> RoomTypes { get; set; }

    public DbSet<RoomPrice> RoomPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>().HasIndex(h => h.Id).IsUnique();

        modelBuilder.Entity<HotelSpecial>().HasIndex(h => h.Id).IsUnique();

        modelBuilder.Entity<RoomType>().HasIndex(h => h.Id).IsUnique();

        modelBuilder.Entity<RoomPrice>().HasIndex(h => h.Id).IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}

class HotelContextFactory : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<HotelContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new HotelContext(optionsBuilder.Options);
    }
}
#endregion