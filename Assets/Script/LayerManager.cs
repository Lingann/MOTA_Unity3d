using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本附加在LayerMap对象上，用于管理级别加载和索引
/// </summary>
public class LayerManager : MonoBehaviour {

    // 声明一个静态GameManager 实例 以便其它脚本可以进行访问
    public static LayerManager instance = null;

    
    #region Data Property
    // 级别数据
    public MapData Data = new MapData();

    // 当前楼层，指主角所在楼层
    public int CurrentLayerIndex { get; set; }

    // 激活楼层，指主角所到过的楼层
    public List<int> EnableLayers = new List<int>();
    #endregion   


    #region Public Field in Editor

    // 获取Layers
    public GameObject LayersParent;

    #endregion


    // 获取当前楼层
    [HideInInspector]
    public GameObject currentLayer;

    // 获取下一层
    private Layer nextLayer;

    // 获取 所有Layer
    private Layer[] layers;


    #region MonoBehivour
    /// <summary>
    /// 当对象被唤醒时调用
    /// 如果对象的属性是需要被保存的，请在该方法中获取该属性
    /// </summary>
    void Awake() {
        // 判断实例是否已经退出
        if (instance == null)
            // 如何没有，实例化this
            instance = this;
        //如果已经实例化，但不是this
        else if (instance != this)
            // 销毁对象
            Destroy(gameObject);

        // 监听数据事件
        AddEvent();

        // 查找获取 所有楼层
        layers = LayersParent.transform.GetComponentsInChildren<Layer>(true);

        // 将初始楼层设置为0
        currentLayer = layers[0].gameObject;
        if (!EnableLayers.Contains(0))
            EnableLayers.Add(0);

        CloseAllLayers();
    }


    /// <summary>
    /// 
    /// </summary>
    private void Start() {
        // 如果当前层未被激活，则激活当前层
        if(!currentLayer.activeSelf)
            currentLayer.SetActive(true);
    }

    /// <summary>
    /// 当游戏对象被销毁时调用
    /// </summary>
    private void OnDestroy() {
        // 当游戏对象销毁时，取消订阅数据操作事件
        DeleteEvent();
    }

    #endregion MonoBehaiour

    #region GetData
    /// <summary>
    /// 通过Transform查找路径(不包含根节点)
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static string GetPath(Transform transform) {

        string path = transform.name;

        while (transform.parent.parent != null) {

            transform = transform.parent;

            // 该语句可以避免查找到根节点，由于需要根据该根节点来查找对象，所以不能包含根节点

            path = transform.name + "/" + path;

        }

        Debug.Log(path);
        return path;
    }

    /// <summary>
    /// 查找父对象路径
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static string GetParentPath(Transform transform) {

        Transform parent = transform.parent;

        return GetPath(parent);

    }

    /// <summary>
    /// 获取楼层
    /// </summary>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    public Layer GetLayer(int layerIndex) {
        for (int i = 0; i < layers.Length; i++) {
            if (layerIndex == layers[i].CurrentLayer)
                return layers[i];
        }
        return null;
    }

    #endregion GetData

    #region DataOperate
    ///// <summary>
    ///// 存储数据
    ///// </summary>
    private void StoreData() {
            Data.EnableLayers = EnableLayers;
            Data.CurrentLayerIndex = currentLayer.GetComponent<Layer>().CurrentLayer;
        }

    /// <summary>
    ///  添加数据
    /// </summary>
    private void AddData() {
        DataController.dataContainer.Map.Add(Data);
    }

    /// <summary>
    /// 加载数据,该方法将在Awake之后，Start方法之前调用
    /// </summary>
    private void LoadData() {
        // 关闭所有楼层
        CloseAllLayers();
        // 获取所有被激活的层
        EnableLayers = Data.EnableLayers;
        // 跳到预先保存的层
        JumpLayer(Data.CurrentLayerIndex);
    }

    /// <summary>
    /// 订阅事件
    ///// </summary>
    void AddEvent() {
        DataController.OnStoreData += StoreData;
        DataController.OnAddData += AddData;
        DataController.OnLoaded += LoadData;
    }

    ///// <summary>
    ///// 取消订阅
    ///// </summary>
    void DeleteEvent() {
        DataController.OnStoreData -= StoreData;
        DataController.OnAddData -= AddData;
        DataController.OnLoaded -= LoadData;
    }
#endregion DataOperate


    #region 逻辑方法

    /// <summary>
    /// 上下楼梯
    /// </summary>
    /// <param name="player"></param>
    /// <param name="stairState"></param>
    public void DownOrUp(Player player, StairState stairState) {
        // 获取当前层
        Layer preLayer = currentLayer.GetComponent<Layer>();

        // 获取当前楼层的索引
        int i = preLayer.CurrentLayer;

        // 判断是否将要进行下楼层
        if (stairState == StairState.Down) {
            // 当前楼层设置为fasle
            currentLayer.SetActive(false);

            // 当前楼层数-1
            i = i - 1;
            // 判断当前楼层是否被包含在激活的楼层集合中，如果没有包含，则添加当前楼层到激活的楼层集合中
            if (!EnableLayers.Contains(i))
                EnableLayers.Add(i);

            currentLayer = layers[i].gameObject;

            // 下一楼层为
            nextLayer = currentLayer.GetComponent<Layer>();

            // 将改变后的楼层设置为true
            currentLayer.SetActive(true);

            // 设置主角在楼层的位置
            player.transform.position = nextLayer.UpPosition.position;

        } else if (stairState == StairState.Up) {

            // 设置当前楼层为false
            currentLayer.SetActive(false);

            // 当前楼层数+1
            i = i + 1;

            // 判断当前楼层是否被包含在激活的楼层集合中，如果没有包含，则添加当前楼层到激活的楼层集合中
            if (!EnableLayers.Contains(i))
                EnableLayers.Add(i);

            currentLayer = layers[i].gameObject;

            // 下一楼层为
            nextLayer = currentLayer.GetComponent<Layer>();

            // 将改变后的楼层设置为true
            currentLayer.SetActive(true);

            // 设置主角在楼层的位置
            player.transform.position = nextLayer.DownPosition.position;
        }

    }

    /// <summary>
    /// 判断是否能够跳转的楼层
    /// </summary>
    /// <param name="i">楼层索引</param>
    /// <returns></returns>
    private bool IsCanJump(int i) {
        if (EnableLayers.Contains(i))
            return true;
        return false;
    }

    /// <summary>
    /// 跳转到指定楼层
    /// </summary>
    /// <param name="i">楼层索引</param>
    private void JumpLayer(int i) {
        if (!IsCanJump(i)) {
            Debug.Log("无法到达指定楼层");
            Debug.Log("可以去的楼层总数目为： " + EnableLayers.Count);
            foreach(int item  in EnableLayers) {
                Debug.Log("可以去的楼层为：" + item.ToString());
            }
            return;
        }

        if (currentLayer == null)
            Debug.Log("当前层没有找到");

        //先关闭当前楼层
        currentLayer.SetActive(false);

        currentLayer = layers[i].gameObject;

        currentLayer.SetActive(true);

    }


    /// <summary>
    /// 关闭所有楼层
    /// </summary>
    private void CloseAllLayers() {
        foreach(Layer item in layers) {
            item.gameObject.SetActive(false);
        }
    }
#endregion 逻辑方法
}
