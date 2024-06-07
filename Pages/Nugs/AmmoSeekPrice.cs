using CodeMechanic.Maths;
using CodeMechanic.Types;

namespace evantage.Pages.Nugs;

public record AmmoSeekPrice
{
    public DateTime LastFetchDate { get; set; }
    public double LowestPricePerRound { get; set; }
    public AmmoseekCaliber Caliber { get; set; }
    public string Url { get; set; }

    // public double PricePerPound => (LowestPricePerRound / Caliber.MaxEffectiveRange * 100).RoundTo(8);

    // @(@price.LowestPricePerRound / @price.Caliber.MaxEffectiveRange / 100)
}

public class AmmoseekCaliber : Enumeration
{
    public static AmmoseekCaliber _308_Winchester =
        new AmmoseekCaliber(1, nameof(_308_Winchester), search: "308-winchester", range: 1000.00, grain: 147);

    public static AmmoseekCaliber _224_Valkyrie =
        new AmmoseekCaliber(2, nameof(_224_Valkyrie), range: 1300.00, search: "224-valkyrie", grain: 75);

    public static AmmoseekCaliber _300_Blackout =
        new AmmoseekCaliber(3, nameof(_300_Blackout), search: "300aac-blackout", range: 700.00, grain: 125,
            mv: 2200.00);

    public static AmmoseekCaliber _556_NATO =
        new AmmoseekCaliber(4, nameof(_556_NATO), range: 800.00, search: "5.56x45mm-nato");

    public static AmmoseekCaliber _65_Creedmoor =
        new AmmoseekCaliber(5, nameof(_65_Creedmoor), range: 1000.00, search: "6.5mm-creedmoor");

    public AmmoseekCaliber(int id
        , string name
        , string search
        , double range = 0.00
        , double grain = 0.00
        , double mv = 0.00)
        : base(id, name)
    {
        this.SearchUrl = $"https://ammoseek.com/ammo/{search}";
        // MaxEffectiveRange = range;
        // Grain = grain;
        // MuzzleVelocity = mv;
    }

    // public double Grain { get; set; }
    // public double MuzzleVelocity { get; set; }
    // public double Force => (Math.Pow(MuzzleVelocity, 2) * Grain) / 450437; // POWER({MuzzleVelocity},2)*Grain/450437 

    // public double MaxEffectiveRange { set; get; } =
        // -1.00; // Whatever's advertised on the box or the average expected.  Not accurate, just for show.

    public string SearchUrl { get; set; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}