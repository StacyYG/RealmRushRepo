using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[ExecuteAlways]
public class Sorter : MonoBehaviour
{
    [SerializeField] private GameObject shore1, shore2, shore3, water;
    
    void Start()
    {
        var ponds = new GameObject("Ponds");
        var list1 = GameObject.FindGameObjectsWithTag("Shore1").ToList();
        var list2 = GameObject.FindGameObjectsWithTag("Shore2").ToList();
        var list3 = GameObject.FindGameObjectsWithTag("Shore3").ToList();
        var list4 = GameObject.FindGameObjectsWithTag("Water").ToList();
        foreach (var tile in list1)
        {
            var newTile = Instantiate(shore1, tile.transform.position, tile.transform.rotation, ponds.transform);
            newTile.name = tile.name;
        }
        foreach (var tile in list2)
        {
            var newTile = Instantiate(shore2, tile.transform.position, tile.transform.rotation, ponds.transform);
            newTile.name = tile.name;
        }
        foreach (var tile in list3)
        {
            var newTile = Instantiate(shore3, tile.transform.position, tile.transform.rotation, ponds.transform);
            newTile.name = tile.name;
        }
        foreach (var tile in list4)
        {
            var newTile = Instantiate(water, tile.transform.position, tile.transform.rotation, ponds.transform);
            newTile.name = tile.name;
        }
        
    }
}
