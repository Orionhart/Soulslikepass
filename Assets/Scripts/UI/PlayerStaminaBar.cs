using System;
using DG.Tweening;
using UnityEngine;

public class PlayerStaminaBar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float widthMultiplier = 1.2f;
    
    [Header("References")]
    [SerializeField] private RectTransform _staminaBar;
    [SerializeField] private RectTransform _staminaBarCurrent;

    private void Start()
    {
        PlayerScript.Instance.StaminaObject.OnStaminaChanged.AddListener(UpdateStaminaBar);
        UpdateStaminaBar(PlayerScript.Instance.StaminaObject.current, PlayerScript.Instance.StaminaObject.maxStamina);
    }

    public void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        _staminaBarCurrent.DOSizeDelta(new Vector2(currentStamina * widthMultiplier,_staminaBarCurrent.sizeDelta.y),1);
        _staminaBar.DOSizeDelta(new Vector2(maxStamina * widthMultiplier,_staminaBar.sizeDelta.y),1);
    }
}
