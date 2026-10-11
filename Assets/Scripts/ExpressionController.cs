using UnityEngine;

public class ExpressionController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetExpression(string expressionName)
    {
        Debug.Log($"SetExpression function is called with expression: {expressionName}");
        if (animator != null)
        {
            Debug.Log($"Setting expression: {expressionName}");
            animator.SetTrigger(expressionName);
        }
        else
        {
            Debug.LogWarning("Animator is not assigned in ExpressionController.");
        }
    }
}
