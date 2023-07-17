using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardText : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMesh nameTextMesh;
    private CollectableItem itemRef;
    private GameObject playerRef;

    private void Start()
    {
        itemRef = GetComponentInParent<CollectableItem>();
        playerRef = FindObjectOfType<Player>().gameObject;
        if (itemRef != null)
        {
            nameTextMesh.text = itemRef.item.itemName;

            if (Vector3.Distance(playerRef.transform.position, this.transform.position) < 2f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
