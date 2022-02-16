using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // Start is called before the first frame update
    [SerializeField] private Camera UICamera;
    [SerializeField] private List<Canvas> canvas;
    void Start()
    {
        OnSetupUI();
    }

    private void OnSetupUI()
    {
        UICamera.fieldOfView = 60.0f;
        foreach (var canva in canvas)
        {
            canva.worldCamera = UICamera;
        }
    }
}
