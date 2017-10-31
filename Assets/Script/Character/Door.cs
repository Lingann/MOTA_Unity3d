using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorEnum {
    Null,
    黄门,
    蓝门,
    红门,
    绿门,
    牢门
}

public class Door : MonoBehaviour {

    public DoorEnum DoorType;

    #region Data Property
    DoorData Data = new DoorData();
    public string DoorName { get; set; }
    public string ParentPath { get { return GetParentPath(); } }
    public string PrefabPath { get { return PREFAB_BASE + DoorName; } }

    private const string PREFAB_BASE = "Prefabs/Doors/";
    #endregion

    private bool isOpening;
    public bool IsOpening {
        get { return isOpening; }
        set { isOpening = value; }
    }

    Animator Anim;

    #region MonoBehaviour
    // Use this for initialization
    void Awake () {
        AddEvent();

	}

    private void Start() {
        DoorName = GetDoorName();
        isOpening = false;

        Anim = GetComponent<Animator>();
    }

    private void OnDestroy() {
        DeleteEvent();
    }
    #endregion MonoBehaviuor

    #region GetData
    /// <summary>
    /// 获取父对象路径
    /// </summary>
    /// <returns></returns>
    private string GetParentPath() {
        return LayerManager.GetParentPath(transform);
    }


    private DoorEnum GetDoorType(string DoorName) {
        switch (DoorName) {
            case "黄门":
                return DoorType = DoorEnum.黄门;
            case "蓝门":
                return DoorType = DoorEnum.蓝门;
            case "红门":
                return DoorType = DoorEnum.红门;
            case "绿门":
                return DoorType = DoorEnum.绿门;
            case "牢门":
                return DoorType = DoorEnum.牢门;
        }
        return DoorType = DoorEnum.Null;
    }

    private string GetDoorName() {
        switch (DoorType) {
            case DoorEnum.黄门:
                return DoorName = "黄门";
            case DoorEnum.蓝门:
                return DoorName = "黄门";
            case DoorEnum.红门:
                return DoorName = "黄门";
            case DoorEnum.绿门:
                return DoorName = "绿门";
            case DoorEnum.牢门:
                return DoorName = "牢门";
        }
        return DoorName = "";
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
    private void Destroy() {
        Destroy(gameObject);
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    private void StoreData() {
        Data.PrefabPath = PrefabPath;
        Data.ParentPath = LayerManager.GetParentPath(transform);
        Data.BelongLayer = transform.parent.parent.GetComponent<Layer>().CurrentLayer;
        Data.ParentName = transform.parent.name;
        Data.PositionX = transform.position.x;
        Data.PositionY = transform.position.y;
        Data.PositionZ = transform.position.z;
    }
    /// <summary>
    /// 加载存档数据
    /// </summary>
    private void LoadData() {
        SetParent(Data.ParentName,Data.BelongLayer);
        transform.position = new Vector3((float)Data.PositionX, (float)Data.PositionY, (float)Data.PositionZ);
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    private void AddData() {
        DataController.dataContainer.Doors.Add(Data);
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    private void AddEvent() {
        DataController.OnStoreData += StoreData;
        DataController.OnAddData += AddData;
        DataController.OnLoaded += LoadData;
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    private void DeleteEvent() {
        DataController.OnStoreData -= StoreData;
        DataController.OnAddData -= AddData;
        DataController.OnLoaded -= LoadData;
    }
    #endregion Data Operate

    #region Logic Methods
    /// <summary>
    /// 开门动画
    /// </summary>
    public void Open() {
        // 将动画控制器条件设置为打开
        Anim.SetBool("Open", true);

        // 设置当前门的状态为 isOpening
        isOpening = true;
        
        //开启打开门动画协程
        StartCoroutine(Opening());
    }

    /// <summary>
    ///  开门事件
    /// </summary>
    /// <param name="_obj"></param>
    private IEnumerator Opening() {
        // 下一帧再进行判断
        yield return null;
        
        // 判断当前动画是否为空
        if (Anim != null) {

            // 判断是否正在进行门打开动画.
            // 第一次进入的时候不知道什么鬼，DoorAnim.GetCurrentAnimatorStateInfo(0).IsName("yellowDoorOpen")  = true.
            // 这可能是该方法在Unity内部的执行顺序问题。可能是开启动画后， 在第一帧并没有被启动，同时该函数的值并不是依靠动画参数来决定， 所以就导致该函数的值为false
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName("DoorOpening")) {
                
                StartCoroutine(Opening());
            } else {
                // 设置门状态为关闭
                isOpening = false;

                // 销毁对象需要一个过程，这个过程使得该协程继续执行了次
                gameObject.SetActive(false);
                
                // 销毁对象，由于销毁了对象，所以协程不再执行
                Destroy(gameObject);
            }
        }
        // 下面语句会在 索引A 后输出两次
        //Debug.Log("索引B"); 
    }
    #endregion


}
