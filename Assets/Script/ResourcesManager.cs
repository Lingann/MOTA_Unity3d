using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using LitJson;

public class ResourcesManager : MonoBehaviour{

    // 声明一个静态GameManager 实例 以便其它脚本可以进行访问
    public static ResourcesManager instance = null;

    public static string SpriteBasePath = "PropsSprite/";

    public static Dictionary<string,Sprite> SpriteContainer = new Dictionary<string, Sprite>();

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
        DontDestroyOnLoad(gameObject);

        AddSprite("Item01_01");
        AddSprite("Item01_02");
        AddSprite("Item01_03");
        AddSprite("Item01_04");
        AddSprite("Item01_05");
        AddSprite("Item01_06");
    }


    void AddSprite(string atlasName) {
        Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteBasePath + atlasName);

        foreach (Sprite sprite in sprites) {
            if(!SpriteContainer.ContainsValue(sprite))
                SpriteContainer.Add(sprite.name,sprite);
        }

    }

    public static Sprite GetSprite(string spriteName) {
        Debug.Log(spriteName);
        if (spriteName == "")
            return null;
        return SpriteContainer[spriteName];
    }





    /// <summary>
    /// 读取Xml资源
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static XmlDocument GetXmlData(string fileName) {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset xml = Resources.Load<TextAsset>(fileName) as TextAsset;
        xmlDoc.LoadXml(xml.text);
        if (xml == null) {
            Debug.Log("Xml文件加载失败，文件名错误或不存在名为" + fileName + "的文件");
        }
        return xmlDoc;
    }


    public static XmlNodeList GetDialogue(string name) {
        // 获取Xml文档
        XmlDocument xmlDoc = GetXmlData("Dialogue");
        // 获取指定dialogue节点
        XmlNode dialogue = xmlDoc.DocumentElement.SelectSingleNode("dialogue[@name = '" + name + "']");
        // 获取指定dialogue节点的所有<sentence>
        XmlNodeList sentenceList = dialogue.SelectNodes("sentence");
        return sentenceList;
    }

    /// <summary>
    ///  获取json数据
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="arrIndex">数组索引</param>
    /// <returns>Json Date 用于保存json数据</returns>
    public static JsonData GetJsonData(string fileName, string arrIndex) {
        TextAsset MonsterJson = Resources.Load<TextAsset>(fileName) as TextAsset;
        string jsonString = MonsterJson.text;
        JsonData rootData = JsonMapper.ToObject(jsonString);
        JsonData jsonData = rootData[arrIndex];
        if (jsonData == null) {
            Debug.Log("json文件加载失败，文件名错误或不存在名为" + fileName + "的文件");
            return jsonData;
        }
        return jsonData;
    }

}
