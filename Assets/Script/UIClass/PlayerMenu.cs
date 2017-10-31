using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour {
    public Player player;
    public Text CurrentLayer;
    public Text HeroLevels;
    public Text HeroHP;
    public Text HeroAtk;
    public Text HeroDef;
    public Text YellowKeys;
    public Text BlueKeys;
    public Text RedKeys;
    public Text Golds;

    private void OnGUI() {
        CurrentLayer.text = LayerManager.instance.currentLayer.GetComponent<Layer>().CurrentLayer.ToString();
        HeroLevels.text = player.Levels.ToString();
        HeroHP.text = player.HP.ToString();
        HeroAtk.text = player.ATK.ToString();
        HeroDef.text = player.DEF.ToString();
        YellowKeys.text = player.YellowKeys.ToString();
        BlueKeys.text = player.BlueKeys.ToString();
        RedKeys.text = player.RedKeys.ToString();
        Golds.text = player.Golds.ToString();
    }
}
