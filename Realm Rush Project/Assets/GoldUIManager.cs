using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUIManager : MonoBehaviour
{
    private List<GameObject> textObjects = new List<GameObject>();

    [SerializeField] private GameObject goldChangePrefab;

    [SerializeField] private int poolSize = 10;

    [SerializeField] private float sleepWaitTime = 2f;

    [SerializeField] private Color _rewardColor, _stealColor;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var g = Instantiate(goldChangePrefab, transform);
            g.SetActive(false);
            textObjects.Add(g);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Print(Vector3 position, string toPrint, bool isReward)
    {
        var textObj = GetObjectInPool();
        textObj.transform.position = position;
        var tmPro = textObj.GetComponent<TextMeshPro>();
        tmPro.text = toPrint;
        tmPro.color = isReward ? _rewardColor : _stealColor;
        StartCoroutine(PutToSleep(textObj, sleepWaitTime));
    }
    
    GameObject GetObjectInPool()
    {
        foreach (var textObj in textObjects)
        {
            if (!textObj.activeSelf)
            {
                textObj.SetActive(true);
                return textObj;
            }
        }
        
        var newTextObj = Instantiate(goldChangePrefab);
        textObjects.Add(newTextObj);
        return newTextObj;
    }

    private IEnumerator PutToSleep(GameObject gameObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}
