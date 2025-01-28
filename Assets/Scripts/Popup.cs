using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    [Header("Status")] 
    [SerializeField] private bool isShowing = false;
    [SerializeField] private List<Button> buttons = new();
    
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform popupParent;
    [SerializeField] private Transform optionParent;
    [SerializeField] private Image image;

    private void Awake()
    {
        Instance = this;
        popupParent.gameObject.SetActive(false);
    }

    public void ShowPopup(List<PopupOption> options, Sprite sprite = null)
    {
        foreach (var option in options)
        {
            GameObject buttonObject = Instantiate(optionPrefab, optionParent);
            Button button = buttonObject.GetComponentInChildren<Button>();
            TextMeshProUGUI text = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            button.onClick = option.OnClick;
            text.text = option.Text;
        }

        if (sprite != null)
        {
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }
        else
        {
            image.gameObject.SetActive(false);
        }
        
        popupParent.gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        isShowing = false;
        popupParent.gameObject.SetActive(false);

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
    }
}

public class PopupOption
{
    public Button.ButtonClickedEvent OnClick;
    public string Text;
    
    public PopupOption(Button.ButtonClickedEvent unityEvent, string text)
    {
        Text = text;
        OnClick = unityEvent;
    }
}
