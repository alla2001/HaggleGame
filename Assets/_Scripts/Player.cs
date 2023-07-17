using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int maxCarryWeight = 40;
    public int currentWeight;
    public GameObject inventoryUI;
    public static Player instance;
    public ItemInventory playerInventory;
    public TextMeshProUGUI scoreText;
   
    public int score;
  


    public void AddScore(int value)
    {
        score += value; 

        scoreText.text = "Score : " + score.ToString();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
      

    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI.SetActive(false);
        currentWeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryUI!=null && Input.GetKeyDown(KeyCode.Tab) )
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                GetComponent<FirstPersonController>().canMove= false;
            }
            else
            {
                GetComponent<FirstPersonController>().canMove = true ;
            }
        }
    }


}
