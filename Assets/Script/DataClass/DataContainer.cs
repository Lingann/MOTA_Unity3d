using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataContainer{

    public List<MapData> Map = new List<MapData>();

    public List<PlayerData> Player = new List<PlayerData>();

    public List<MonsterData> Monsters = new List<MonsterData>();

    public List<DoorData> Doors = new List<DoorData>();

    public List<PropsData> PropsData = new List<PropsData>();

    public List<NPCData> NPCs = new List<NPCData>();

}
