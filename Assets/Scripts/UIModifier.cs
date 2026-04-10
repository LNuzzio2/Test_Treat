using UnityEngine;
/// <summary>
/// Simple script to generally control scale and rotation animations from the inspector
/// </summary>
public class UIModifier : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private bool useRotation = false;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;

    [Header("Pulse Scale Settings")]
    [SerializeField] private bool useScale = false;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float scaleAmount = 0.1f; 

    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;
    }

    void Update()
    {
        if (useRotation)
        {
            ApplyRotation();
        }

        if (useScale)
        {
            ApplyPulse();
        }
    }

    //Rotation
    private void ApplyRotation()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    //Scale
    private void ApplyPulse()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * scaleAmount;
        transform.localScale = _initialScale + Vector3.one * pulse;
    }
}