using Portfolio.Backend.Models;
using System.Text.Json;

namespace Portfolio.Backend.Services.Local;

public class WildMagicService
{
  private readonly Dictionary<int, string> _effects = new();
  private readonly Random _random = new Random();

  public WildMagicService(IWebHostEnvironment env, ILogger<WildMagicService> logger)
  {
    try
    {
      var filePath = Path.Combine(env.ContentRootPath, "Data", "wild_magic.json");
      var jsonString = File.ReadAllText(filePath);

      var table = JsonSerializer.Deserialize<WildMagicTableDto>(jsonString, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });

      if(table != null)
      {
        _effects = table.Effects;
        logger.LogInformation($"Loaded {table.TableName} with {_effects.Count} effects into memory.");
      }
    }

    catch (Exception ex)
    {
      logger.LogError($"Failed to load Wild Magic table: {ex.Message}");
    }
  }

  public WildMagicRoll GetWildMagicEffect()
  {
    if (_effects == null)
      return new WildMagicRoll("Error Loading Wild Magic Table", -1);

    var roll = _random.Next(21);
    return new WildMagicRoll(_effects[roll], roll);
  }
}
