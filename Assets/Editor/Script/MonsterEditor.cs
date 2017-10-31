using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterEditor : EditorWindow {

    [MenuItem("Window/MonsterEditor")]
    static void Init(){
        EditorWindow.GetWindow(typeof(MonsterEditor));
    }

    private void OnGUI() {
        GUILayout.Label("Monster Editor", EditorStyles.boldLabel);
    }
}
