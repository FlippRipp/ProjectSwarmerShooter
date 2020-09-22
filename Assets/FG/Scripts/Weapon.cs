﻿using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Fire(Transform weaponTransform, Transform activator, Vector3 attackDir);
}
