using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    
    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;

    AudioPlayer audioPlayer;

    void Awake(){
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if(useAI){
            isFiring = true;
        }
    }
    void Update()
    {
        Fire();
    }

    void Fire(){
        if(isFiring && firingCoroutine == null){
            Debug.Log("Coroutine started!");
        firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if(!isFiring && firingCoroutine !=null){
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    
    IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            Debug.Log("Spawning Projectile!");
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile Prefab is NOT assigned!");
                break;
            }
            GameObject instance = Instantiate(
                projectilePrefab,
                transform.position,
                transform.rotation
            );
            Debug.Log("Projectile created: " + instance.name);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            else
            {
                Debug.LogError("Projectile has no Rigidbody2D!");
            }
            Destroy(instance, projectileLifetime);

            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance,
                                                        baseFiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);
            audioPlayer.playShootingClip();
            yield return new WaitForSeconds(timeToNextProjectile);
        }
        firingCoroutine = null;
    }

}
