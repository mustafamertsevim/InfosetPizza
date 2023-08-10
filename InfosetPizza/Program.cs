using System;
using System.Linq;
using System.Collections.Generic;

public class RestaurantBranch
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class RestaurantController
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantController(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<RestaurantBranch> GetNearestRestaurants(double customerLatitude, double customerLongitude)
    {
        const double maxDistanceKm = 10;
        const int maxResultCount = 5;

        var nearestRestaurants = _dbContext.RestaurantBranches
            .OrderBy(r => CalculateDistance(customerLatitude, customerLongitude, r.Latitude, r.Longitude))
            .Where(r => CalculateDistance(customerLatitude, customerLongitude, r.Latitude, r.Longitude) <= maxDistanceKm)
            .Take(maxResultCount)
            .ToList();

        return nearestRestaurants;
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double r = 6371; // Earth radius in km
        var latDiff = (lat2 - lat1) * (Math.PI / 180);
        var lonDiff = (lon2 - lon1) * (Math.PI / 180);
        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = r * c;

        return distance;
    }
}

public class RestaurantDbContext
{
    public List<RestaurantBranch> RestaurantBranches { get; set; }

    public RestaurantDbContext()
    {
        // Veritabanı örnek verileri burada oluşturabilirsiniz.
        RestaurantBranches = new List<RestaurantBranch>
        {
            new RestaurantBranch { Id = 1, Name = "Restaurant 1", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 2, Name = "Restaurant 2", Latitude = 39.8781, Longitude = -71.6298 },
            new RestaurantBranch { Id = 3, Name = "Restaurant 3", Latitude = 43.8781, Longitude = -77.6298 },
            new RestaurantBranch { Id = 4, Name = "Restaurant 4", Latitude = 44.8781, Longitude = -57.6298 },
            new RestaurantBranch { Id = 5, Name = "Restaurant 5", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 6, Name = "Restaurant 6", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 7, Name = "Restaurant 7", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 8, Name = "Restaurant 8", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 9, Name = "Restaurant 9", Latitude = 40.7128, Longitude = -74.0060 },
            new RestaurantBranch { Id = 10, Name = "Restaurant 10", Latitude = 40.7128, Longitude = -74.0060 },
        };
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var dbContext = new RestaurantDbContext();
        var controller = new RestaurantController(dbContext);

        var nearestRestaurants = controller.GetNearestRestaurants(40.7128, -74.0060);
        foreach (var restaurant in nearestRestaurants)
        {
            Console.WriteLine($"Name: {restaurant.Name}, Latitude: {restaurant.Latitude}, Longitude: {restaurant.Longitude}");
        }
    }
}
