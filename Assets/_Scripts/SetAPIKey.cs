using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SetAPIKey : MonoBehaviour
{
    public TMP_InputField apiKeyField;
    public string sceneName;

    public void SetKey()
    {
        ChatGPT.APIKey = apiKeyField.text;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
