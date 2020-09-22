using UnityEngine;

public class PooledParticle : MonoBehaviour
{
    private ParticleSystem pooledParticleSystem;
    private float duration;
    private float onEnabledTime;

    private void Awake()
    {
        pooledParticleSystem = GetComponent<ParticleSystem>();
        if (!pooledParticleSystem)
        {
            Debug.LogError("gameObject " + gameObject.name + " doesn't have a particle Component on it.");
        }
        else
        {
            duration = pooledParticleSystem.main.duration;
        }
    }

    private void OnEnable()
    {
        onEnabledTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - onEnabledTime > duration)
        {
            gameObject.SetActive(false);
        }
    }
}
