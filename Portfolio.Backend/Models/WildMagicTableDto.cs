namespace Portfolio.Backend.Models;
public class WildMagicTableDto
{
  public string TableName { get; set; } = string.Empty;
  public string DiceType { get; set; } = string.Empty;
  public Dictionary<int, string> Effects { get; set; } = new();
}

