using DG.Tweening;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float widthMultiplier = 1.2f;
    
    [Header("References")]
    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private RectTransform _healthBarCurrent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        PlayerScript.Instance.HealthObject.OnHeal.AddListener(UpdateHealthBar);
        PlayerScript.Instance.HealthObject.OnHit.AddListener(UpdateHealthBar);
        PlayerScript.Instance.HealthObject.OnDeath.AddListener(UpdateHealthBar);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        UpdateHealthBarValue(PlayerScript.Instance.HealthObject.current, PlayerScript.Instance.HealthObject.maxHealth);
    }

    public void UpdateHealthBarValue(float currentHealth, float maxHealth)
    {
        _healthBarCurrent.DOSizeDelta(new Vector2(currentHealth * widthMultiplier,_healthBarCurrent.sizeDelta.y),1);
        _healthBar.DOSizeDelta(new Vector2(maxHealth * widthMultiplier,_healthBar.sizeDelta.y),1);
        animator.SetTrigger("HealthChanged");
    }
}
