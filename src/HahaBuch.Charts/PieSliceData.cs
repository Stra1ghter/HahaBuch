namespace HahaBuch.Charts;

public class PieSliceData
{
    public string Label { get; set; } = string.Empty;

    public string Color { get; set; } = "000000";

    /// <summary>Value (original numeric amount).</summary>
    public decimal Value { get; set; }

    /// <summary>Fraction of whole (0..1).</summary>
    public double Fraction { get; set; }
}
