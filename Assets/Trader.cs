using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    private GameObject playerRef;
    [SerializeField] private GameObject textMeshObject;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject playerInventoryUI;
    private bool dialogueIsAvailable;
    private bool isInDialogue;
    public ItemInventory traderInventory;
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false);
        playerRef = Player.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDialogue && Input.GetKeyDown(KeyCode.Escape))
        {
            isInDialogue = false;
            textMeshObject.SetActive(true);
            dialoguePanel.SetActive(false);
            playerInventoryUI.SetActive(false);
            playerRef.GetComponent<FirstPersonController>().canMove = true;
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            traderInventory.MoveToInventory(Player.instance.playerInventory);
        }

        if (Vector3.Distance(playerRef.transform.position, transform.position) <= interactionDistance)
            dialogueIsAvailable = true;
        else
            dialogueIsAvailable = false;


        if (dialogueIsAvailable)
        {
            textMeshObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                dialoguePanel.SetActive(true);
                playerInventoryUI.SetActive(true);
                playerRef.GetComponent<FirstPersonController>().canMove = false;
                //Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                isInDialogue = true;
            }

        }
        else
        {
            textMeshObject.SetActive(false);
        }

    }
}
