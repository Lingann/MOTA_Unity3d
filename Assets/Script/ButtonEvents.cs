using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Linq;

public enum ButtonAction {
    None,
    Jump,
    Purchase,
    Obtain,
    Game,
    UI,
    GameObject
}

public class ButtonEvents : MonoBehaviour {
    private GameObject ControllerObject;
    public Player player;
    public ButtonAction CurrentAction;
    private string action;
    private string item;
    private int jump;
    private string to;
    private string obtain;
    private string cost;
    private int price;
    private int value;
    public XmlNode OptionNode;
    Text ButtonText;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();

	}

    public ButtonAction CheckOption( XmlNode _OptionNode) {
        OptionNode = _OptionNode;
        if (OptionNode.Attributes["action"] != null)
            action = OptionNode.Attributes["action"].Value;

        ButtonText = GetComponentInChildren<Text>();
        item = OptionNode.SelectSingleNode("item").InnerXml;
        ButtonText.text = item;
        switch (action) {
            case "jump":
                XmlNode Jump = OptionNode.SelectSingleNode("jump");
                jump = int.Parse(Jump.InnerXml);
                to = Jump.Attributes["to"].Value;
                return CurrentAction = ButtonAction.Jump;
            case "purchase":
                XmlNode Price = OptionNode.SelectSingleNode("price");
                XmlNode Value = OptionNode.SelectSingleNode("value");
                cost = Price.Attributes["cost"].Value;
                obtain = Value.Attributes["obtain"].Value;
                price = int.Parse(Price.InnerXml);
                value = int.Parse(Value.InnerXml);
                return CurrentAction = ButtonAction.Purchase;
            case "obtain":
                XmlNode obtainValue = OptionNode.SelectSingleNode("value");
                obtain = obtainValue.Attributes["obtain"].Value;
                value = int.Parse(obtainValue.InnerXml);
                return CurrentAction = ButtonAction.Obtain;
        }
        return CurrentAction = ButtonAction.None;
    }

    public void Execute() {
        switch (CurrentAction) {
            case ButtonAction.Jump:
                UIManager.instance.CurrentState = UIState.Default;
                JumpDialogue();
                break;
            case ButtonAction.Purchase:
                Extrabuy();
                UIManager.instance.CurrentState = UIState.Default;
                break;
            case ButtonAction.Obtain:
                Obtain();
                UIManager.instance.CurrentState = UIState.Default;
                break;
        }
        UIManager.instance.dialogue.SelectPanel.SetActive(false);
    }

    private void JumpDialogue() {
        UIManager.instance.dialogue.StartDialogue(to, jump);
    }

    private void Extrabuy() {
        if (player.IsEnough(cost, price)) {
            player.ChangeProperty(obtain, value);
            player.ChangeProperty(cost, (-price));
            Debug.Log("购买成功，" + obtain + "增加：" + value + "。" + cost + "减少:" + price);
        } else {
            UIManager.instance.remainder.GetProps("金币不足，购买失败");
        }

    }

    private void Obtain() {
        player.ChangeProperty(obtain, value);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
