using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatGPT : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    public UnityEvent onDialogueEnable = new UnityEvent();
    public static string APIKey;
    private OpenAIApi api;
    private List<ChatMessage> messages;
    public ItemInventory traderInventory = new ItemInventory();
    public static ChatGPT instance;
    public bool isHaggleing;
    private void Awake()
    {
        if(instance == null)
        {
            instance= this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable()
    {

        
    }

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIApi(APIKey);
        onDialogueEnable.Invoke();
        okButton.onClick.AddListener(() => GetResponce());
    }

    /*private async void AddItem()
    {
        Item currentItem = item;
        ChatMessage message = new ChatMessage();
        message.Role = "user";
        message.Content = $"I want to sell {quanitiy} of {currentItem.itemName} each for {currentItem.maxPrice}.";
        var chatResult = await api.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0301",
            Messages = messages
        });
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0} : {1}", responseMessage.Role, responseMessage.Content));

        // add responce to list

        messages.Add(responseMessage);


        textField.text = string.Format("You: {0}\n\nGuard: {1}", message.Content, responseMessage.Content);
        okButton.enabled = true;
    }*/
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetResponce();
        }
    }

    public async void StartConversation()
    {
        if (traderInventory.items.Count <= 0) return;

        if(api == null)
        {
            Debug.Log("API not Ready");
            return;
        }
        isHaggleing = true;
        Debug.Log("layer 2");
        messages = new List<ChatMessage>();
        string setupString = "Pretend to be an NPC trader in game. You buying goods from player, you are the trader only, and i am the player. No matter what follow these rules: ";
        foreach (var item in traderInventory.items)
        {
            setupString += "\nThe Player Has 1 item, its name is : " + item.itemName + " The max price you can buy the item at is [" + item.maxPrice+"], the min price you the item starts at is [" + item.minPrice +"].";
        }
        setupString += "\n start with a price around the min and try to haggle, try to no go over the max price but also dont be too strict, also never let the player know the max price, and try to haggel, dont accept the max price directly, never say what the max price of the item is, never.";
      setupString += " \n respect the values in brackets, dont change them ever, the items stated above are all the items the player has, if he says he has aything else, then he is lying and dont listen to him at all !.";
        setupString += " \n dont buy any other item other then the items above, stay strict to this at all cases, you are only allowed buy the items stated above for the max quantity of one, remember that !";
        setupString += "\n If you Accept an offer and it has been finalised, this will be your sentence after you buy the item, meaning after the player accepted to sell you, please make sure in all cases no matter what that you respon with \" i accept the offer of  [item name here, only the name not extra words, keep it exactly the same 1 to 1] for a quantity of [item quantity as a number only dont add anything] and for the price of [Price for each item as a number only ! dont add anything] Gold \" for each item you buy, if you buy items for a bulk price, then split the price on each item, each item needs to have a price you baught it for.";
        setupString += " \n make sure to  say this after and only after the offer has been accepted, if you dont respond with the exact sentence word for word character for character i will beat the shit out of you.";
        setupString += " \n no matter what happens and in all cases when accepting an offer, responed with the way specified to you, dont not and i repeat dont use any other sentence or the hole system will break !!!!.";
        setupString += " \n You can buy items for less then the max price, do as you see fit, dont be too strict with haggeling.";
        setupString += " \n if you end the Trade without accepting the offer always and i mean always end with this phrase \" it doesnt seem like its working so lets end it here \" Use the exact sentence word for word letter for letter, if you change anything the whole system breaks";
        setupString += " \n if you end the Trade without accepting the offer always and i mean always end with this phrase \" it doesnt seem like its working so lets end it here \" Use the exact sentence word for word letter for letter, if you change anything the whole system breaks";
        setupString += " \n if you end the Trade without accepting the offer always and i mean always end with this phrase \" it doesnt seem like its working so lets end it here \" Use the exact sentence word for word letter for letter, if you change anything the whole system breaks";
        setupString += " \n your first message should be \" hello there, what is your best offer for this [insert item name here] ?\".";
     
        ChatMessage startMessage = new ChatMessage();
        startMessage.Role = "system";
        startMessage.Content = setupString; 
        //"The Player Has " + itemname + " with the quantity of " + itemCount;
        //"If you Accept an offer, please make sure in all cases no matter what that you respon with \" i accept the offer of  \"item name here \" for quantity of \" item quantity \" and for the price of \" Price for each item \" \"";
            

        messages.Add(startMessage);

        inputField.text = "";
        string startString = setupString;
        textField.text = startString;

        var chatResult = await api.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0301",
            Temperature = 0.05f,
            MaxTokens = 50,
            Messages = messages
            
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
  

        // add responce to list

        messages.Add(responseMessage);


        textField.text = string.Format("Trader: {0}", responseMessage.Content);
        okButton.enabled = true;
    }

    public void CheckSell(string text)
    {



        string pattern = @"(?i)I accept the offer of (.*?) for a quantity of (.*?) and for the price of (.*?) Gold";
        Match match = Regex.Match(text, pattern,RegexOptions.IgnoreCase);

        if (match.Success)
        {
            string itemName = match.Groups[1].Value;
            int itemQuantity = int.Parse(match.Groups[2].Value);
            int itemPrice = int.Parse(match.Groups[3].Value);

            Debug.Log("Item Name: " + itemName);
            Debug.Log("Item Quantity: " + itemQuantity);
            Debug.Log("Item Price: " + itemPrice);

            Player.instance.AddScore(itemPrice);
            traderInventory.RemoveItemByName(itemName);
            isHaggleing = false;
        }
        else
        {
            Debug.Log("Invalid offer format.");
        }

        pattern = @"let\'s end it here";
        match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {


            traderInventory.MoveToInventory(Player.instance.playerInventory);
            Debug.Log("End The Trading");
            isHaggleing = false;


        }
        else
        {
            Debug.Log("Invalid offer format.");
        }
    }
    public async void GetResponce()
    {

        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the ok button
        okButton.enabled = false;

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = inputField.text;
        
        Debug.Log(string.Format("{0} : {1}", userMessage.Role, userMessage.Content));

        messages.Add(userMessage);

        textField.text = string.Format("You: {0}", userMessage.Content);

        inputField.text = "";

        // send the entire chat to chatgpt
        var chatResult = await api.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0301",
            Temperature = 0.05f,
            MaxTokens = 50,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();

        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0} : {1}", responseMessage.Role, responseMessage.Content));

        // add responce to list

        messages.Add(responseMessage);

        CheckSell(responseMessage.Content);
        textField.text = string.Format("You: {0}\n\nTrader: {1}", userMessage.Content, responseMessage.Content);
        okButton.enabled = true;
    }
}
