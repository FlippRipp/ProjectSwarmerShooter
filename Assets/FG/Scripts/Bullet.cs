using UnityEngine;

namespace FG
{

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.ObjectType particleEffect;

        private float damage = 10;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")) return;

            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
            }

            GameObject particle = ObjectPooler.instance.GetPooledObject(particleEffect);
            if (particle)
            {
                particle.transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }
    }
}