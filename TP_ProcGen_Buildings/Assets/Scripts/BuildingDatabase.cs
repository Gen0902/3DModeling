using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingDatabase
{
    private static List<Module> buildings;

    public static void Initialize()
    {
        buildings = new List<Module>();

        var objects = Resources.LoadAll("Prefabs", typeof(Module));
        foreach (var item in objects)
            buildings.Add((Module)item);
    }

    public static Module GetRandomBuilding(BuildProperties buildProps)      
    {
        List<Module> tempList = buildings.FindAll(b => b.buildProperties.Match(buildProps));

        if (tempList.Count <= 0)
        {
            Debug.LogError("Building list is empty for this filters combination : " + buildProps.ToString());
            return null;
        }
        else
        {
            int r = Random.Range(0, tempList.Count);
            return tempList[r];
        }
    }
}
