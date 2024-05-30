
using UnityEngine;

public class Enemy : Actor
{
    private Player m_player;
    private EnemyStats m_enemyStats;

   private float m_curDamage;
    private float m_xpBonus;
    public float CurDamage { get => m_curDamage;private set => m_curDamage = value; }
    protected override void Init()
    {
        m_player=GameManager.Ins.Player;
        if (m_player == null || statData == null) return;
        m_enemyStats=(EnemyStats)statData;
        m_enemyStats.Load();
        StatsCaculate();
        OnDead.AddListener(OnSpawnCollectable);
        OnDead.AddListener(OnAddxpToPlayer);
    }

    private void StatsCaculate()
    {
     var playerStats=m_player.PlayerStats;
        if (playerStats == null) return;
        float hpUpgrade=  m_enemyStats.hpUp*Helper.GetUpgradeFormula(playerStats.level+1);
        float damageUpgrade =  m_enemyStats.damageUp * Helper.GetUpgradeFormula(playerStats.level + 1);
        float randomXpBonus = Random.Range(m_enemyStats.maxXPBonus, m_enemyStats.maxXPBonus);
        CurHp = m_enemyStats.hp+ hpUpgrade;
        CurDamage = m_enemyStats.damage+damageUpgrade;
        m_xpBonus = randomXpBonus * Helper.GetUpgradeFormula(playerStats.level + 1);
    }
    protected override void Die()
    {
        base.Die();
        m_anim.SetTrigger(AnimConsts.ENEMY_DEAD_PARAM);
    }
    private void OnSpawnCollectable()
    {
        CollectableManager.Ins.Spawn(transform.position);
    }
    private void OnAddxpToPlayer()
    {
        GameManager.Ins.Player.Addxp(m_xpBonus);
    }
    private void FixedUpdate()
    {
        Move();
    }
    protected override void Move()
    {
        if (IsDead || m_player == null) return;
        Vector2 playerDir=m_player.transform.position-transform.position;
        playerDir.Normalize();
        if(!m_isKnockback)
        {
            Flip(playerDir);
            m_rb.velocity=playerDir*m_enemyStats.moveSpeed*Time.deltaTime;

            return;
        }
        m_rb.velocity=playerDir*-m_enemyStats.knockBackFore*Time.deltaTime; 
    }

    private void Flip(Vector2 playerDir)
    {
        if (playerDir.x > 0)
        {
            if (transform.localScale.x > 0) return;
            transform.localScale=new Vector3(transform.localScale.x*-1, transform.localScale.y,transform.localScale.z);
        }  
        else if (playerDir.x < 0)
        {
            if(transform.localScale.x < 0)return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDisable()
    {
        OnDead.RemoveListener(OnSpawnCollectable);
        OnDead.RemoveListener(OnAddxpToPlayer);
    }
}
