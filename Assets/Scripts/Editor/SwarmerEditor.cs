using System;
using UnityEngine;
using  UnityEditor;

[CustomEditor(typeof(Swarmer))]
public class SwarmerEditor : Editor
{
    private Swarmer swarmer;
    private void OnEnable()
    {
        swarmer = (Swarmer) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.MinMaxSlider(ref swarmer.minWaver, ref swarmer.maxWaver, -180, 180);
        float decimals = 0;
        swarmer.minWaver = Mathf.Round(swarmer.minWaver * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
        swarmer.maxWaver = Mathf.Round(swarmer.maxWaver * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
    }
}
