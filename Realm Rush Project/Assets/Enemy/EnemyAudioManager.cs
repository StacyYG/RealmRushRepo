using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    private AudioSource _hit, _reward, _steal;
    [SerializeField] private AudioClip _hitClip, _rewardClip, _stealClip;

    // Start is called before the first frame update
    void Start()
    {
        _hit = gameObject.AddComponent<AudioSource>();
        _reward = gameObject.AddComponent<AudioSource>();
        _steal = gameObject.AddComponent<AudioSource>();
        _hit.clip = _hitClip;
        _reward.clip = _rewardClip;
        _steal.clip = _stealClip;
        _steal.volume = 0.25f;
    }

    public void PlayHitSound()
    {
        if (!_hit.isPlaying)
        {
            _hit.Play();
        }
    }
    
    public void PlayRewardSound()
    {
        if (!_reward.isPlaying)
        {
            _reward.Play();
        }
    }
    
    public void PlayStealSound()
    {
        if (!_steal.isPlaying)
        {
            _steal.Play();
        }
    }
}
