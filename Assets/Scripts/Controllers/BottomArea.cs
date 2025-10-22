using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomArea : MonoBehaviour
{
    public event EventHandler OnBottomFull;
    public event EventHandler OnMatchCleared;

    private const int BOTTOM_CELL_COUNT = 5;
    private const int MATCH_COUNT = 3;

    private List<BottomCell> bottomCells;
    private Transform m_root;

    public bool IsFull()
    {
        foreach(var cell in bottomCells)
        {
            if (cell.IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public int EmptySlots()
    {
        int count = 0;
        foreach(var cell in bottomCells)
        {
            if (cell.IsEmpty())
            {
                count++;
            }
        }
        return count;
    }

    public void Initialize(Transform root)
    {
        m_root = root;
        bottomCells = new List<BottomCell>();
        CreateBottomCells();
    }

    private void CreateBottomCells()
    {
        Vector3 startPos = new Vector3(-2f, -4f, 0f);
        GameObject prefabBG = Resources.Load<GameObject>(Constants.PREFAB_CELL_BACKGROUND);

        for (int i = 0; i < BOTTOM_CELL_COUNT; i++)
        {
            GameObject go = Instantiate(prefabBG);
            go.transform.position = startPos + new Vector3(i, 0f, 0f);
            go.transform.SetParent(m_root);

            BottomCell cell = go.AddComponent<BottomCell>();
            cell.Setup(i);

            bottomCells.Add(cell);
        }
    }

    public bool TryAddItem(Item item)
    {
        // tim cell trong dau tien
        BottomCell emptyCell = null;
        foreach(var cell in bottomCells)
        {
            if (cell.IsEmpty())
            {
                emptyCell = cell;
                break;
            }
        }

        if(emptyCell == null)
        {
            return false;
        }

        emptyCell.AddItem(item);
        item.View.DOMove(emptyCell.transform.position, 0.3f).OnComplete(() =>
        {
            CheckForMatches();

            if (IsFull())
            {
                OnBottomFull?.Invoke(this, EventArgs.Empty);
            }
        });

        return true;
    }

    private void CheckForMatches()
    {
        Dictionary<string, List<BottomCell>> groups = new Dictionary<string, List<BottomCell>>();

        foreach(var cell in bottomCells)
        {
            if (cell.IsEmpty()) continue;

            string key = cell.Item.GetType().Name + "_" + GetItemTypeString(cell.Item);

            if (!groups.ContainsKey(key))
                groups[key] = new List<BottomCell>();

            groups[key].Add(cell);
        }

        // neu co group co 3 item giong nhau
        foreach (var group in groups.Values)
        {
            if (group.Count >= MATCH_COUNT)
            {
                var matchGroup = group.GetRange(0, MATCH_COUNT);
                ClearMatches(matchGroup);
                break;
            }
        }
    }

    private string GetItemTypeString(Item item)
    {
        if (item is NormalItem normalItem)
        {
            return normalItem.ItemType.ToString();
        }
        return "Unknown";
    }

    private void ClearMatches(List<BottomCell> matchedCells)
    {
        foreach (var cell in matchedCells)
        {
            cell.ExplodeItem();
        }

        OnMatchCleared?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        foreach (var cell in bottomCells)
        {
            cell.Clear();
            GameObject.Destroy(cell.gameObject);
        }
        bottomCells.Clear();
    }

    public int GetItemCount()
    {
        int count = 0;
        foreach (var cell in bottomCells)
        {
            if (!cell.IsEmpty())
                count++;
        }
        return count;
    }
}
