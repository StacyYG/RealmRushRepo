using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0, 20)] private int poolSize = 10;
    [SerializeField] [Range(0.1f, 15f)] private float spawnTimer = 1f;

    private GameObject[] _pool;

    public float TimeInterval { get { return spawnTimer; } }

    private void Awake()
    {
        PopulatePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        _pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            _pool[i] = Instantiate(enemyPrefab, transform);
            _pool[i].name = $"Ram{i}";
            _pool[i].SetActive(false);
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            
            yield return new WaitForSeconds(spawnTimer);
            
        }
    }

    void EnableObjectInPool()
    {
        foreach (var enemy in _pool)
        {
            if (!enemy.activeSelf)
            {
                enemy.SetActive(true);
                return;
            }
        }
    }
}
