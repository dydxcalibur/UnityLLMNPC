using UnityEngine;
using TMPro;

public class JsonMsgHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ExpressionController expressionController;
    [SerializeField] private TMP_Text outputText;
    public void HandleJsonMessage(string jsonMessage)
    {
        // filter empty or null msg
        if (string.IsNullOrEmpty(jsonMessage))
        {
            Debug.LogWarning("Received an empty JSON message.");
            return;
        }
        Debug.Log("Received JSON Message: " + jsonMessage);
        // Parse the JSON message and handle it accordingly
        CharacterMessage characterMessage = null;
        try
        {
            // Assuming the JSON message has a structure like: { "command": "someCommand", "data": "someData" }
            characterMessage = JsonUtility.FromJson<CharacterMessage>(jsonMessage);
            if (characterMessage != null)
            {
                Debug.Log("Facial Expression: " + characterMessage.facialExpression);
                expressionController.SetExpression(characterMessage.facialExpression);

                Debug.Log("Animation: " + characterMessage.animation);
                //

                Debug.Log("Speech: " + characterMessage.speech);
                // outputText.SetActive(true);
                outputText.text = characterMessage.speech;
                // Handle the command and data as needed
            }
            else
            {
                Debug.LogWarning("Failed to parse JSON message.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing JSON message: " + ex.Message);
        }
    }
}

public class CharacterMessage
{
    public string facialExpression;
    public string animation;
    public string speech;
}
