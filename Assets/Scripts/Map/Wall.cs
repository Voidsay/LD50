using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Room", menuName = "Map/Room")]
public class Wall : ScriptableObject
{
    public GameObject prefab;
    public Connections connection;
    [Min(1)]
    public int weight;
    public bool streetType;
}

[Flags]
[Serializable]
public enum Connections
{
    none = 0,
    //door
    du = 1 << 0,
    dr = 1 << 1,
    dd = 1 << 2,
    dl = 1 << 3,
    //window
    wu = 1 << 4,
    wr = 1 << 5,
    wd = 1 << 6,
    wl = 1 << 7,
    //breakable wall
    bu = 1 << 8,
    br = 1 << 9,
    bd = 1 << 10,
    bl = 1 << 11,
    //facade wall
    fu = 1 << 12,
    fr = 1 << 13,
    fd = 1 << 14,
    fl = 1 << 15,
    //interior wall
    iu = 1 << 16,
    ir = 1 << 17,
    id = 1 << 18,
    il = 1 << 19,

    border = 1 << 30,
    placed = 1 << 31
}