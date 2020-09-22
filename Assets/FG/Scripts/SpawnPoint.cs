using UnityEngine;

namespace FG
{
    public class SpawnPoint : MonoBehaviour
    {
        public bool isVisible;
        public Vector2 size;

        private void OnBecameVisible()
        {
            isVisible = true;
            Debug.Log("visable");

        }

        private void OnBecameInvisible()
        {
            isVisible = false;
            Debug.Log("Invisible");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (!isVisible)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 1, size.y));
        }
    }
}