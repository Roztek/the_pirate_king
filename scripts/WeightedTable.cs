using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedTable
{
    public List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

    public int total_weight = 0;


    public void AddItem(object obj, int weight)
    {
        var item = new Dictionary<string, object>
        {
            { "object", obj },
            { "weight", weight }
        };

        items.Add(item);
        total_weight += weight;
    }


    // TODO: Decide if we want this code. These two alternative methods relate to a comment in UpgradeManager
    //       which pertain to ability upgrades being offered twice

    // public object PickRandom(Array[] exclude = null)
    // {
    //     List<Dictionary<string, object>> adjusted_items = items;
    //     int adjusted_total_weight = total_weight;

    //     if (exclude != null && exclude.Length > 0)
    //     {
    //         adjusted_items = new List<Dictionary<string, object>>();
    //         adjusted_total_weight = 0;

    //         foreach (var item in items)
    //         {
    //             if (exclude.Contains(item["object"]))
    //                 continue;

    //             adjusted_items.Add(item);
    //             adjusted_total_weight += (int)item["weight"];
    //         }
    //     }

    //     if (items.Count == 0)
    //         return null;

    //     int random_weight = (int) (GD.Randi() % adjusted_total_weight);
    //     int current_weight = 0;

    //     foreach (var item in adjusted_items)
    //     {
    //         current_weight += (int) item["weight"];
    //         if (random_weight <= current_weight)
    //             return item["object"];
    //     }

    //     return null;
    // }


    // TODO: Use this PickRandom() if I decide to not offer the same ability twice where the functionality will
    //       be implemented in UpgradeManager. Will decide this in the future.

    public object PickRandom()
    {
        List<Dictionary<string, object>> adjusted_items = items;
        int adjusted_total_weight = total_weight;

        if (adjusted_items.Count == 0)
            return null;

        int random_weight = (int) (GD.Randi() % adjusted_total_weight);
        int current_weight = 0;

        foreach (var item in adjusted_items)
        {
            current_weight += (int)item["weight"];
            if (random_weight < current_weight)
                return item["object"];
        }

        return null;
    }


    // public object PickRandom(object[] exclude = null)
    // {
    //     List<Dictionary<string, object>> adjusted_items = new List<Dictionary<string, object>>();
    //     int adjusted_total_weight = 0;

    //     foreach (var item in items)
    //     {
    //         if (exclude != null && exclude.Contains(item["object"]))
    //             continue;

    //         adjusted_items.Add(item);
    //         adjusted_total_weight += (int) item["weight"];
    //     }

    //     if (adjusted_items.Count == 0)
    //         return null;

    //     int random_weight = (int)(GD.Randi() % adjusted_total_weight);
    //     int current_weight = 0;

    //     foreach (var item in adjusted_items)
    //     {
    //         current_weight += (int)item["weight"];
    //         if (random_weight < current_weight)
    //             return item["object"];
    //     }

    //     return null;
    // }


    public void RemoveItem(object item)
    {
        var item_to_remove = items.FirstOrDefault(entry => entry["object"] == item);

        if (item_to_remove != null)
        {
            total_weight -= (int) item_to_remove["weight"];
            items.Remove(item_to_remove);
        }
    }
}
