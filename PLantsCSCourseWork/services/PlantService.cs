using Gardener.Database;
using Gardener.dto.request;
using Gardener.dto.response;
using Gardener.entities;
using Microsoft.EntityFrameworkCore;

namespace Gardener.Services;

public class PlantService(GardenerDbContext db) : IPlantService
{
    public async Task<List<PlantListResponse>> GetAllPlantsAsync(string? sortBy)
    {
        var query = from plant in db.Plants
                    join growth in db.Growths on plant.Id equals growth.Id
                    select new PlantListResponse(
                        plant.Id,
                        plant.Name ?? string.Empty,
                        plant.ScienceName,
                        growth.Climate,
                        growth.Space,
                        growth.Soil,
                        growth.Water,
                        growth.LandScape
                    );

        var list = await query.ToListAsync();

        sortBy = sortBy?.ToLower() ?? "none";
        list = sortBy switch
        {
            "name_asc" => list.OrderBy(p => p.Name).ToList(),
            "name_desc" => list.OrderByDescending(p => p.Name).ToList(),
            "space_asc" => list.OrderBy(p => p.Space).ToList(),
            "space_desc" => list.OrderByDescending(p => p.Space).ToList(),
            "climate_asc" => list.OrderBy(p => p.Climate).ToList(),
            "climate_desc" => list.OrderByDescending(p => p.Climate).ToList(),
            _ => list
        };
        return list;
    }

    public async Task<PlantDetailsResponse?> GetPlantDetailsAsync(int id)
    {
        var plant = await db.Plants.FindAsync(id);
        if (plant == null) return null;

        var rating = await db.PlantRatings.FindAsync(id);
        if (rating != null)
        {
            rating.ViewCount++;
            await db.SaveChangesAsync();
        }

        var growth = await db.Growths.FindAsync(id);
        var bio = await db.BioChars.FindAsync(id);

        if (growth == null || bio == null) return null;

        return new PlantDetailsResponse(
            plant.Id,
            plant.Name ?? string.Empty,
            plant.ScienceName,
            plant.Description,
            bio.LeafType, bio.Root, bio.Fruit, bio.AmmFruit?.ToString(), bio.Morphology,
            growth.Ppfd, growth.Humidity, growth.Ph, growth.Space, growth.Soil,
            growth.Survivability, growth.GrowthSpeed, growth.Climate, growth.Water, growth.LandScape
        );
    }

    public async Task<Plant> AddPlantAsync(CreatePlantRequest request)
    {
        var plant = new Plant
        {
            Name = request.Name,
            ScienceName = request.ScienceName,
            Description = request.Description
        };
        db.Plants.Add(plant);
        await db.SaveChangesAsync();

        var rating = new PlantRating 
        { 
            Id = plant.Id, 
            Plant = plant, 
            ViewCount = 0, 
            FavoriteCount = 0 
        };

        var growth = new Growth
        {
            Id = plant.Id,
            Plant = plant,
            Ppfd = request.Ppfd,
            Humidity = request.Humidity,
            Ph = request.Ph,
            Space = request.Space,
            Soil = request.Soil,
            Survivability = request.Survivability,
            GrowthSpeed = request.GrowthSpeed,
            Climate = request.Climate,
            Water = request.Water,
            LandScape = request.LandScape
        };
        var bio = new BioChar
        {
            Id = plant.Id,
            Plant = plant,
            LeafType = request.LeafType,
            Root = request.Root,
            Fruit = request.Fruit,
            AmmFruit = request.AmmFruit?.FirstOrDefault(),
            Morphology = request.Morphology
        };
        db.PlantRatings.Add(rating);
        db.Growths.Add(growth);
        db.BioChars.Add(bio);
        await db.SaveChangesAsync();
        return plant;
    }

    public async Task<bool> UpdatePlantAsync(int id, UpdatePlantRequest request)
    {
        var plant = await db.Plants.FindAsync(id);
        if (plant == null) return false;

        plant.Name = request.Name;
        plant.ScienceName = request.ScienceName;
        plant.Description = request.Description;

        var growth = await db.Growths.FindAsync(id);
        var bio = await db.BioChars.FindAsync(id);
        if (growth == null || bio == null) return false;

        growth.Ppfd = request.Ppfd;
        growth.Humidity = request.Humidity;
        growth.Ph = request.Ph;
        growth.Space = request.Space;
        growth.Soil = request.Soil;
        growth.Survivability = request.Survivability;
        growth.GrowthSpeed = request.GrowthSpeed;
        growth.Climate = request.Climate;
        growth.Water = request.Water;
        growth.LandScape = request.LandScape;

        bio.LeafType = request.LeafType;
        bio.Root = request.Root;
        bio.Fruit = request.Fruit;
        bio.AmmFruit = request.AmmFruit?.FirstOrDefault();
        bio.Morphology = request.Morphology;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePlantAsync(int id)
    {
        var plant = await db.Plants.FindAsync(id);
        if (plant == null) return false;

        var growth = await db.Growths.FindAsync(id);
        var bio = await db.BioChars.FindAsync(id);
        var favorites = db.Favorites.Where(f => f.PlantId == id);
        var rating = await db.PlantRatings.FindAsync(id);

        db.Favorites.RemoveRange(favorites);
        if (growth != null) db.Growths.Remove(growth);
        if (bio != null) db.BioChars.Remove(bio);
        if (rating != null) db.PlantRatings.Remove(rating);
        db.Plants.Remove(plant);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<string>> GetTop3PopularPlantNamesAsync()
    {
        var topPlants = await db.PlantRatings
            .OrderByDescending(r => r.FavoriteCount)
            .OrderByDescending(r => r.ViewCount)
            .ThenBy(r => r.Id)
            .Take(3)
            .Select(r => r.Plant.Name ?? r.Plant.ScienceName)
            .ToListAsync();
        return topPlants;
    }
}