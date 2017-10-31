using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {
    public GameObject BattlePanle;
    public Image MonsterSprite;
    public Text MonsterName;
    public Text MonsterHP;
    public Text MonsterAtk;
    public Text MonsterDef;

    public Text HeroHP;
    public Text HeroAtk;
    public Text HeroDef;

    public void StartBattle(GameObject _hero, GameObject _monster) {
        UIManager.instance.CurrentState = UIState.Battle;
        BattlePanle.SetActive(true);
        Player hero = _hero.GetComponent<Player>();
        Sprite monsterSprite = _monster.GetComponent<SpriteRenderer>().sprite;
        Monster monster = _monster.GetComponent<Monster>();
        HeroHP.text = hero.HP.ToString();
        HeroAtk.text = hero.ATK.ToString();
        HeroDef.text = hero.DEF.ToString();
        MonsterSprite.sprite = monsterSprite;
        MonsterName.text = monster.MonsterName;
        MonsterHP.text = monster.HP.ToString();
        MonsterAtk.text = monster.Atk.ToString();
        MonsterDef.text = monster.Def.ToString();
    }

    public void GetMonster(Monster monster) {
        if (monster.HP < 0) {
            monster.HP = 0;
        }
        MonsterHP.text = monster.HP.ToString();
    }
    public void GetHero(Player hero) {
        HeroHP.text = hero.HP.ToString();
    }

    public void EndBattle() {
        BattlePanle.SetActive(false);
        UIManager.instance.CurrentState = UIState.Default;
    }
}
