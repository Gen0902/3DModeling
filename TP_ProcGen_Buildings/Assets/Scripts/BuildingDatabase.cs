using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingDatabase
{
    private static List<Building> buildings;

    public static void Initialize()
    {
        buildings = new List<Building>();

        var objects = Resources.LoadAll("Prefabs", typeof(Building));
        foreach (var item in objects)
            buildings.Add((Building)item);
    }

    public static Building GetRandomBuilding(BuildProperties buildProps)
    {
        List<Building> tempList = buildings.FindAll(b => b.buildProperties.Match(buildProps));

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
