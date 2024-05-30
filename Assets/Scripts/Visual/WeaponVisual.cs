using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] AudioClip m_shootingSoound;
    [SerializeField] AudioClip m_reloadSound;
    private Weapon m_weapon;
    private void Awake()
    {
        m_weapon = GetComponent<Weapon>();
    }
    public void OnShoot()
    {
        AudioController.Ins.PlaySound(m_shootingSoound);
        CineController.Ins.ShakeTrigger();
    }
    public  void OnReload()
    {
        GUIManager.Ins.ShowReloadTxt(true);
    }
    public void OnReloadDone()
    {
        AudioController.Ins.PlaySound(m_reloadSound);
        GUIManager.Ins.ShowReloadTxt(false);
    }
}
