using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ItemInventory : MonoBehaviour
{
    
    public List<ItemSO> items = new List<ItemSO>();
    public UnityEvent onItemCollected = new UnityEvent();
    public List<ItemSlot> slots = new List<ItemSlot>();
    public GameObject itemImage;
    public int maxSize = 4;

    private void OnEnable()
    {
        Display();
    }
    public void Display()
    {
        int i = 0;
        foreach (ItemSlot slot in slots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
          
        }
        foreach (var item in items)
        {

            if (slots[i].transform.childCount> 0)
            {
                Destroy(slots[i].transform.GetChild(0));
            }
            GameObject temp = Instantiate(itemImage);
            temp.GetComponent<DragHandler>().parentSlot = slots[i];
            temp.GetComponent<DragHandler>().item = item;
            temp.transform.SetParent(slots[i].transform);
            temp.transform.localPosition = Vector3.zero;
            temp.GetComponent<Image>().sprite = item.icon;
            i++;
            
        }
    }
    private void Awake()
    {
        DragHandler.RefreshUI.AddListener(() => { Display(); });
    }

    public int GetCountOfItem(ItemSO _item)
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.name == _item.name)
            {
                count ++;
            }
        }
        return count;
    }

    public bool AddItem(ItemSO item)
    {
        if(items.Count< maxSize)
        {
            items.Add(item);
            onItemCollected?.Invoke();
            Display();
            return true;
        }
        Display();
        return false;
      
    }
    public void RemoveItemByName(string name)
    {
        foreach (var item in items)
        {
            if (item.itemName == name)
            {
                RemoveItem(item);
                return;
            }
        }
        Display();
    }
    public void RemoveItem(ItemSO item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
        Display();
    }

    public void MoveToInventory(ItemInventory targetInventory)
    {
        foreach (var item in items)
        {
            targetInventory.AddItem(item);
        }
        items.Clear();
        Display();
    }
}
