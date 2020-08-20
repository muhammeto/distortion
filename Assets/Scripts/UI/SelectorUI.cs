using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public enum SelectorType
{
    ButtonSelector,
    SliderSelector
}
public enum SelectorDirection
{
    Vertical,
    Horizontal,
    HorizontalVertical
}
public class SelectorUI : MonoBehaviour
{
    [SerializeField] private List<RectTransform> selectable = null;
    [SerializeField] private Button selfExitButton = null;
    [SerializeField] private SelectorType type = SelectorType.ButtonSelector;
    [SerializeField] private SelectorDirection direction = SelectorDirection.Vertical;
    [SerializeField] private int offset = 0;
    [SerializeField] private KeyCode seltexitInvokeKey = KeyCode.Escape;
    [SerializeField] private KeyCode buttonInvokeKey1 = KeyCode.Return;
    [SerializeField] private KeyCode buttonInvokeKey2 = KeyCode.Space;



    private int currentSelected = 0;
    private RectTransform selector;
    private bool canSelect = true;

    private void OnEnable()
    {
        selector = GetComponent<RectTransform>();
        currentSelected = 0;
        selector.anchoredPosition = selectable[currentSelected].anchoredPosition;
        canSelect = true;
    }

    private void Select(int increment)
    {
        canSelect = false;
        SoundManager.Instance.Select();
        currentSelected += increment;
        currentSelected += selectable.Count;
        currentSelected %= selectable.Count;
        selector.DOAnchorPos(selectable[currentSelected].anchoredPosition, 0.2f).OnComplete(()=>canSelect=true);
    }

    public void Select(RectTransform selectThis)
    {
        if (selectable.Contains(selectThis))
        {
            canSelect = false;
            SoundManager.Instance.Select();
            currentSelected = selectable.IndexOf(selectThis);
            selector.DOAnchorPos(selectable[currentSelected].anchoredPosition, 0.05f).OnComplete(() => canSelect = true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(seltexitInvokeKey))
        {
            selfExitButton.onClick.Invoke();
        }
        if (type == SelectorType.ButtonSelector && (Input.GetKeyDown(buttonInvokeKey1) || Input.GetKeyDown(buttonInvokeKey2)))
        {
            selectable[currentSelected].GetComponent<Button>().onClick.Invoke();
        }else if (type == SelectorType.SliderSelector && (Input.GetAxisRaw("Horizontal") != 0))
        {
            selectable[currentSelected].GetComponentInChildren<Slider>().value += (Input.GetAxisRaw("Horizontal") * Time.deltaTime);
        }

        if (!canSelect) return;
        if (direction == SelectorDirection.Vertical)
        {
            if((int)Input.GetAxisRaw("Vertical")!=0) Select((int)-Input.GetAxisRaw("Vertical"));
        }
        else if(direction == SelectorDirection.Horizontal)
        {
            if ((int)Input.GetAxisRaw("Horizontal") != 0) Select((int)Input.GetAxisRaw("Horizontal"));
        }
        else if(direction == SelectorDirection.HorizontalVertical && type==SelectorType.ButtonSelector)
        {
            if ((int)Input.GetAxisRaw("Horizontal") != 0) Select((int)Input.GetAxisRaw("Horizontal"));
            if ((int)Input.GetAxisRaw("Vertical") != 0) Select((int)-Input.GetAxisRaw("Vertical")*offset);
        }
    }
}
