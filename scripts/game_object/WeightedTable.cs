using Godot;
using System.Collections.Generic;
using System.Linq;

public class WeightedTable
{
    public List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

    private int _total_weight = 0;


    public void AddItem(object obj, int weight)
    {
        var item = new Dictionary<string, object>
        {
            { "object", obj },
            { "weight", weight }
        };

        items.Add(item);
        _total_weight += weight;
    }


    public object PickRandom()
    {
        List<Dictionary<string, object>> adjusted_items = items;
        int adjusted_total_weight = _total_weight;

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


    public void RemoveItem(object item)
    {
        var item_to_remove = items.FirstOrDefault(entry => entry["object"] == item);

        if (item_to_remove != null)
        {
            _total_weight -= (int) item_to_remove["weight"];
            items.Remove(item_to_remove);
        }
    }
}
