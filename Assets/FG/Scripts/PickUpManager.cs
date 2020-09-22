using System.Collections.Generic;
using UnityEngine;

namespace FG
{

    public class PickUpManager : MonoBehaviour
    {
        [SerializeField] private List<PickUp> pickUps = new List<PickUp>();

        [SerializeField] private float timeBetweenPickupSpawns = 10f;
        [SerializeField] private int maxPickUps = 2;

        private float lastPickUpSpawn;
        private float currentActivePickUpAmount;
        
        private void Awake()
        {
            lastPickUpSpawn = Time.time;
            GameplayEventManager.instance.OnPickUp += OnPickUp;
            for (int i = 0; i < maxPickUps - 1; i++)
            {
                EnableRandomPickUp();
                currentActivePickUpAmount++;
            }
        }

        private void Update()
        {
            if(currentActivePickUpAmount >= maxPickUps) return;
            if (Time.time - lastPickUpSpawn > timeBetweenPickupSpawns)
            {
                currentActivePickUpAmount++;
                EnableRandomPickUp();
            }
        }

        private void OnPickUp()
        {
            lastPickUpSpawn = Time.time;
            currentActivePickUpAmount--;
        }

        private void EnableRandomPickUp()
        {
            int randomPickUp = Random.Range(0, pickUps.Count);
            
            if (!pickUps[randomPickUp].gameObject.activeSelf)
            {
                pickUps[randomPickUp].gameObject.SetActive(true);
            }
            else
            {
                EnableRandomPickUp();
            }
        }
    }
}
