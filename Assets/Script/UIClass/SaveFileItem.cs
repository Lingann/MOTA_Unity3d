using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileItem : MonoBehaviour {
    public Text FileIndex;
    public Text CurrentLayer;
    public Text GameTime;

    [HideInInspector] public int FileID;

    DataContainer FileData = new DataContainer();
    Button btn;

    private void Start() {
        btn = GetComponent<Button>();

    }

    public void GetFileData (int fileId) {
        string FilePath = ArchiveFile.GetSaveFilePath(fileId);
        FileData = DataController.LoadArchive(FilePath);
        CurrentLayer.text = FileData.Map[0].CurrentLayerIndex.ToString();
        FileIndex.text = "文件" + fileId;
    }

    public void GetNewData(int fileId) {
        FileIndex.text = "文件" + fileId;
    }

    public void AddSaveEvent() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate {
            GameManager.instance.Save(FileID);
        });
    }

    public void AddLoadEvent() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate {
            GameManager.instance.Load(FileID);
        });
    }

    
	
}
