using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Script that controls a basic pool, in this case of dice
/// </summary>
public class DicePooler : MonoBehaviour
{
    public static DicePooler Instance; // Singleton para fácil acceso

    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> _pooledDice = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        // Pre-instant
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(dicePrefab);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            _pooledDice.Add(obj);
        }
    }

    public GameObject GetDice()
    {
        // Search a disable dice
        foreach (GameObject dice in _pooledDice)
        {
            if (!dice.activeInHierarchy)
            {
                return dice;
            }
        }
        return null;

        /* // Optional: if we dong have any more dice
        GameObject newObj = Instantiate(dicePrefab);
        newObj.transform.SetParent(this.transform);
        newObj.SetActive(false);
        _pooledDice.Add(newObj);
        return newObj;*/
    }
}