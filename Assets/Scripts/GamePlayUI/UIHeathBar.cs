using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeathBar : MonoBehaviour
{
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private TMP_Text textHealth;

    private void Start()
    {
        sliderHealth.maxValue = (float)PlayerDataService.Instance.GetPlayerModel().MaxHealth;
        OnUpdateHealth();
    }
    private void OnEnable()
    {
        EventSystem.Instance.Subscribe(SystemEventType.SystemEventUpdateHealth, OnUpdateHealth);
    }

    private void OnDisable()
    {
        EventSystem.Instance.Unsubscribe(SystemEventType.SystemEventUpdateHealth, OnUpdateHealth);
    }

    private void OnUpdateHealth(Message message)
    {
        OnUpdateHealth();
    }
    private void OnUpdateHealth()
    {
        var healthValue = string.Format("{0}/{1}", (float)PlayerDataService.Instance.GetPlayerModel().Health, (float)PlayerDataService.Instance.GetPlayerModel().MaxHealth);
        sliderHealth.value = (float) PlayerDataService.Instance.GetPlayerModel().Health;
        textHealth.text = healthValue;
    }
}
