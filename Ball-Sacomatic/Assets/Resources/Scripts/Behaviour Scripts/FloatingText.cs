using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public Animator animator;

    private void Start()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0); //Gets the current clips from the animator
        Destroy(gameObject, clipInfo[0].clip.length); //Destroy after length of time has passed
    }

    //Update the text of the points
    public void SetText(string text)
    {
        animator.GetComponent<Text>().text = text;
    }

    //What is wrong with this??
    public void SetColor(Color color)
    {
        animator.GetComponent<Text>().color = color;
    }

    //What is wrong with this??
    public void SetSize(int size)
    {
        animator.GetComponent<Text>().fontSize = size;
    }
}