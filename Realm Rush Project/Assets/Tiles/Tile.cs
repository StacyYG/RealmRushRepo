using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower tower;
    [SerializeField] private bool isPlaceable = true;

    private GridManager _gridManager;
    private PathFinder _pathFinder;
    Vector2Int _coordinates = new Vector2Int();
    private CanvasManager _canvas;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = FindObjectOfType<PathFinder>();
        _canvas = FindObjectOfType<CanvasManager>();
        
    }

    private void Start()
    {
        if (_gridManager != null)
        {
            _coordinates = _gridManager.GetCoordsFromPos(transform.position);
            if (!isPlaceable)
            {
                _gridManager.BlockNode(_coordinates);
            }
        }

    }

    private void OnMouseDown()
    {
        // the tile has to be walkable and will not block the path, to build a tower
        if (isPlaceable && _gridManager.GetNode(_coordinates).isWalkable && !_pathFinder.WillBlockPath(_coordinates))
        {
            var isSuccessful = tower.CreateTower(tower, transform.position);
            if (isSuccessful)
            {
                _gridManager.BlockNode(_coordinates);
                _pathFinder.NotifyReceivers();
                _canvas.UpdateCost(tower.Cost());
            }
            else
            {
                _canvas.NoticeNoMoney(Input.mousePosition);
            }
        }
    }
    
}
