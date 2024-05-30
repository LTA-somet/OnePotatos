using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    STARTING,
    PLAYING,
    PAUSED,
    GAMEOVER
}
public class GameManager : Singleton<GameManager>
{
    public static GameState state;
    public Camera mainCamera;
    [SerializeField] private Map m_mapPrePrefab;
    [SerializeField] private Player m_playerPrefab;
    [SerializeField] private Enemy[] m_enemyPrefab;
    [SerializeField] private GameObject m_enemySpawnVfx;
    [SerializeField] private float m_enemySpawnTime;
    [SerializeField] private Enemy[] m_bossPrefab;
    [SerializeField] private GameObject m_bossSpawnVfx;
    [SerializeField] private float m_bossSpawnTime;
    [SerializeField] private int m_playerMaxLife;
    [SerializeField] private int m_playerStaringLife;
    [SerializeField] private    Text timer;
    private Map m_map;
    private Player m_player;
    private PlayerStats m_playerStats;
    private int m_curLife;
    private float m_curTime;
  //  private int scene = 3;
    public float stageTime;

    public Player Player { get => m_player;private set => m_player = value; }
    public int CurLife {
        get => m_curLife; 
        set {
        m_curLife = value;
            m_curLife =Mathf.Clamp(m_curLife,0,m_playerMaxLife);
        }
    
    
    }

    protected override void Awake()
    {
        MakeSingleton(false);
    }
    private void Start()
    {
        m_curTime = stageTime;
        Init();    
    }
    private void Update()
    {
        
        if (m_curTime > 0)
        {
            m_curTime -= Time.deltaTime;
            timer.text = $"{Mathf.FloorToInt(m_curTime)}";
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GUIManager.Ins.ShowGunUpgradeDialog();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIManager.Ins.ShowSettingDialog();
        }
      //  if (m_curTime <= 0)
      //  {

      //SceneManager.LoadScene("Victory");
            
      //  }
    }
    private void Init()
    {
        state = GameState.STARTING;
        m_curLife = m_playerStaringLife;
        SpawnMap_player();
        GUIManager.Ins.ShowGameGUI(true);
       
        GUIManager.Ins.UpdateCoinCounting(Prefs.coins);
     
        PlayGame();
    }
   
    private void SpawnMap_player()
    {
        if (m_mapPrePrefab == null || m_playerPrefab == null) return;
        m_map=Instantiate(m_mapPrePrefab,Vector3.zero, Quaternion.identity);
        m_player=Instantiate(m_playerPrefab,m_map.playerSpawnPoint.position, Quaternion.identity);


        CameraMove cameraFollow = Camera.main.GetComponent<CameraMove>();
        cameraFollow.target = m_player.transform;
        cameraFollow.offset = new Vector3(0, 0, -10);
    }

    public void PlayGame()
    {
     
        state = GameState.PLAYING;
        m_playerStats = m_player.PlayerStats;
      
        
        SpawnEnemy();
        SpawnBoss();
        if(m_playerStats == null||m_player==null) return; 
        GUIManager.Ins.ShowGameGUI(true);
        GUIManager.Ins.UpdateLifeInfo(m_curLife);
        GUIManager.Ins.UpdateCoinCounting(Prefs.coins);
        GUIManager.Ins.UpdateHpInfo(m_player.CurHp, m_playerStats.hp);
        GUIManager.Ins.UpdateLevelInfo(m_playerStats.level, m_playerStats.xp, m_playerStats.xpToLevelUp);
    }

    private void SpawnEnemy()
    {
        var randomEnemy = GetRandomEnemy();
        if (randomEnemy==null||m_map==null)return;
        StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
            
    
    }
    private void SpawnBoss()
    {
        var randomEnemy = GetRandomBoss();
        if (randomEnemy == null || m_map == null) return;
        StartCoroutine(SpawnBoss_Coroutine(randomEnemy));


    }
    private Enemy GetRandomEnemy()
    {
       if(m_enemyPrefab==null||m_enemyPrefab.Length<=0) return null;    
       int randomIdx=UnityEngine.Random.Range(0,m_enemyPrefab.Length);
        return m_enemyPrefab[randomIdx];
    }
    private Enemy GetRandomBoss()
    {
        if (m_bossPrefab == null || m_bossPrefab.Length <= 0) return null;
        int randomIdx = UnityEngine.Random.Range(0, m_bossPrefab.Length);
        return m_bossPrefab[randomIdx];
    }
    private IEnumerator SpawnEnemy_Coroutine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3f);
       
        while (state==GameState.PLAYING) {
            if (m_map.randomAiSpawnPoint == null) break;
            Vector3 spawnPoint=m_map.randomAiSpawnPoint.position;
            if (m_enemySpawnVfx)
                Instantiate(m_enemySpawnVfx, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(randomEnemy, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(m_enemySpawnTime);

        }
      yield  return null;
    }
    private IEnumerator SpawnBoss_Coroutine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3f);

        while (state == GameState.PLAYING)
        {
            if (m_map.randomAiSpawnPoint == null) break;
            Vector3 spawnPoint = m_map.randomAiSpawnPoint.position;
            if (m_bossSpawnVfx)
                Instantiate(m_bossSpawnVfx, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(randomEnemy, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(m_bossSpawnTime);

        }
        yield return null;
    }
    public void GameoverChecking(Action OnLostLife=null,Action OnDead = null)
    {
        if(m_curLife<=0)return;
        m_curLife--;
        OnLostLife?.Invoke();
        if (m_curLife <= 0)
        {
            state = GameState.GAMEOVER;
            OnDead?.Invoke();
            Debug.Log("Gameover");
        }
    }
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
