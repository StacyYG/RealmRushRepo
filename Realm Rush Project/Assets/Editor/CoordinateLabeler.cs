using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = Color.gray;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1f, 0.5f, 0f);
    private TextMeshPro _label;
    Vector2Int _coordinates = new Vector2Int();
    private GridManager _gridManager;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _label = GetComponent<TextMeshPro>();
        _label.enabled = false;
        _rectTransform = GetComponent<RectTransform>();
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }

        SetLabelColor();
        ToggleLabels();
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _label.enabled = !_label.enabled;
        }
    }
    private void SetLabelColor()
    {
        if (ReferenceEquals(_gridManager, null)) 
            return;
        var node = _gridManager.GetNode(_coordinates);
        if(ReferenceEquals(node, null))
            return;
        if (!node.isWalkable) 
            _label.color = blockedColor;
        else if (node.isPath)
            _label.color = pathColor;
        else if (node.isExplored)
            _label.color = exploredColor;
        else
            _label.color = defaultColor;
    }
    private void DisplayCoordinates()
    {
        if (ReferenceEquals(_gridManager, null)) 
            return;
        var pos = transform.parent.position;
        _coordinates.x = Mathf.RoundToInt(pos.x / _gridManager.UnityGridSize);
        _coordinates.y = Mathf.RoundToInt(pos.z / _gridManager.UnityGridSize);

        _label.text = $"{_coordinates.x},{_coordinates.y}";
        RotateText();
    }

    void UpdateObjectName()
    {
        transform.parent.name = _coordinates.ToString();
    }

    void RotateText()
    {
        //var rotateY = -transform.parent.rotation.y;
        transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.up);
    }
}
