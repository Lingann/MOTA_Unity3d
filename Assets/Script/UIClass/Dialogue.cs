using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour {


    // 获取对话面板
    public GameObject DialoguePanel;

    // 设置一个文本框，从编辑器中将相应的文本框拖住到当前对应的nameText,用于显示对话人物
    public Text dialogueName;
    
    // 设置一个文本框，从编辑器中将相应的文本框拖住到当前对应的dialogueText，用于显示对话内容
    public Text dialogueText;

    // 获取选择面板
    public GameObject SelectPanel;
  
    // 获取选项
    public Button[] options;

    // 新建一个队列，用于索引将要显示的对话
    private Queue<XmlNode> sentences;

    // 获取xml所有对话
    private XmlNodeList sentenceList;
    
    // 设置以下xml节点引用，避免每次都创建新的引用
    private XmlDocument xmlDoc;
    private XmlNode root;
    private XmlNode dialogues;
    private XmlNode dialogue;
    private XmlNode sentence;

    /// <summary>
    /// 开启对话
    /// </summary>
    /// <param name="name"></param>
    public void StartDialogue(string name) {
        UIManager.instance.CurrentState = UIState.Dialogue;
        // 获取队列
        sentences = new Queue<XmlNode>();
        // 获取Xml文档
        xmlDoc = ResourcesManager.GetXmlData("Dialogue");
        //获取XML文档根节点
        root = xmlDoc.DocumentElement;
        // 获取指定dialogue节点
        dialogue = root.SelectSingleNode("dialogue[@name = '" + name + "']");
        // 获取指定dialogue节点的所有<sentence>
        sentenceList = dialogue.SelectNodes("sentence");
        // 清除队列，清除上次对话内容
        sentences.Clear();
        //遍历对话内容，并添加到对话队列中
        foreach (XmlNode sentence in sentenceList) {
            // 将对话列表添加到队列
            sentences.Enqueue(sentence);
        }
        if (sentences.Count == 0)
            Debug.Log("没有任何对话加入队列，请确保游戏角色的名称与Xml名称相同");

        // 获取第一句话
        CheckNextSentence();
    }



    /// <summary>
    /// 开启对话
    /// </summary>
    /// <param name="name"></param>
    public void StartDialogue(string name, int index) {
        if(index == 0) {
            StartDialogue(name);
            return;
        }
        // 获取队列
        sentences = new Queue<XmlNode>();
        // 获取Xml文档
        xmlDoc = ResourcesManager.GetXmlData("Dialogue");
        //获取XML文档根节点
        root = xmlDoc.DocumentElement;
        // 获取包含指定dialogue 父节点
        dialogues = root.SelectSingleNode("dialogue[@name = '" + name + "']");
        // 获取指定索引的dialogue节点
        dialogue = dialogues.SelectSingleNode("subDialogue[@index = '" + index + "']");
        // 获取指定dialogue节点的所有<sentence>
        sentenceList = dialogue.SelectNodes("sentence");
        // 清除队列，清除上次对话内容
        sentences.Clear();
        //遍历对话内容，并添加到对话队列中
        foreach (XmlNode sentence in sentenceList) {
            // 将对话列表添加到队列
            sentences.Enqueue(sentence);
        }
        if (sentences.Count == 0)
            Debug.Log("没有任何对话加入队列，请确保游戏角色的名称与Xml名称和索引在范围内");

        // 获取第一句话
        CheckNextSentence();
    }

    /// <summary>
    /// 检查下一句话的属性
    /// </summary>
    public void CheckNextSentence() {
        // 如果对话队列没有对话内容了，则调用对话
        if (sentences.Count == 0) {
            // 当对话结束时调用
            EndDialogue();
            return;
        }
        // 取出一条对话信息
        sentence = sentences.Dequeue();
        
        // 获取当前对话内容的所有选项节点
        XmlNodeList optionNodes = sentence.SelectNodes("option");

        // 如果当前对话内容内容选项节点数目为0，则激活对话面板并现实下一条对话内容，否则激活选择面板
        if (optionNodes.Count == 0) {
            // 开启对话面板
            DialoguePanel.SetActive(true);
            // 显示下一句话
            DisplayNextSentence(sentence);
        } else {
            // 关闭对话面板
            DialoguePanel.SetActive(false);
            // 开启选择面板
            SelectPanel.SetActive(true);
            // 循环遍历选项节点
            for (int i = 0; i < optionNodes.Count; i++) {
                options[i].gameObject.SetActive(true);
                options[i].onClick.RemoveAllListeners();
                options[i].gameObject.name = "Option" + i;
                ButtonEvents buttonEvent = options[i].GetComponent<ButtonEvents>();
                buttonEvent.CurrentAction = buttonEvent.CheckOption(optionNodes[i]);
                options[i].onClick.AddListener(buttonEvent.Execute);
            }
        }
    }

    /// <summary>
    /// 获取对话内容
    /// </summary>
    public void DisplayNextSentence(XmlNode sentence) {
        // 获取对话人物名字
        XmlNode role = sentence.SelectSingleNode("role");
        // 获取对话内容
        XmlNode detail = sentence.SelectSingleNode("detail");
        // 显示对话人物名字
        dialogueName.text = role.InnerXml;
        // 调用对话内容逐字显示方法
        StopAllCoroutines();
        // 开始一个协程用于逐字显示对话内容
        StartCoroutine(TypeSentence(detail.InnerXml));
    }

    /// <summary>
    /// 逐字显示方法
    /// </summary>
    /// <param name="_detail">对话内容</param>
    /// <returns></returns>
    IEnumerator TypeSentence(string _detail) {
        dialogueText.text = "";
        foreach (char letter in _detail.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// 结束对话调用
    /// </summary>
    public void EndDialogue() {
        DialoguePanel.SetActive(false);
        SelectPanel.SetActive(false);
        UIManager.instance.CurrentState = UIState.Default;
    }


}
