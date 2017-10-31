using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[Serializable]
public class DataController{

    public static DataContainer dataContainer = new DataContainer();

    // 序列化事件委托
    public delegate void SerializeAction();

    // 加载事件，用于监听游戏对象的LoadData方法
    public static event SerializeAction OnLoaded;

    //存储事件，用于监听游戏对象的StoreData方法
    public static event SerializeAction OnStoreData;

    // 添加数据事件，用于监听游戏对象的AddData方法
    public static event SerializeAction OnAddData;

    /// <summary>
    /// 加载游戏对象
    /// </summary>
    /// <param name="path">存档路径</param>
    public static void Load(string path) {
        dataContainer = LoadArchive(path);

        //Debug.Log("存档路径为" + path);
        //Debug.Log( "主角数据 " + dataContainer.Player.Count);
        //Debug.Log("NPC数据" + dataContainer.NPCs.Count);
        //Debug.Log("怪物数据 " + dataContainer.Monsters.Count);
        //Debug.Log("楼层数据" + dataContainer.Map.Count);
        //Debug.Log("道具数据" + dataContainer.PropsData.Count);

        // 获取地图数据
        foreach (MapData mapData in dataContainer.Map) {
            LayerManager.instance.Data = mapData;

            Debug.Log("Load方法被加载，LayerManager从数据源获取到的当前楼层" + LayerManager.instance.Data.CurrentLayerIndex);
        }

        // 查找或创建Player，并覆盖数据
        foreach (PlayerData playerData in dataContainer.Player) {

            if (GameObject.Find("Player") != null) {

                Player prePlayer = GameObject.Find("Player").GetComponent<Player>();

                prePlayer.Data = playerData;

                break;
            }

            Player player = InstantiatePrefab<Player>(playerData.PrefabPath);

            player.Data = playerData;

        }

        // 创建并加载NPC数据
        foreach (NPCData npcData in dataContainer.NPCs) {

            NPC npc = InstantiatePrefab<NPC>(npcData.PrefabPath);

            npc.Data = npcData;

        }

        // 创建并加载怪物数据
        foreach (MonsterData monsterData in dataContainer.Monsters) {

            Monster monster = InstantiatePrefab<Monster>(monsterData.PrefabPath);

            monster.Data = monsterData;

        }

        // 创建并加载道具数据
        foreach (var propsData in dataContainer.PropsData) {
            Debug.Log(dataContainer.PropsData.Count);
            Props props = InstantiatePrefab<Props>(propsData.PrefabPath);
            props.Data = propsData;
        }

        // 调用加载数据事件
        OnLoaded();

        // 清除数据对象容器
        ClearDataContainer();
    }

    /// <summary>
    /// 保存并添加数据
    /// </summary>
    public static void AddData() {
        //// 保存数据到数据对象
        OnStoreData();

        // 将数据对象添加到dataContainer中
        OnAddData();
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    public static void Save() {
        // 保存并添加数据
        AddData();

        // 数据对象序列化
        string dataString = JsonMapper.ToJson(dataContainer);

        // 创建存档
        ArchiveFile.AddArchive(dataString);

        // 清除当前数据对象容器
        ClearDataContainer();

    }


    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="FileID">保存存档的ID</param>
    public static void Save(int FileID) {
        // 保存并添加数据
        AddData();

        // 数据对象序列化
        string dataString = JsonMapper.ToJson(dataContainer);

        // 创建存档
        ArchiveFile.AddArchive(FileID, dataString);

        // 清除当前数据对象容器
        ClearDataContainer();

    }

    /// <summary>
    /// 将数据写入指定的存档文件中，并覆盖
    /// </summary>
    /// <param name="path"></param>
    public static void Save(string path) {

        // 数据对象序列化
        string dataString = JsonMapper.ToJson(dataContainer);

        // 覆盖存档存档
        ArchiveFile.OveriderArchive(path,dataString);

        // 清除当前数据对象容器
        ClearDataContainer();
    }


    /// <summary>
    /// 实例化预制
    /// </summary>
    /// <typeparam name="T">附加在预制上的组件</typeparam>
    /// <param name="prefabPath">预制路径</param>
    /// <returns></returns>
    public static T InstantiatePrefab<T>(string prefabPath) {

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
            Debug.Log("预制没有找到，预制路径为： " + prefabPath);

        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go == null)
            Debug.Log("实例化失败");

        T data = go.GetComponent<T>();

        return data;

    }

    /// <summary>
    /// 实例化预制
    /// </summary>
    /// <typeparam name="T">附加在预制上的组件</typeparam>
    /// <param name="prefabPath">预制路径</param>
    /// <param name="parentPath">父对象路径</param>
    /// <returns></returns>
    public static T InstantiatePrefab<T>(string prefabPath, string parentPath) {

        Transform parent = LayerManager.instance.transform.Find(parentPath);

        if (parent == null) {
            Debug.Log(parentPath);
            return InstantiatePrefab<T>(prefabPath);
        }

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) {
            Debug.Log("预制查找失败，预制路径为：" + prefabPath);
        }

        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go == null)
            Debug.Log("实例化失败");

        go.transform.SetParent(parent);

        T data = go.GetComponent<T>();

        return data;

    }




    /// <summary>
    /// 清除对象数据容器
    /// </summary>
    private static void ClearDataContainer() {

        dataContainer.Map.Clear();

        dataContainer.Player.Clear();

        dataContainer.NPCs.Clear();

        dataContainer.Monsters.Clear();

        dataContainer.PropsData.Clear();

        dataContainer.Doors.Clear();

        //dataContainer.Map.Clear();
    }

    /// <summary>
    /// 加载存档方法
    /// </summary>
    /// <param name="path">存档路径</param>
    /// <returns></returns>
    public static DataContainer LoadArchive(string path) {
        // 读取存档信息
        string json = File.ReadAllText(path);   
        // 返回存档信息
        return JsonMapper.ToObject<DataContainer>(json);

    }



}
