using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;
    
    [Tooltip("Adds amount to maxHitPoints when enemy dies")]
    [SerializeField] private int difficultyRamp = 1;
    
    private int _currentHitPoints;

    private Enemy _enemy;

    private Transform _healthBar;
    private float _initialScaleX;
    private EnemyAudioManager _enemyAudio;
    private void Awake()
    {
        _healthBar = transform.GetChild(1);
        _initialScaleX = _healthBar.localScale.x;
        _enemyAudio = GetComponentInParent<EnemyAudioManager>();
        
    }

    // OnEnable: when the object is enabled/re-enabled
    void OnEnable()
    {
        _currentHitPoints = maxHitPoints;
        ChangeHealthBar(1f);
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        _enemyAudio.PlayHitSound();
    }

    void ProcessHit()
    {
        _currentHitPoints--;
        ChangeHealthBar((float) _currentHitPoints / maxHitPoints);
        if (_currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            _enemy.RewardGold();
        }
    }

    void ChangeHealthBar(float scaleXMultiplier)
    {
        var scale = _healthBar.localScale;
        scale.x = _initialScaleX * scaleXMultiplier;
        _healthBar.localScale = new Vector3(scale.x, scale.y, scale.z);
        
    }
}
