using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Career {
    普通,
    法师,
    剑客
}

public class _Monster : MonoBehaviour {
    public string MonsterName;
    public Career careenr;
    public int HP;
    public int ATK;
    public int DEF;
    public int Golds;
    public int EXP;
    public string Introduction;
}
