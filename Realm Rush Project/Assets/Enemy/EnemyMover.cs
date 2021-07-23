using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Tooltip("number of grids travels in 1 second")] [Range(0f, 5f)]
    private float speed = 1f;
    
    private Enemy _enemy;
    private List<Node> path = new List<Node>();
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    private ObjectPool _objectPool;
    private LayerMask _mask;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = FindObjectOfType<PathFinder>();
        _objectPool = FindObjectOfType<ObjectPool>();
        _mask = LayerMask.GetMask("Enemy");

    }

    // OnEnable: when the object is enabled/re-enabled
    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }
    
    void RecalculatePath(bool resetWholePath)
    {
        var coordinates = new Vector2Int();
        if (resetWholePath)
            coordinates = _pathFinder.StartCoordinates;
        else
            coordinates = _gridManager.GetCoordsFromPos(transform.position);

        StopAllCoroutines();
        path.Clear();
        path = _pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = _gridManager.GetPosFromCoords(_pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        _enemy.StealGold();
        gameObject.SetActive(false);
    }
    
    IEnumerator FollowPath()
    {
        for(int i = 1; i < path.Count; i++)
        {
            var startPos = transform.position;
            var endPos = _gridManager.GetPosFromCoords(path[i].coordinates);
            var travelPercent = 0f;

            transform.LookAt(endPos);
            
            while (travelPercent < 1f)
            {
                if (Jammed())
                {
                    yield return new WaitForEndOfFrame();
                }
                
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        
        FinishPath();
    }
    
    private bool Jammed()
    {
        var enemyDistance = speed * _objectPool.TimeInterval * 10f;
        return Physics.Raycast(transform.position + 2f * Vector3.up, transform.forward, enemyDistance - 1f, _mask);
    }
    
}
