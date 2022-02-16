using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameDialog : MonoBehaviour
{
    private readonly string ANIM_SHOW = "Show";
    private readonly string ANIM_HIDE = "Hide";

    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textTitle;

    private bool isShow = false;

    private void OnEnable()
    {
        EventSystem.Instance.Subscribe(SystemEventType.SystemEventShowDialogEndGame, OnShowEndGameDialog);
    }

    private void OnDisable()
    {
        EventSystem.Instance.Unsubscribe(SystemEventType.SystemEventShowDialogEndGame, OnShowEndGameDialog);
    }

    private void Update()
    {
        if (!isShow) return;
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnGameRestart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitGame();
        }
    }

    private void OnShowEndGameDialog(Message message)
    {
        isShow = true;

        textTitle.text = (string)message.Data;
        animator.Play(ANIM_SHOW);
    }

    public void OnGameRestart()
    {
        GamePlayController.Instance.OnGameRestart();
    }

    public void OnQuitGame()
    {
        GamePlayController.Instance.OnQuitGame();
    }

    public void OnClose()
    {
        animator.Play(ANIM_HIDE);
    }
}
