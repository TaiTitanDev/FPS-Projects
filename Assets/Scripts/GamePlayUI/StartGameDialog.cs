using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameDialog : MonoBehaviour
{
    private readonly string ANIM_SHOW = "Show";
    private readonly string ANIM_HIDE = "Hide";

    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        EventSystem.Instance.Subscribe(SystemEventType.SystemEventShowDialogGetStart, OnShowStartGameDialog);
    }

    private void OnDisable()
    {
        EventSystem.Instance.Unsubscribe(SystemEventType.SystemEventShowDialogGetStart, OnShowStartGameDialog);
    }

    private void OnShowStartGameDialog(Message message)
    {
        animator.Play(ANIM_SHOW);
        Invoke(nameof(OnClose), 3.0f);
    }

    private void OnClose()
    {
        animator.Play(ANIM_HIDE);
    }
}
