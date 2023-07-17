using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour
{
    public ItemSO item;
    public ItemInventory inventory;
    public bool canDrag = true;


    public void OnDropItem(ItemSO item)
    {
       inventory.AddItem(item);
      
    }
    public void OnDragItem(ItemSO item)
    {
        inventory.RemoveItem(item);
    }
}
