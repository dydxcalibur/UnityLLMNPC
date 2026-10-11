using UnityEngine;
using TMPro;

public class ChatInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private LLMLoader backendScript; // Reference to the backend script that handles the input


    void Start()
    {
        if (inputField != null)
        {
            // Register to the onSubmit event, which fires when the user presses Enter
            inputField.onSubmit.AddListener(OnSubmitText);
        }
        else
        {
            Debug.LogError("TMP_InputField reference is missing!", this);
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener when the object is destroyed to prevent memory leaks
        if (inputField != null)
        {
            inputField.onSubmit.RemoveListener(OnSubmitText);
        }
    }

    private void OnSubmitText(string text)
    {
        // Check that the text isn't empty or just whitespace
        if (!string.IsNullOrWhiteSpace(text))
        {
            // 1. Send the text to your backend script
            if (backendScript != null)
            {
                backendScript.SendUserPrompt(text);
            }
            else
            {
                Debug.LogWarning("Backend script reference is missing!");
            }

            // 2. Clear the text field
            inputField.text = string.Empty;

            // 3. Refocus the input field so the user can type again immediately
            inputField.ActivateInputField();
            inputField.Select();
        }
    }
}
