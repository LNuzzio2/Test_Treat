using System.Collections;
using UnityEngine;
/// <summary>
/// Script to control dice instances; strength, rotation, etc.
/// </summary>
public class DiceExplosionManager : MonoBehaviour
{
    [Header("Value Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int diceCount = 12;
    [SerializeField] private float explosionForce = 8f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float upwardModifier = 2f; // Fuerza hacia arriba para el arco
    [Space]
    [SerializeField] private GameObject confetiPartycles;

    [ContextMenu("Test Explosion")]
    public void TriggerExplosion()
    {
        // Verificamos si la aplicación está corriendo para evitar errores de Pool
        if (Application.isPlaying)
        {
            StartExplotion();
        }
        else
        {
            Debug.LogWarning("You must be in Play Mode to test the Dice Explosion!");
        }
    }

    public void StartExplotion()
    {
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        confetiPartycles.SetActive(true);

        for (int i = 0; i < diceCount; i++)
        {
            GameObject dice = DicePooler.Instance.GetDice();

            if (dice != null)
            {
                dice.transform.position = spawnPoint.position;
                dice.transform.rotation = Random.rotation;

                Rigidbody rb = dice.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                dice.SetActive(true);

                Vector3 forceDirection = (Vector3.up * upwardModifier) + (Random.insideUnitSphere * explosionRadius);

                rb.AddForce(forceDirection * explosionForce, ForceMode.Impulse);

                rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(spawnPoint.position, Vector3.up * 1f);
        }
    }
}