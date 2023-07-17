using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ItemDetection : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float maxDistanceToTarget;

    CollectableItem hilightedItem;
    private AudioSource audioSource;

    private Player playerRef;

    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        playerRef = GetComponentInParent<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
       

        if(Physics.Raycast(transform.position,transform.forward, out hit, maxDistanceToTarget, itemLayer) )
        {
            CollectableItem item = hit.collider.gameObject.GetComponent<CollectableItem>();
            if (hilightedItem != null)
            {
                hilightedItem.UnHilight();
                hilightedItem = null;
            }
            hilightedItem = item;
            hilightedItem.Hilight();
            if (!Input.GetKeyDown(KeyCode.E))
                return;
            if (playerRef.playerInventory.AddItem(item.item)) {
                
                PlayPickupSound();
                Destroy(item.gameObject);
                Debug.Log("Collected Item:" + item.item.itemName);
                if (hilightedItem != null)
                {
                    hilightedItem.UnHilight();
                    hilightedItem = null;
                }
                
            }
        }
        else
        {
            if (hilightedItem != null)
            {
                hilightedItem.UnHilight();
                hilightedItem = null;
            }
        }
       
    }

    void PlayPickupSound()
    {
        audioSource.PlayOneShot(pickupSound, audioSource.volume);
    }
}
