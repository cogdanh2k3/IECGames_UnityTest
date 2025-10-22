using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMain : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnPlay;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnPlay.onClick.AddListener(OnClickPlay);
    }

    private void OnDestroy()
    {
        if (btnPlay) btnPlay.onClick.RemoveAllListeners();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    private void OnClickPlay()
    {
        m_mngr.LoadLevel();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
