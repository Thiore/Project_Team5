using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour
{
    public static Dictionary<int, Item3D> childItem { get; private set; } = new Dictionary<int, Item3D>();



    public static void GetItem(int id, Item3D item)
    {
        childItem.Add(id, item);
    }
    public static void UseItem(int id)
    {
        childItem.Remove(id);
    }


}
