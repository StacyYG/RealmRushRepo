using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int goldReward = 25;
    [SerializeField] private int goldPenalty = 25;

    private Bank _bank;
    private GoldUIManager _goldUIManager;
    private EnemyAudioManager _enemyAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        _bank = FindObjectOfType<Bank>();
        _goldUIManager = FindObjectOfType<GoldUIManager>();
        _enemyAudio = GetComponentInParent<EnemyAudioManager>();
        
    }

    public void RewardGold()
    {
        if (_bank == null) 
            return;
        _bank.Deposit(goldReward);
        _goldUIManager.Print(transform.position, $"+{goldReward}", true);
        _enemyAudio.PlayRewardSound();
    }

    public void StealGold()
    {
        if (_bank == null) 
            return;
        _bank.WithDraw(goldPenalty);
        _goldUIManager.Print(transform.position, $"-{goldReward}", false);
        _enemyAudio.PlayStealSound();
    }
}
