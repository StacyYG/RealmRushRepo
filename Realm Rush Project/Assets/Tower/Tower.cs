using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int startingCost = 100;
    [SerializeField] [Range(0f, 5f)] private float buildDelay = 1f;
    [Tooltip("how much the price increases everytime a tower is placed")]
    [SerializeField] private int priceRamp = 50;
    private void Start()
    {
        StartCoroutine(Build());
    }

    private Transform Parent()
    {
        var parent = GameObject.FindGameObjectWithTag("Towers");
        
        if (ReferenceEquals(parent, null))
        {
            var p = new GameObject("Towers").transform;
            p.tag = "Towers";
            return p;
        }
        
        return parent.transform;
    }

    public int Cost()
    {
        // calculate the tower cost. Tower cost increases every time a new tower is built
        var towerCount = Parent().childCount;
        var cost = startingCost + priceRamp * towerCount;
        return cost;
    }
    
    public bool CreateTower(Tower tower, Vector3 position)
    {
        var bank = FindObjectOfType<Bank>();
        if (bank == null)
            return false;
        
        var cost = Cost();
        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower.gameObject, position, Quaternion.identity, Parent());
            bank.WithDraw(cost);
            return true;
        }

        return false;
    }

    private IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
    
}
