using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


public enum PropsEnum {
    Null,
    黄钥匙,
    蓝钥匙,
    红钥匙,
    大黄钥匙,
    红宝石,
    蓝宝石,
    红血瓶,
    蓝血瓶,
    铁剑,
    铁盾
}
public class Props : MonoBehaviour {
    
    #region DataProperty
    public PropsData Data = new PropsData();
    public string PropsName { get; set; }
    public string ParentPath { get { return GetParentPath(); } }
    public string PrefabPath { get { return PREFAB_BASE + PREFAB_NAME; } }

    private const string PREFAB_BASE = "Prefabs/Props/";
    private const string PREFAB_NAME = "props";

    #endregion

    #region PropsConfig
    public PropsEnum PropsType;
    public int Num { get; set; }
    public int Multiple { get; set; }
    public string Property { get; set; }
    #endregion PropsConfig

    #region  MonoBehaviour Events
    /// <summary>
    /// 当对象被唤醒时调用
    /// 如果对象的属性是需要被保存的，请在该方法中获取该属性
    /// </summary>
    private void Awake() {
        // 当对象被唤醒时，加入数据读写事件监听
        AddEvent();

        // 获取当前道具名
        PropsName = GetPropsName();

    }


    // Use this for initialization
    void Start() {
        // 读取配置文件数据
        GetInitalData();
    }

    private void OnDestroy() {
        DeleteEvent();
    }

    #endregion MonoBehaviour Events
    
    #region Get Data Methods
    /// <summary>
    /// 加载配置文件数据到当前对象
    /// </summary>
    private void GetInitalData() {
        string fileName = "InitialData";
        JsonData PropsData = ResourcesManager.GetJsonData(fileName, "Props");
        for (int i = 0; i < PropsData.Count; i++) {
            if (PropsName == PropsData[i]["Name"].ToString()) {
                Num = (int)PropsData[i]["Num"];
                Multiple = (int)PropsData[i]["Multiple"];
                Property = PropsData[i]["Property"].ToString();
                Debug.Log(PropsName + "Multiple is :" + Multiple);
            }
        }
    }

    /// <summary>
    /// 获取父对象路径
    /// </summary>
    /// <returns></returns>
    private string GetParentPath() {
        return LayerManager.GetParentPath(transform);
    }


    private PropsEnum GetPropsType(string PropsName) {
        switch (PropsName) {
            case "黄钥匙":
                return PropsType = PropsEnum.黄钥匙;
            case "蓝钥匙":
                return PropsType = PropsEnum.蓝钥匙;
            case "黄色大钥匙":
                return PropsType = PropsEnum.大黄钥匙;
            case "红钥匙":
                return PropsType = PropsEnum.红钥匙;
            case "红宝石":
                return PropsType = PropsEnum.红宝石;
            case "蓝宝石":
                return PropsType = PropsEnum.蓝宝石;
            case "铁剑":
                return PropsType = PropsEnum.铁剑;
            case "铁盾":
                return PropsType = PropsEnum.铁盾;
        }
        transform.name = PropsName;
        return PropsType = PropsEnum.Null;
    }

    private string GetPropsName() {
        switch (PropsType) {
            case PropsEnum.黄钥匙:
                return PropsName = "黄钥匙";
            case PropsEnum.蓝钥匙:
                return PropsName = "蓝钥匙";
            case PropsEnum.红钥匙:
                return PropsName = "红钥匙";
            case PropsEnum.大黄钥匙:
                return PropsName = "黄色大钥匙";
            case PropsEnum.红宝石:
                return PropsName = "红宝石";
            case PropsEnum.蓝宝石:
                return PropsName = "蓝宝石";
            case PropsEnum.铁剑:
                return PropsName = "铁剑";
            case PropsEnum.铁盾:
                return PropsName = "铁盾";
        }
        return PropsName = "";
    }
    #endregion GetData

    #region SetData
    /// <summary>
    /// 设置游戏对象路径
    /// </summary>
    /// <param name="parentName"></param>
    /// <param name="belongLayer"></param>
    private void SetParent(string parentName, int belongLayer) {
        // 获取父对象
        Transform parent = LayerManager.instance.transform.Find(Data.ParentPath);

        // 如果父对象不为空，设置为父对象
        if (parent != null) {
            transform.SetParent(parent);
            return;
        }

        // 创建一个新的对象作为父对象
        transform.parent = new GameObject(parentName).transform;

        // 设置父对象的名字
        transform.parent.name = parentName;

        // 设置父对象所在的父节点
        transform.parent.parent = LayerManager.instance.GetLayer(belongLayer).transform;

    }

    #endregion SetData


    #region Data Operate

    /// <summary>
    /// 存储数据
    /// </summary>
    void StoreData() {
        Data.ParentPath = ParentPath;
        Data.PrefabPath = PrefabPath;
        Data.PropsName = PropsName;
        Data.ParentName = transform.parent.name;
        Data.LocalLayer = transform.parent.parent.GetComponent<Layer>().CurrentLayer; ;
        Data.SpriteName = GetComponent<SpriteRenderer>().sprite.name;
        Debug.Log(Data.SpriteName);
        Data.PositionX = transform.position.x;
        Data.PositionY = transform.position.y;
        Data.PositionZ = transform.position.z;
    }

    /// <summary>
    /// 加载存档数据
    /// </summary>
    void LoadData() {
        Debug.Log(Data.SpriteName);
        GetComponent<SpriteRenderer>().sprite = ResourcesManager.GetSprite(Data.SpriteName);
        PropsName = Data.PropsName;
        PropsType = GetPropsType(Data.PropsName);
        transform.name = Data.PropsName;
        SetParent(Data.ParentName, Data.LocalLayer);
        transform.position = new Vector3((float)Data.PositionX, (float)Data.PositionY, (float)Data.PositionZ);
        GetInitalData();
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    void AddData() {
        DataController.dataContainer.PropsData.Add(Data);
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

    #endregion DataOperate

    #region Logic Method
    public string Reminder() {
        if (Num > 0) {
            string message = Property + "增加 " + Num;
            return message;
        } else if (Num < 0) {
            string message = Property + "减少 " + Num;
            return message;
        } else {
            return null;
        }
    }
    #endregion Logic Method

}