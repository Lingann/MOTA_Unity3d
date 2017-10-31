using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Monster : MonoBehaviour {
    public MonsterData Data = new MonsterData();

    // Monster名称
    public string MonsterName;
    public string PrefabPath { get { return PREFAB_BASE + MonsterName; } }
    public string ParentPath { get { return LayerManager.GetParentPath(transform); } }

    private const string PREFAB_BASE = "Prefabs/Monsters/";

    public int HP { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Golds { get; set; }
    public int Exp { get; set; }


    private void Awake() {
        AddEvent();
    }
    // Use this for initialization
    void Start () {
        GetMonsterValue();
    }

    private void OnDestroy() {
        DeleteEvent();
    }

    /// <summary>
    /// 获取怪物数据配置信息
    /// </summary>
    private void GetMonsterValue() {
        string fileName = "InitialData";
        JsonData MonsterData = ResourcesManager.GetJsonData(fileName, "Monster");
        for (int i = 0; i < MonsterData.Count; i++) {
            if (MonsterName == MonsterData[i]["Name"].ToString()) {
                HP = (int)MonsterData[i]["HP"];
                Atk = (int)MonsterData[i]["Atk"];
                Def = (int)MonsterData[i]["Def"];
                Golds = (int)MonsterData[i]["Gold"];
                Exp = (int)MonsterData[i]["Exp"];
            }
        }
    }

    #region SetData
    /// <summary>
    /// 设置游戏对象路径
    /// </summary>
    private void SetParent() {
        // 获取父对象
        Transform parent = LayerManager.instance.transform.Find(Data.ParentPath);

        // 如果父对象不为空，设置为父对象
        if (parent != null) {
            transform.SetParent(parent);
            return;
        }

        // 创建一个新的对象作为父对象
        transform.parent = new GameObject(Data.ParentName).transform;

        // 设置父对象所在的父节点
        transform.parent.parent = LayerManager.instance.GetLayer(Data.BelongLayer).transform;

    }
    #endregion SetData

    /// <summary>
    /// 存储数据
    /// </summary>
    public void StoreData() {
        Data.PrefabPath = PrefabPath;
        Data.BelongLayer = transform.parent.parent.GetComponent<Layer>().CurrentLayer;
        Data.ParentPath = ParentPath;
        Data.ParentName = transform.parent.name;

        Data.PositionX = transform.position.x;
        Data.PositionY = transform.position.y;
        Data.PositionZ = transform.position.z;
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    public void AddData() {
        DataController.dataContainer.Monsters.Add(Data);
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadData() {
        SetParent();
        transform.position = new Vector3((float)Data.PositionX, (float)Data.PositionY, (float)Data.PositionZ);
    }


    /// <summary>
    /// 订阅事件
    /// </summary>
    void AddEvent() {
        DataController.OnStoreData += StoreData;
        DataController.OnAddData += AddData;
        DataController.OnLoaded += LoadData;
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    void DeleteEvent() {
        DataController.OnStoreData -= StoreData;
        DataController.OnAddData -= AddData;
        DataController.OnLoaded -= LoadData;
    }

    /// <summary>
    /// 攻击方法
    /// </summary>
    /// <param name="Atk">自身攻击力</param>
    /// <param name="_obj">攻击对象</param>

    //public int DemageValue(int otherAtk, int otherDef) {
    //    int DamageValue = (HP / (otherAtk - Def)) * (Atk - otherDef) - (Atk - otherDef);
    //    return DamageValue;
    //}

    public IEnumerator Attack() {
        yield return null;
    }
    public void Damage(int otherAtk) {
        HP = HP - (otherAtk - Def);
    }

    public int GetDemageValue(int otherAtk, int otherDef) {
        int DamageValue = Mathf.CeilToInt(HP / (otherAtk - Def)) * (Atk - otherDef) ;
        return DamageValue;
    }
}
