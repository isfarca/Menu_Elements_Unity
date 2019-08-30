using UnityEngine;
using System;
using Malee;

[Serializable] public class MenuObject
{
    public string type;
    public GameObject prefab;
    public Type Type { get { return Type.GetType(type + ", Assembly-CSharp"); } }
}

[Serializable] public class MenuList : ReorderableArray<MenuObject>
{
    public GameObject GetPrefab<T>() where T : Menu
    {
        var menu = array.Find(x => typeof(T) == x.Type);

        return menu.prefab;
    }

    public GameObject GetPrefab(Type type)
    {
        var menu = array.Find(x => type == x.Type);

        return menu.prefab;
    }
}