using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter {
    string CharacterName { get; set; }
    string ParentPath { get; }
    string PrefabPath { get; }

    void Awake();
    void OnDestroy();

    void StoreData();
    void LoadData();
    void AddData();

    void AddEvent();
    void DeleteEvent();
}
