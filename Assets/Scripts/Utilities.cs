using System.Collections.Generic;
using System.Linq;
using GameMap.Locations;
using Spells;

public static class Utilities
{
    public static IEnumerable<Spell> QuickSpellSortDescending(IEnumerable<Spell> list)
    {
        if (list.Count() <= 1) return list;
        var pivot = list.First();

        var less = list.Skip(1).Where(i => i.SpellCost <= pivot.SpellCost);
        var greater = list.Skip(1).Where(i => i.SpellCost > pivot.SpellCost);

        return QuickSpellSortDescending(greater).Union(new List<Spell> {pivot}).Union(QuickSpellSortDescending(less));
    }

    //Ascending

    public static IEnumerable<LocationPoint> QuickSortAscending(IEnumerable<LocationPoint> list)
    {
        if (list.Count() <= 1) return list;
        var pivot = list.First();

        var less = list.Skip(1).Where(i => i.LocationData.LocationNumber <= pivot.LocationData.LocationNumber);
        var greater = list.Skip(1).Where(i => i.LocationData.LocationNumber > pivot.LocationData.LocationNumber);

        return QuickSortAscending(less).Union(new List<LocationPoint> {pivot}).Union(QuickSortAscending(greater));
    }
}