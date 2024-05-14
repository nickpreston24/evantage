using System.Collections.Generic;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;

namespace evantage.Models;

public class InstallmentPlan : Enumeration
{
    public string Abbreviation;

    public double GetEarnings()
    {
        var earning_lookup = new Dictionary<InstallmentPlan, double>()
        {
            { NextUp, 10 },
            { Other, 0 },
        };

        var _ = earning_lookup.TryGetValue(this, out var found);
        found.Dump("found installment cost :>> ");
        return found;
    }

    public static InstallmentPlan NextUp => new(1, nameof(NextUp).ToLower(), "NU");
    public static InstallmentPlan Other => new(2, nameof(Other).ToLower(), "Other");

    public InstallmentPlan(int id, string name, string abbreviation) : base(id, name)
    {
        Abbreviation = abbreviation;
    }
}