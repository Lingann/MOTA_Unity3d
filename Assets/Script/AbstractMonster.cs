using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMonster : MonoBehaviour {
    public abstract int HP { get; set; }
    public abstract int Atk { get; set; }
    public abstract int Def { get; set; }
    public abstract int Gold { get; set; }
    public abstract int Exp { get; set; }
    public abstract void Damage(int otherAtk);
    public abstract IEnumerator Attack();
    public abstract int GetDemageValue(int otherAtk, int otherDef);
}
