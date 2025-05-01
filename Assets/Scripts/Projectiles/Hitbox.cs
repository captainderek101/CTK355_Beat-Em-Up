using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float healthEffect = -1f;
    public AnimationCurve knockbackSpeedCurve;
    public float knockbackDuration = 1f;
    public GameObject source = null;
}
