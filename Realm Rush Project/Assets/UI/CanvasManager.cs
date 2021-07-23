using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _blockPathNotice, _noMoneyNotice, _towerCost;

    private TextMeshProUGUI _towerCostTmp;
    // Start is called before the first frame update
    void Start()
    {
        _blockPathNotice.SetActive(false);
        _noMoneyNotice.SetActive(false);
        _towerCost.SetActive(true);
        _towerCostTmp = _towerCost.GetComponent<TextMeshProUGUI>();
    }

    public void NoticeBlockPath(Vector3 position)
    {
        _blockPathNotice.SetActive(true);
        _blockPathNotice.transform.position = position;
    }
    
    public void NoticeNoMoney(Vector3 position)
    {
        _noMoneyNotice.SetActive(true);
        _noMoneyNotice.transform.position = position;
    }

    public void UpdateCost(int cost)
    {
        _towerCostTmp.text = $"next tower: {cost}";
    }
}
