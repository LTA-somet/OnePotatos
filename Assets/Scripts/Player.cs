using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
    [Header("Player Setting:")]
    [SerializeField] private float m_accelerationSpeepd;
    [SerializeField] private float m_maxMousePosDistance;
    [SerializeField] private Vector2 m_velocityLimit;
    [SerializeField] private float m_enemyDetectionRadius;
    [SerializeField] private LayerMask m_enemyDetectionLayer;
    private float m_curSpeed;
    private Actor m_enemyTargeted;
    private Vector2 m_enemyTargetedDir;
    private PlayerStats m_playerStats;
    private bool facingRight = true;
    [Header("Player Events:")]
    public UnityEvent OnAddXp;
    public UnityEvent OnLevelUp;
    public UnityEvent OnLostLife;
    public PlayerStats PlayerStats { get => m_playerStats;private set => m_playerStats = value; }

    protected override void Init()
    {
        LoadStats();
    }

    private void LoadStats()
    {
       if(statData==null)return;
       m_playerStats=(PlayerStats)statData;
        m_playerStats.Load();
        CurHp = m_playerStats.hp;
    }
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (moveHorizontal > 0 && !facingRight)
        {
            Flip(); // Lật nhân vật về bên phải nếu đang nhìn về bên trái
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip(); // Lật nhân vật về bên trái nếu đang nhìn về bên phải
        }
        Move();
    }
    private void FixedUpdate()
    {
        DetectEnemy();
    }

    private void DetectEnemy()
    {
        var enemyFindeds =Physics2D.OverlapCircleAll(transform.position, m_enemyDetectionRadius,m_enemyDetectionLayer);
        var finalEnemy=FindNearestEnemy(enemyFindeds);
        if(finalEnemy==null) return;
        m_enemyTargeted = finalEnemy;
        WeaponHandle();
    }

    private void WeaponHandle()
    {
       if(m_enemyTargeted == null||weapon==null)return ;
       m_enemyTargetedDir=m_enemyTargeted.transform.position-weapon.transform.position;
        m_enemyTargetedDir.Normalize();
        float angle =Mathf.Atan2(m_enemyTargetedDir.y,m_enemyTargetedDir.x)*Mathf.Rad2Deg;
        weapon.transform.rotation=Quaternion.Euler(0f,0f,angle);
        if (m_isKnockback)return ;
        
       weapon.Shoot(m_enemyTargetedDir);
    }

    private Actor FindNearestEnemy(Collider2D[] enemyFindeds)
    {
        float minDistance = 0;
        Actor finalEnemy = null;
        if(enemyFindeds == null||enemyFindeds.Length<=0) return null;
        for(int i = 0; i < enemyFindeds.Length; i++)
        {
            var enemyFind = enemyFindeds[i];
            if(enemyFind == null) continue;
            if(finalEnemy == null)
            {
                minDistance=Vector2.Distance(transform.position,enemyFind.transform.position);
            }
            else
            {
                float distanceTemp = Vector2.Distance(transform.position, enemyFind.transform.position);
                if(distanceTemp>minDistance)continue;
                minDistance = distanceTemp;
            }
            finalEnemy = enemyFind.GetComponent<Actor>();  
        }
        return finalEnemy;
    }

    private void Flip()
    {
        facingRight = !facingRight; 

     
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //protected override void Move()
    //{
    //    if (IsDead) return;

    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 movingDir = mousePos - (Vector2)transform.position;
    //    movingDir.Normalize();
    //    if (!m_isKnockback)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {

    //            Run(mousePos, movingDir);

    //        }
    //        else
    //        {
    //            BackToIdle();
    //        }
    //        return;
    //    }
    //    m_rb.velocity = m_enemyTargetedDir * -statData.knockBackFore * Time.deltaTime;
    //    m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    //}
    protected override void Move()
    {
        if (IsDead) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movingDir = new Vector2(moveHorizontal, moveVertical);
        if (!m_isKnockback)
        {
            if (movingDir != Vector2.zero)
            {
                Run(movingDir);
            }
            else
            {
                BackToIdle();
            }
            return;
        }
        m_rb.velocity = m_enemyTargetedDir * -statData.knockBackFore * Time.deltaTime;
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    }

    private void Run(Vector2 movingDir)
    {
      

        m_curSpeed += m_accelerationSpeepd * Time.deltaTime;
        m_curSpeed = Mathf.Clamp(m_curSpeed, m_playerStats.moveSpeed, m_playerStats.moveSpeed);
        float delta = m_curSpeed * Time.deltaTime;

        m_rb.velocity = movingDir * delta;
        float velocityLimitX = Mathf.Clamp(m_rb.velocity.x, -m_velocityLimit.x, m_velocityLimit.y);
        float velocityLimitY = Mathf.Clamp(m_rb.velocity.y, -m_velocityLimit.y, m_velocityLimit.y);
        m_rb.velocity = new Vector2(velocityLimitX, velocityLimitY);
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, true);
    }

    private void BackToIdle()
    {
        m_curSpeed -= m_accelerationSpeepd * Time.deltaTime;
        m_curSpeed = Mathf.Clamp(m_curSpeed, 0, m_curSpeed);

        m_rb.velocity = Vector2.zero;
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    }

    //private void Run(Vector2 mousePos, Vector2 movingDir)
    //{
    //    m_curSpeed += m_accelerationSpeepd * Time.deltaTime;
    //    m_curSpeed = Mathf.Clamp(m_curSpeed, 0, m_playerStats.moveSpeed);
    //    float delta = m_curSpeed*Time.deltaTime;
    //    float distanceToMousePos =Vector2.Distance(transform.position, mousePos);
    //    distanceToMousePos = Mathf.Clamp(distanceToMousePos, 0, m_maxMousePosDistance / 3);
    //    delta += distanceToMousePos;
    //    m_rb.velocity = movingDir * delta;
    //    float velocityLimitX = Mathf.Clamp(m_rb.velocity.x, -m_velocityLimit.x, m_velocityLimit.y);

    //    float velocityLimitY= Mathf.Clamp(m_rb.velocity.y, -m_velocityLimit.y, m_velocityLimit.y);
    //    m_rb.velocity=new Vector2(velocityLimitX, velocityLimitY); ;
    //    m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM,true);
    //}
    public void Addxp(float xpBonus)
    {
        if (m_playerStats == null) return;
        m_playerStats.xp += xpBonus;
        m_playerStats.Upgrade(OnUpgradeStat);
        OnAddXp?.Invoke();
        m_playerStats.Save();
    }
    private void OnUpgradeStat()
    {
        OnLevelUp?.Invoke();
    }
    public override void TakeDamage(float damage)
    {
        if(m_isInvincible) return;
       // Debug.Log("take damage");
        CurHp -= damage;
        CurHp = Mathf.Clamp(CurHp, 0, PlayerStats.hp);
        Knockback();
        OnTakeDamge?.Invoke();
        if (CurHp > 0) return;
        GameManager.Ins.GameoverChecking(OnLostLifeDelegate,OnDeadDelegate);
    }

    private void OnLostLifeDelegate()
    {
        
        CurHp = m_playerStats.hp;
        if (m_stopKnockbackCo != null)
        {
            StopCoroutine(m_stopKnockbackCo);
        }
        if(m_invincibleCo != null)
        {
            StopCoroutine(m_invincibleCo);
        }
        Invincible(3f);
        OnLostLife?.Invoke();
    }
    private void OnDeadDelegate()
    {
        CurHp = 0;
        Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.CurDamage);
            }
        }
       else if(collision.gameObject.CompareTag(TagConsts.COLLECTABLE_TAG))
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();
            collectable?.Trigger();
            Destroy(collectable.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(133,250,47,50);
        Gizmos.DrawSphere(transform.position, m_enemyDetectionRadius);
    }
}
