using System;
using UnityEngine;

public class GameplayEventManager : MonoBehaviour
{

    public static GameplayEventManager current;
    public event Action onPickUp;

    private void Awake()
    {
        if (!current)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickUp()
    {
        onPickUp?.Invoke();
    }
}
