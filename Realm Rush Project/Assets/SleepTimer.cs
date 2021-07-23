using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTimer : MonoBehaviour
{
    [SerializeField] private float _waitTime = 1f;

    private void OnEnable()
    {
        StartCoroutine(GoToSleep(_waitTime));
        
    }
    IEnumerator GoToSleep(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}
