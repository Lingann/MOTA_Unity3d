using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour {
    public delegate void LoadSceneAction();


    // 声明一个静态GameManager 实例 以便其它脚本可以进行访问
    public static EditorManager instance = null;



    [HideInInspector] public int LoadFileID;

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

    public void Save(int FileID) {
        DataController.Save(FileID);
    }

    public void LoadData(int FileID) {
        DataController.Load(ArchiveFile.GetSaveFilePath(FileID));
    }

    public void Load(int fileID) {

        LoadFileID = fileID;

        // 当场景加载完成后的事件监听
        SceneManager.sceneLoaded += OnSceneLoaded;


        Scene preScene = SceneManager.GetActiveScene();


        SceneManager.LoadScene(preScene.buildIndex, LoadSceneMode.Single);

    }

    /// <summary>
    /// 当场景加载完成后执行的事件
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("GameManager: OnSceneLoaded 场景已经加载完成 ，场景名称为 " + scene.name);


        // 加载事件
        LoadData(LoadFileID);

        // 取消监听
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
