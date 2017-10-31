using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public delegate void LoadSceneAction();

    // 声明一个静态GameManager 实例 以便其它脚本可以进行访问
    public static GameManager instance = null;

    [HideInInspector] public int LoadFileID { get; set; }

    void Awake() {
        // 判断实例是否已经退出
        if (instance == null)
            // 如何没有，实例化this
            instance = this;
        //如果已经实例化，但不是this
        else if (instance != this)
            // 销毁对象
            Destroy(gameObject);
        // 当正在重新加载场景时，设置this不被销毁
        DontDestroyOnLoad(gameObject);
    }


    #region Public Methods
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void ReStartGame() {

    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void ExitGame() {
        
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    /// <param name="FileID"></param>
    public void Save(int FileID) {
        DataController.Save(FileID);
    }

    /// <summary>
    /// 读取游戏
    /// </summary>
    /// <param name="fileID"></param>
    public void Load(int fileID) {
        // 存档索引
        LoadFileID = fileID;

        // 当场景加载完成后的事件监听，当场景加载完后，会自动调用Scenemanager.SceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 获取当前被激活的场景
        Scene preScene = SceneManager.GetActiveScene();

        // 重新加载当前场景
        SceneManager.LoadScene(preScene.buildIndex, LoadSceneMode.Single);

    }
    #endregion Public Methods

    /// <summary>
    /// 当场景加载完成后执行的事件，该方法在Awake之后调用，Start方法之前调用
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("GameManager: OnSceneLoaded 场景已经加载完成 ，场景名称为 " + scene.name);

        // 加载事件
        LoadData(LoadFileID);

        // 取消监听
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 加载存档数据
    /// </summary>
    /// <param name="FileID"></param>
    private void LoadData(int FileID) {
        // 加载存档数据
        DataController.Load(ArchiveFile.GetSaveFilePath(FileID));
    }

}
