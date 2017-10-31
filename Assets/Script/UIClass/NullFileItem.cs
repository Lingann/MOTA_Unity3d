using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NullFileItem : MonoBehaviour {
    public GameObject nullFileItem;
    public Text FileIndex;

    [HideInInspector] public int FileID;

    Button btn;

    void Start() {
        btn = GetComponent<Button>();
    }

    public void GetFileData(int fileId) {
        FileIndex.text = "文件" + fileId;
    }

    public void AddSaveEvent() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate {
            UIManager.instance.saveFileMenu.AddSaveFile(nullFileItem);
            GameManager.instance.Save(FileID);
        });
    }

    public void AddLoadEvent() {
        btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate {
            GameManager.instance.Load(FileID);
        });
    }
}
