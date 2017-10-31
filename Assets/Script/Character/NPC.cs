using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour {
    public NPCData Data = new NPCData();
    public string NPCName;
    public string PrefabPath { get { return "Prefabs/Characters/" + NPCName; } }
    public string ParentPath { get { return LayerManager.GetParentPath(transform); } }

    private const string PREFAB_BASE = "Prefabs/NPCs/";

    public UnityEvent OnStartDialogue;
    public UnityEvent OnEndDialogue;

    // Use this for initialization
    void Awake () {
        AddEvent();

	}

    private void OnDestroy() {
        DeleteEvent();
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
    void StoreData() {
        Data.PrefabPath = PrefabPath;
        Data.BelongLayer = transform.parent.parent.GetComponent<Layer>().CurrentLayer;
        Data.ParentPath =  LayerManager.GetParentPath(transform);
        Data.ParentName = transform.parent.name;
        Data.PositionX = transform.position.x;
        Data.PositionY = transform.position.y;
        Data.PositionZ = transform.position.z;
    }
    /// <summary>
    /// 加载存档数据
    /// </summary>
    void LoadData() {
        SetParent();
        transform.position = new Vector3((float)Data.PositionX, (float)Data.PositionY, (float)Data.PositionZ);
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    void AddData() {
        DataController.dataContainer.NPCs.Add(Data);
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
    

    public void Execute(string name) {
        UIManager.instance.dialogue.StartDialogue(name);
    }

    void DisPlayOptions() {

    }

    void StartSelect() {
        
    }



    public void Execute(Player player,int num) {

    }


}
