using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Vector3 position;
    public bool isVisible;
    public Vector2 size;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 1, size.y));
    }
}