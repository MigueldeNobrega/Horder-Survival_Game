using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilPooling : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();
    void Start()
    {
        // Crear la pool de proyectiles
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            pool.Enqueue(projectile);
        }
    }

    public GameObject GetProjectile()
    {
        if (pool.Count > 0)
        {
            GameObject projectile = pool.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab);
            return projectile;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        pool.Enqueue(projectile);
    }
}
