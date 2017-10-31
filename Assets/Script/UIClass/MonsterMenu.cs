using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMenu : MonoBehaviour {
    public Player player;

    public GameObject MonsterItemParentPrefab;

    public Button MonsterMenuButton;

    [HideInInspector]
    public GameObject MonsterItemParent;

    public GameObject MonsterItemPrefab;

    private Monster[] AllMonsters;

    private List<string> MonsterList = new List<string>();

    private GameObject CurrentLayer;

    Transform MonsterBox;

    Button btn;

    public void GetMonsters() {
        // 获取当前楼层
        CurrentLayer = LayerManager.instance.currentLayer;

        if (CurrentLayer == null)
            Debug.Log("当前层没有找到");
        

        // 找到当前楼层的monster容器
        MonsterBox = CurrentLayer.transform.Find("Monster");

        if(MonsterBox == null) {
            MonsterBox = new GameObject("Monster").transform;
            MonsterBox.parent = CurrentLayer.transform;
        }

        // 获取当前楼层下的所有monster
        AllMonsters = MonsterBox.GetComponentsInChildren<Monster>();
    }

    public void GetData() {
        MonsterItemParent = Instantiate(MonsterItemParentPrefab, GetComponent<MainMenu>().MainMenuPanel.transform);
        foreach (Monster temp in AllMonsters) {
            if (!MonsterList.Contains(temp.MonsterName)) {
                MonsterList.Add(temp.MonsterName);
                GameObject go = Instantiate(MonsterItemPrefab, MonsterItemParent.transform);
                MonsterItem monsterItem = go.GetComponent<MonsterItem>();
                monsterItem.MonsterSprite.sprite = temp.GetComponent<SpriteRenderer>().sprite;
                monsterItem.anim.runtimeAnimatorController = temp.GetComponent<Animator>().runtimeAnimatorController;


                monsterItem.MonsterName.text = temp.MonsterName;
                monsterItem.MonsterHP.text = temp.HP.ToString();
                monsterItem.MonsterATK.text = temp.Atk.ToString();
                monsterItem.MonsterDEF.text = temp.Def.ToString();
                monsterItem.MonsterGolds.text = temp.Golds.ToString();
                monsterItem.MonsterEXP.text = temp.Exp.ToString();
                int damageValue = temp.GetDemageValue(player.ATK, player.DEF);
                if (damageValue < 0)
                    monsterItem.MonsterDemageValue.text = "不可敌";
                else
                    monsterItem.MonsterDemageValue.text = damageValue.ToString();
            }
        }
    }

    /// <summary>
    /// 打开主菜单
    /// </summary>
    public void OpenMainMenu() {
        GetMonsters();
        GetData();
    }

    /// <summary>
    /// 打开怪物菜单
    /// </summary>
    public void OpenMonsterMenu() {
        MonsterItemParent.SetActive(true);
    }

    /// <summary>
    /// 关闭主菜单
    /// </summary>
    public void CloseMainsMenu() {
        Destroy(MonsterItemParent);
        MonsterList.Clear();
        
    }

    /// <summary>
    /// 关闭怪物列表
    /// </summary>
    public void CloseMonsterMenu() {
        MonsterItemParent.SetActive(false);
    }
}
