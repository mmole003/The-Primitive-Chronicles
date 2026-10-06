using UnityEngine;
using System;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;

    public int gemsCollected = 0;

    public event Action<int> OnGemsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectGem(int amount)
    {
        gemsCollected += amount;

        Debug.Log("Gems Collected: " + gemsCollected);

        OnGemsChanged?.Invoke(gemsCollected);
    }
}