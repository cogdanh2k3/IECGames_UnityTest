using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public event EventHandler OnWin;
    public event EventHandler OnLose;

    public event Action OnMoveEvent = delegate { };

    public bool IsBusy { get; private set; }

    private Board m_board;

    private GameManager m_gameManager;

    private bool m_isDragging;

    private Camera m_cam;

    private Collider2D m_hitCollider;

    private GameSettings m_gameSettings;

    private List<Cell> m_potentialMatch;

    private float m_timeAfterFill;

    private bool m_hintIsShown;

    private bool m_gameOver;

    private BottomArea bottomArea;
    

    public void StartGame(GameManager gameManager, GameSettings gameSettings)
    {
        m_gameManager = gameManager;
        m_gameSettings = gameSettings;
        m_gameManager.StateChangedAction += OnGameStateChange;

        m_cam = Camera.main;

        m_board = new Board(this.transform, gameSettings);
        m_board.FillWithDivisibleBy3();

        bottomArea = gameObject.AddComponent<BottomArea>();
        bottomArea.Initialize(this.transform);
        bottomArea.OnBottomFull += BottomArea_OnBottomFull;
        bottomArea.OnMatchCleared += BottomArea_OnMatchCleared;

        
    }

    private void BottomArea_OnMatchCleared(object sender, EventArgs e)
    {
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (m_board.IsEmpty())
        {
            m_gameOver = true;
            OnWin?.Invoke(this,EventArgs.Empty);
            m_gameManager.GameWin();
        }
    }

    private void BottomArea_OnBottomFull(object sender, EventArgs e)
    {
        if (!m_board.IsEmpty())
        {
            m_gameOver = true;
            OnLose?.Invoke(this, EventArgs.Empty);
            m_gameManager.GameOver();
        }
    }

    private void OnGameStateChange(GameManager.eStateGame state)
    {
        switch (state)
        {
            case GameManager.eStateGame.GAME_STARTED:
                IsBusy = false;
                m_gameOver = false;
                break;
            case GameManager.eStateGame.PAUSE:
                IsBusy = true;
                break;
            case GameManager.eStateGame.GAME_OVER:
                m_gameOver = true;
                break;
        }
    }


    public void Update()
    {
        if (m_gameOver) return;
        if (IsBusy) return;


        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if(cell != null && !cell.IsEmpty)
                {
                    //Debug.Log("tapping item");

                    TryMoveItemToBottom(cell);
                }
            }
        }
    }

    private void TryMoveItemToBottom(Cell cell)
    {
        // kiem tra bottom cell da full chua
        if (bottomArea.IsFull())
        {
            return;
        }
        IsBusy = true;

        Item item = cell.Item;
        cell.Free();

        bool success = bottomArea.TryAddItem(item);

        if (success)
        {
            OnMoveEvent();
            StartCoroutine(CompleteMoveCoroutine());
        }
        else
        {
            cell.Assign(item);
            IsBusy = false;
        }
    }

    private IEnumerator CompleteMoveCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        IsBusy = false;
    }

    internal void Clear()
    {
        m_board.Clear();
        bottomArea.Clear();
    }

    // 
}
