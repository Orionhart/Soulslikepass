using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    public static CheckpointUI Instance;

    private bool animating = false;
    public bool isOpen = false;

    [SerializeField] private RectTransform canvasParent;
    [SerializeField] private float shownX = 0;
    [SerializeField] private float hiddenX = 0;
    [SerializeField] private float timeToShowHide = 1f;
    [SerializeField] private Button levelUpButton;
    [SerializeField] public Button exitButton;

    private void Awake()
    {
        Instance = this;
        Hide(true);
    }

    private void Start()
    {
        
    }

    public void Show(bool instant = false)
    {
        if (isOpen) return;
        if (!instant)
        {
            animating = true;
            canvasParent.DOLocalMoveX(shownX, timeToShowHide).OnComplete(() =>
            {
                animating = false;
                isOpen = true;
            });
        }
        else
        {
            canvasParent.DOLocalMoveX(shownX, 0);
            isOpen = true;
        }
    }

    public void Hide(bool instant = false)
    {
        if (!isOpen) return;
        if (!instant)
        {
            animating = true;
            canvasParent.DOLocalMoveX(hiddenX, timeToShowHide).OnComplete(() =>
            {
                animating = false;
                isOpen = false;
            });
        }
        else
        {
            canvasParent.DOLocalMoveX(hiddenX, 0);
            isOpen = false;
        }
    }
}
