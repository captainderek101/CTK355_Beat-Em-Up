using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenClearAttack : MonoBehaviour
{
    [SerializeField] private Transform explosion;
    [SerializeField] private SpriteRenderer billboard;
    [SerializeField] private float timeToActivation;
    [SerializeField] private float maxScale;
    [SerializeField] private float maxAlpha;
    [SerializeField] private float healthEffect = -10f;
    [HideInInspector] public GameObject source = null;

    private const string enemyTagName = "Enemy";

    private void Start()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        float timeSinceStart = 0;
        float currentScale = explosion.localScale.x;
        float scalePerSecond = (maxScale - currentScale) / timeToActivation;
        Color currentColor = billboard.color;
        float currentAlpha = currentColor.a;
        float alphaPerSecond = (maxAlpha - currentAlpha) / timeToActivation;
        while (timeSinceStart + 0.01f < timeToActivation)
        {
            yield return new WaitForFixedUpdate();
            currentScale += scalePerSecond * Time.fixedDeltaTime;
            explosion.localScale = new Vector3(currentScale, currentScale, currentScale);
            currentAlpha += alphaPerSecond * Time.fixedDeltaTime;
            currentColor.a = currentAlpha;
            billboard.color = currentColor;
            timeSinceStart += Time.fixedDeltaTime;
        }
        billboard.enabled = false;
        DealDamage();
    }

    private void DealDamage()
    {
        // damage all enemies on the screen
        HealthController[] healthControllers = (HealthController[])FindObjectsByType(typeof(HealthController), FindObjectsSortMode.None);
        foreach (HealthController healthController in healthControllers)
        {
            if(healthController.tag == enemyTagName)
            {
                Vector3 screenPosition = Camera.main.WorldToViewportPoint(healthController.transform.position);
                if (Mathf.Min(screenPosition.x, screenPosition.y) > 0 && Mathf.Max(screenPosition.x, screenPosition.y) < 1)
                {
                    healthController.ChangeHealth(healthEffect, source);
                }
            }
        }
        Destroy(gameObject);
    }
}
