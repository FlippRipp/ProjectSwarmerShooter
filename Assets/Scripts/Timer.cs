using System;
using UnityEngine;
using  UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timer;

    private void Update()
    {
        timer.text = Mathf.Round(Time.time).ToString();
    }
}
