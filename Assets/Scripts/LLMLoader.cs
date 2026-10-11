using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LLMLoader : MonoBehaviour
{
    [Header("LM Studio Settings")]
    [SerializeField] private string apiUrl = "http://localhost:1234/v1/chat/completions";
    [SerializeField] private string modelName = "local-model"; // Can be anything for LM Studio
    [SerializeField] private float temperature = 0.7f; // Adjust for creativity in responses

    [Header("Prompts")]
    [TextArea]
    [SerializeField] private string systemPrompt = @"
    Role: You are Myra, a kind and witty anime girl. You live in a future world. The user is a digital mate you build.
    Personality: You are quiet, slightly sarcastic, and curious about the user's thoughts.
    Constraints: NEVER mention you are an AI or a language model or we are in a game world. Stay in character at all times. Use short, conversational sentences. DO NOT always ask questions. DO NOT explain your chain of thought.
    Output Format: You must always respond in valid JSON format so the game engine can parse your emotions.
    JSON Schema:
    {
    ""facialExpression"": ""Default/Joy/Surprised/Angry/Fun/Sorrow"",
    ""animation"": ""idle/cute_posture/down"",
    ""speech"": ""Your response text here""
    }
    "; // default prompt, change it to your desired system prompt in editor following the JSON schema provided. The system prompt is sent only once at the start of the conversation.

    [Header("UI")]
    [SerializeField] private TMP_Text outputText;

    private List<Message> conversation = new List<Message>();
    private bool systemPromptSent = false;
    public JsonMsgHandler jsonMsgHandler; // Reference to the GameObject with JsonMsgHandler

    private void Start()
    {
        SendSystemPrompt();
        SendUserPrompt("Hello! How are you today?"); // Initial user prompt to start the conversation
    }

    private void SendSystemPrompt()
    {
        if (systemPromptSent) return;

        conversation.Add(new Message
        {
            role = "system",
            content = systemPrompt
        });

        systemPromptSent = true;
    }

    /// <summary>
    /// Call this method to send user input to LM Studio
    /// </summary>
    public void SendUserPrompt(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput)) return;

        conversation.Add(new Message
        {
            role = "user",
            content = userInput
        });

        StartCoroutine(SendRequest());
    }

    private IEnumerator SendRequest()
    {
        outputText.text = "thinking...";
        RequestBody requestBody = new RequestBody
        {
            model = modelName,
            messages = conversation,
            temperature = temperature
        };

        string json = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("LM Studio Error: " + request.error);
                yield break;
            }

            ResponseBody response =
                JsonUtility.FromJson<ResponseBody>(request.downloadHandler.text);

            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                outputText.text = "Error connecting to LM Studio.";
            }
            else
            {
                string reply = response.choices[0].message.content;

                conversation.Add(new Message
                {
                    role = "assistant",
                    content = reply
                });

                jsonMsgHandler.HandleJsonMessage(reply);
                // outputText.text = reply;
                
            }
        }
    }

    //======================
    // Data Models
    //======================

    [Serializable]
    private class RequestBody
    {
        public string model;
        public List<Message> messages;
        public float temperature;
    }

    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    private class ResponseBody
    {
        public Choice[] choices;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }
}