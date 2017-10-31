using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public enum UIState {
    Default,
    Menu,
    Dialogue,
    Select,
    Battle,
    GetProps,
    Stroy,
    MainMenu,
    CantMove
}
public class UIManager : MonoBehaviour {

    public static UIManager instance = null;
    public UIState CurrentState;
    [HideInInspector] public Dialogue dialogue;
    [HideInInspector] public Battle battle;
    [HideInInspector] public Remainder remainder;
    [HideInInspector] public MainMenu mainMenu;
    [HideInInspector] public SaveFilesMenu saveFileMenu;
    [HideInInspector] public PlayerMenu playerMenu;

    private void Awake() {

        // 判断实例是否已经退出
        if (instance == null)
            // 如何没有，实例化this
            instance = this;
        //如果已经实例化，但不是this
        else if (instance != this)
            // 销毁对象
            Destroy(gameObject);

        // 当正在重新加载场景时，设置this不被销毁
        //DontDestroyOnLoad(gameObject);
        // 获取访问主菜单脚本的引用
        mainMenu = GetComponent<MainMenu>();
        // 获取存档文件菜单脚本引用
        saveFileMenu = GetComponent<SaveFilesMenu>();

        // 获取访问Dialogue脚本的引用
        dialogue = GetComponent<Dialogue>();
        // 获取访问Battle脚本的引用
        battle = GetComponent<Battle>();
        // 获取访问Remainder脚本引用
        remainder = GetComponent<Remainder>();

        playerMenu = GetComponent<PlayerMenu>();


        CurrentState = UIState.Default;
    }
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(CurrentState == UIState.Default) {
                mainMenu.OpenMainMenu();
                CurrentState = UIState.MainMenu;
                Player.CurrentState = UIState.MainMenu;

            }else if(CurrentState == UIState.MainMenu) {
                mainMenu.CloseMainMenu();
                CurrentState = UIState.Default;
                Player.CurrentState = UIState.Default;
            }
        }

    }

}
