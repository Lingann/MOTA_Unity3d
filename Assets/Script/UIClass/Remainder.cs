using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remainder : MonoBehaviour {
    public GameObject RemainderPanle;
    public Text RamainderText;

    public void GetProps(string reminder) {

        Animator GetPropsAnim = RemainderPanle.GetComponent<Animator>();
        RemainderPanle.SetActive(true);
        RemainderPanle.GetComponentInChildren<Text>().text = reminder;
        GetPropsAnim.SetTrigger("IsActive");
        UIManager.instance.CurrentState = UIState.GetProps;
        StartCoroutine(PropsReminder(GetPropsAnim));
    }
    private IEnumerator PropsReminder(Animator Anim) {
        yield return null;
        // 第一次进入的时候不知道什么鬼，DoorAnim.GetCurrentAnimatorStateInfo(0).IsName("yellowDoorOpen")  = true.
        // 这可能是该方法在Unity内部的执行顺序问题。可能是开启动画后， 在第一帧并没有被启动，同时该函数的值并不是依靠动画参数来决定， 所以就导致该函数的值为false
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Opening")) {
            StartCoroutine(PropsReminder(Anim));
        } else {
            RectTransform rectTransform = RemainderPanle.GetComponent<RectTransform>() as RectTransform;
            RectTransform textRectTransform = RamainderText.GetComponent<RectTransform>() as RectTransform;
            textRectTransform.sizeDelta.Set(0, 40);
            rectTransform.sizeDelta.Set(0, 60);
            // 销毁对象需要一个过程，这个过程使得该协程继续执行了次
            RemainderPanle.SetActive(false);
            UIManager.instance.CurrentState = UIState.Default;
        }
        // 下面语句会在 索引A 后输出两次
        //Debug.Log("索引B"); 
    }
}
