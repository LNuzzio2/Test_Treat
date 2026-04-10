using UnityEngine;
/// <summary>
/// Script to control the life of dice. Also used on other objects in a recycling format.
/// </summary>
public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float delay = 5f;

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), delay);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}