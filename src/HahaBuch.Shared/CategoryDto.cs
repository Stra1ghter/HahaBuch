using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;
using HahaBuch.SharedContracts.DataAnnotations;

namespace HahaBuch.SharedContracts;

public class CategoryDto
{
    public Guid Id { get; set; }
    
    [LocalizedRequired]
    [LocalizedStringLength(2, 20)]
    public string Name { get; set; } = null!;
    
    [LocalizedStringLength(0, 120)]
    public string? Description { get; set; }
    
    [LocalizedRequired]
    [LocalizedStringLength(6)]
    public string RgbColorString { get; set; } = default!;
    
    [JsonIgnore]
    public Color Color
    {
        get => ColorTranslator.FromHtml($"#{RgbColorString}");
        set => RgbColorString = value.ToArgb().ToString("X6")[2..];
    }
    
    [JsonIgnore]
    public string HtmlColorString
        => $"#{RgbColorString}";
    
    [JsonIgnore]
    public bool IsDarkColor
        => Color.GetBrightness() <= 0.42f;
    
    public CategoryDto()
    {
    }

    public CategoryDto(Color color)
    {
        Id = Guid.NewGuid();
        Color = color;
    }

    public static CategoryDto GenerateWithDefaultColor(int currentCount)
    {
        if (currentCount < ModernBackgrounds.Length)
        {
            return new CategoryDto()
            {
                Color = ModernBackgrounds[currentCount],
            };
        }

        Random random = new();
        float hue = random.Next(0, 360);
        float saturation = 0.7f + (float)random.NextDouble() * 0.3f;
        float lightness = 0.5f + (float)random.NextDouble() * 0.1f; 

        return new CategoryDto()
        {
            Color = HSLToColor(hue, saturation, lightness), 
        };
    }
    
    private static readonly Color[] ModernBackgrounds = {
        Color.FromArgb(222, 244, 10),  // Greenish yellow
        Color.FromArgb(140, 140, 140),  // Gray
        Color.FromArgb(34, 211, 238),  // Cyan 400
        Color.FromArgb(74, 222, 128),  // Green 400
        Color.FromArgb(251, 191, 36),  // Amber 400
        Color.FromArgb(248, 113, 113), // Red 400
        Color.FromArgb(139, 92, 246),  // Violet 500
        Color.FromArgb(244, 114, 182), // Pink 400
    };
    
    private static Color HSLToColor(float hue, float saturation, float lightness)
    {
        float c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        float x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        float m = lightness - c / 2;
    
        float r, g, b;
    
        if (hue < 60) { r = c; g = x; b = 0; }
        else if (hue < 120) { r = x; g = c; b = 0; }
        else if (hue < 180) { r = 0; g = c; b = x; }
        else if (hue < 240) { r = 0; g = x; b = c; }
        else if (hue < 300) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }
    
        return Color.FromArgb(
            (int)((r + m) * 255),
            (int)((g + m) * 255),
            (int)((b + m) * 255)
        );
    }
}