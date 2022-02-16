using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestion : MonoBehaviour
{
    [SerializeField] TMP_Text questionTitle;
    [SerializeField] GameObject checkMark;

    public void SetQuestionTitle(string text)
    {
        questionTitle.text = text;
    }
    public void SetCheckMark(bool isComplete)
    {
        checkMark.SetActive(isComplete);
    }

}
