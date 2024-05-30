using System;
using UnityEngine;

public class ActorStats : Stats
{
    [Header("Base stats:")]
    public float hp;
    public float damage;
    public float moveSpeed;
    public float knockBackFore;
    public float knockBackTime;
    public float invincibleTime;

    public override bool IsMaxLevel()
    {
        return false;
    }

    public override void Load()
    {
        
    }

    public override void Save()
    {
       
    }

    public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
    {

    }


  
}
