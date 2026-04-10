using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
/// <summary>
/// Script to activate/deactivate dynamic random events (objects) in the scene
/// </summary>
public class RandomBackgroundEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject targetObject; 
    [SerializeField] private SplineAnimate _splineAnimate;

    [Tooltip("Time OFF Min / Max")]
    [SerializeField] private Vector2 idleTimeRange = new Vector2(5f, 15f);

    [Tooltip("Time ON Min / Max")]
    [SerializeField] private Vector2 activeTimeRange = new Vector2(2f, 4f);

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("There is no assigned targetObject.");
            return;
        }

        targetObject.SetActive(false);

        StartCoroutine(EventRoutine());
    }

    private IEnumerator EventRoutine()
    {
        while (true)
        {
            float waitToActivate = Random.Range(idleTimeRange.x, idleTimeRange.y);
            yield return new WaitForSeconds(waitToActivate);

            targetObject.SetActive(true);

            //If there any spline in the event
            SplineBehavior();

            float waitToDeactivate = Random.Range(activeTimeRange.x, activeTimeRange.y);
            yield return new WaitForSeconds(waitToDeactivate);

            targetObject.SetActive(false);
        }
    }

    void SplineBehavior()
    {
        if (_splineAnimate != null)
        {
            _splineAnimate.ElapsedTime = 0f;

            _splineAnimate.Restart(true);

            _splineAnimate.Play();
        }
    }
}