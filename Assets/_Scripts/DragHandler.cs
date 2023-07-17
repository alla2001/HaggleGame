using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Transform originalParent;
    private Vector3 originalPosition;
    public ItemSO item;
    public ItemSlot parentSlot;
    public static UnityEvent RefreshUI = new UnityEvent();
    public GameObject hoverText;
    private void OnEnable()
    {
        if(parentSlot )
        parentSlot.item= item;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        transform.SetParent(transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    GameObject tmp;
    public void OnPointerEnter(PointerEventData eventData)
    {
     
        tmp = Instantiate(hoverText);
        tmp.transform.SetParent(transform);
        tmp.transform.localPosition = new Vector3(75, 75);
        tmp.GetComponent<TextMeshProUGUI>().text = item.itemName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tmp != null)
        {
            Destroy(tmp);
            tmp = null;
        }
    }
 
   
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
        // Check if the item is dropped onto a valid inventory slot
        if (eventData.pointerEnter != null)
        {
            ItemSlot inventorySlot = eventData.pointerEnter.GetComponent<ItemSlot>();
            if (inventorySlot != null)
            {
               
                parentSlot.OnDragItem(item);
                if (inventorySlot.item != null)
                {
                    parentSlot.OnDropItem(inventorySlot.item);
                    inventorySlot.OnDragItem(inventorySlot.item);
                }
                transform.SetParent(inventorySlot.transform);
                transform.localPosition = Vector3.zero ;
                parentSlot = inventorySlot;
                parentSlot.OnDropItem(item);
                RefreshUI?.Invoke();
                return;
            }
        }
     
        // If not dropped onto a valid inventory slot, reset the position
        transform.position = originalPosition;
      
    }

   
}