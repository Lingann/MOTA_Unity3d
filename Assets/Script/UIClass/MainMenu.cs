using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject MainMenuPanel;

    private void Start() {
        MainMenuPanel.SetActive(false);
    }

    //public IEnumerator OpenMainMenu() {
    //    Debug.Log(MainMenuPanel.activeSelf);
    //    MainMenuPanel.SetActive(true);
    //    yield return new WaitForSeconds(0.5f);
    //    UIManager.instance.CurrentState = UIState.MainMenu;
    //}

    //public IEnumerator CloseMianMenu() {
    //    if(MainMenuPanel.activeSelf)
    //        MainMenuPanel.SetActive(false);
    //    yield return new WaitForSeconds(0.3f);
    //    UIManager.instance.CurrentState = UIState.Default;
    //}

    /// <summary>
    /// 打开主菜单
    /// </summary>
    public void OpenMainMenu() {
        MainMenuPanel.SetActive(true);
        GetComponent<MonsterMenu>().OpenMainMenu();
 
    }
    
    /// <summary>
    /// 关闭主菜单
    /// </summary>
    public void CloseMainMenu() {
        MainMenuPanel.SetActive(false);
        GetComponent<MonsterMenu>().CloseMainsMenu();
        GetComponent<SaveFilesMenu>().CloseSaveFilesMenu();
    }

    /// <summary>
    /// 关闭所有菜单
    /// </summary>
    public void CloseAllMenu() {
        GetComponent<MonsterMenu>().CloseMonsterMenu();
        GetComponent<SaveFilesMenu>().CloseSaveFilesMenu();
    }
}
