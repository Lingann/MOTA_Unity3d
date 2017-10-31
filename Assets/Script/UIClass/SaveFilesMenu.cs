using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFilesMenu : MonoBehaviour {
    public GameObject SaveFileParentPrefab;

    [HideInInspector] public GameObject SaveFileItemParent;

    public GameObject SaveFileItemPrefab;

    public GameObject NullFileItemPrefab;

    [HideInInspector] public int FilesNumber = 6;


    public void AddSaveFile(GameObject nullFile) {

        int FileId = nullFile.GetComponent<NullFileItem>().FileID;
        Debug.Log("当前文件索引"+FileId);
        nullFile.SetActive(false);
        GameObject go = Instantiate(SaveFileItemPrefab, SaveFileItemParent.transform);
        go.transform.SetSiblingIndex(nullFile.transform.GetSiblingIndex());

        SaveFileItem saveFileItem = go.GetComponent<SaveFileItem>();
        saveFileItem.FileID = FileId;
        saveFileItem.CurrentLayer.text = UIManager.instance.playerMenu.CurrentLayer.text;
        saveFileItem.GetNewData(FileId);
    }

    public void OverrideSaveFile(SaveFileItem saveFile) {
        saveFile.GetNewData(saveFile.FileID);
    }

    public void OpenSaveMenu() {
        SaveFileItemParent = Instantiate(SaveFileParentPrefab, GetComponent<MainMenu>().MainMenuPanel.transform);
        for (int i = 1; i <= FilesNumber; i++) {
            if (ArchiveFile.FileIsCanFound(ArchiveFile.GetSaveFilePath(i))) {
                GameObject go = Instantiate(SaveFileItemPrefab, SaveFileItemParent.transform);
                SaveFileItem saveFileItem = go.GetComponent<SaveFileItem>();
                saveFileItem.FileID = i;
                saveFileItem.GetFileData(i);
                saveFileItem.AddSaveEvent();
            } else {
                GameObject go = Instantiate(NullFileItemPrefab, SaveFileItemParent.transform);
                NullFileItem nullFileItem = go.GetComponent<NullFileItem>();
                nullFileItem.FileID = i;
                nullFileItem.GetFileData(i);
                nullFileItem.AddSaveEvent();
            }
        }
    }

    public void OpenLoadMenu() {
        SaveFileItemParent = Instantiate(SaveFileParentPrefab, GetComponent<MainMenu>().MainMenuPanel.transform);
        for (int i = 1; i <= FilesNumber; i++) {
            if (ArchiveFile.FileIsCanFound(ArchiveFile.GetSaveFilePath(i))) {
                GameObject go = Instantiate(SaveFileItemPrefab, SaveFileItemParent.transform);
                SaveFileItem saveFileItem = go.GetComponent<SaveFileItem>();
                saveFileItem.FileID = i;
                saveFileItem.GetFileData(i);
                saveFileItem.AddLoadEvent();
            } else {
                GameObject go = Instantiate(NullFileItemPrefab, SaveFileItemParent.transform);
                NullFileItem nullFileItem = go.GetComponent<NullFileItem>();
                nullFileItem.FileID = i;
                nullFileItem.GetFileData(i);
                nullFileItem.AddLoadEvent();
            }
        }

    }

    public void CloseSaveFilesMenu() {
        Destroy(SaveFileItemParent);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
