using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCell : MonoBehaviour
{
    public int Index { get; private set; }
    public Item Item { get; private set; }

    public bool IsEmpty()
    {
        return Item == null;
    }

    public void Setup(int index)
    {
        Index = index;
    }

    public void AddItem(Item item)
    {
        Item = item;
    }

    public void ExplodeItem()
    {
        if (Item == null) return;

        Item.ExplodeView();
        Item = null;
    }
    public void Clear()
    {
        if (Item != null)
        {
            Item.Clear();
            Item = null;
        }
    }
}
