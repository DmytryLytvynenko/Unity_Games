using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private HealthControll Health;

    private Camera camera;


    private void Awake()
    {
        Health.HealthChanged += OnHealthChanged;
        camera = Camera.main;
    }
    private void OnDestroy()
    {
        Health.HealthChanged -= OnHealthChanged;
    }


    private void OnHealthChanged(float valueAsPercentage)
    {
        healthBarFilling.fillAmount = valueAsPercentage;
    }
    private void LateUpdate()
    {
        transform.LookAt(new Vector3(transform.position.x, camera.transform.position.y, camera.transform.position.z));
        transform.Rotate(0, 180, 0);
    }
}
