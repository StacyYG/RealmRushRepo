using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int startCoordinates, endCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    public Vector2Int EndCoordinates { get { return endCoordinates; } }
    private Node _startNode, _endNode, _currentSearchNode;
    private Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    private GridManager _gridManager;

    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>(),
        _reached = new Dictionary<Vector2Int, Node>();
    private Queue<Node> _frontier = new Queue<Node>();
    private CanvasManager _canvas;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _canvas = FindObjectOfType<CanvasManager>();
        if (_gridManager != null)
            _grid = _gridManager.Grid;
        
        _startNode = _grid[startCoordinates];
        _endNode = _grid[endCoordinates];

    }

    // Start is called before the first frame update
    private void Start()
    {
        GetNewPath();

    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }
    
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    private Vector2Int[] RandomDirections(Vector2Int[] directions)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        foreach (var d in directions)
        {
            list.Add(d);
        }

        Vector2Int[] array = list.ToArray();
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(0, list.Count);
            var temp = array[rand];
            array[rand] = array[i];
            array[i] = temp;
        }

        return array;
    }
    
    private void ExploreNeighbors()
    {
        var neighbors = new List<Node>();
        foreach (var direction in RandomDirections(directions))
        {
            var neighborCoords = _currentSearchNode.coordinates + direction;
            
            if (_grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(_grid[neighborCoords]);
            }
        }

        foreach (var neighbor in neighbors)
        {
            if (!_reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                // create connections to build the tree
                neighbor.connectedTo = _currentSearchNode;
                
                _reached.Add(neighbor.coordinates, neighbor);
                _frontier.Enqueue(neighbor);
            }
        }
    }
    private void BreadthFirstSearch(Vector2Int searchStartCoords)
    {
        _startNode.isWalkable = true;
        _endNode.isWalkable = true;
        
        _frontier.Clear();
        _reached.Clear();
        
        bool isRunning = true;

        _frontier.Enqueue(_grid[searchStartCoords]);
        _reached.Add(searchStartCoords, _grid[searchStartCoords]);
        
        while (_frontier.Count > 0 && isRunning)
        {
            _currentSearchNode = _frontier.Dequeue();
            _currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (_currentSearchNode.coordinates == endCoordinates)
            {
                isRunning = false;
            }
        }
    }

    private List<Node> BuildPath()
    {
        var path = new List<Node>();
        var currentNode = _endNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();
        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            // save the tile's walkable state
            var previousState = _grid[coordinates].isWalkable;
            
            _grid[coordinates].isWalkable = false;
            var newPath = GetNewPath();
            
            // recover the tile's walkable state
            _grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                _canvas.NoticeBlockPath(Input.mousePosition);
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        // calls the "RecalculatePath" method in every component in itself and children
        // able to shout to the void even when no one is listening
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
