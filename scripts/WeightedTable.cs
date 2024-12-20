using Godot;
using System.Collections.Generic;

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


    public object PickRandom()
    {
        if (items.Count == 0)
            return null;

        int random_weight = (int) (GD.Randi() % total_weight);
        int current_weight = 0;

        foreach (var item in items)
        {
            current_weight += (int) item["weight"];
            if (random_weight <= current_weight)
                return item["object"];
        }

        return null;
    }
}
