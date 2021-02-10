﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Managers;
using UniRx;
using UniRx.Triggers;

public class TarretAttack : MonoBehaviour
{
    [SerializeField] float fireInterval = 2.0f;
    //[SerializeField] Vector3 rayDirection;
    [SerializeField] float rayDistance = 1;
    [SerializeField] GameObject rayOfOrigin;

    [SerializeField] GameObject m_razerEffect;
    [SerializeField] GameObject m_wasteHeatEffect;
    [SerializeField] GameObject m_shockWaveEffect;

    [SerializeField] GameObject m_razerEffectInsPosi;
    [SerializeField] GameObject m_wasteHeatEffectInsPosi;
    [SerializeField] GameObject m_shockWaveEffectInsPosi;

    [SerializeField] float razerExistTime = 0.5f;
    [SerializeField] float wasteHeatExistTime = 2.0f;
    [SerializeField] float shockWaveExistTime = 1.0f;
    //[SerializeField] Vector3 maxShockWaveSize;
    //[SerializeField] int shockWaveInsValue = 1;
    //[SerializeField] float shockWaveInsInterval = 0.1f;

    public bool attackable = true;

    GameObject m_razer;
    GameObject m_wasteHeat;
    GameObject m_shockWave;

    [SerializeField] Transform muzzle;
    float muzzleRadius;
    GameObject nearEnemy;


    [SerializeField] Gradient razerAfterColor;
    Ray m_ray;
    RaycastHit[] m_rayHits;
    public bool Killable = false;

    BaseTarretBrain baseTarretBrain;

    private void Start()
    {
        baseTarretBrain = GetComponent<BaseTarretBrain>();
        //　弾の半径を取得
        muzzleRadius = muzzle.GetComponent<SphereCollider>().radius;
    }

    void FixedUpdate()
    {
        //KillEnemyFromRazer();
        Debug.DrawLine(muzzle.position, muzzle.position + muzzle.transform.forward * rayDistance);
    }

    void KillEnemyFromRazer()
    {
        //　飛ばす位置と飛ばす方向を設定
        Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
        //　当たったコライダを入れておく変数
        RaycastHit[] hits;
        //　Sphereの形でレイを飛ばしEnemyレイヤーのものをhitsに入れる
        hits = Physics.SphereCastAll(ray, muzzleRadius, rayDistance, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            Destroy(hit.collider.gameObject);
        }
    }

    //レーザーのライン部分のスクリプト
    void FireEffectManager()
    {
        m_razer = Instantiate(m_razerEffect, m_razerEffectInsPosi.transform.position, m_razerEffectInsPosi.transform.rotation);
        LineRenderer razerLineRenderer = m_razer.transform.GetChild(2).gameObject.GetComponent<LineRenderer>();
        FadeFire(razerLineRenderer);

    }

    void FadeFire(LineRenderer razerRenderer)
    {
        Debug.Log("終わり!");

        Destroy(m_razer,razerExistTime);
        //float alphaValue = 255f;
        //DOTween.To(
        //    () => alphaValue,
        //    (x) =>
        //    {
        //        alphaValue = x;
        //        razerAfterColor.SetKeys
        //        (razerAfterColor.colorKeys, new GradientAlphaKey[] { new GradientAlphaKey(alphaValue, 0.0f), new GradientAlphaKey(alphaValue, 1.0f) });
        //    },
        //    0,
        //    razerExistTime)
        //    .OnComplete(() => EndCall());

        //razerRenderer.colorGradient.DOGradientColor(razerAfterColor, razerExistTime)
        //    .OnComplete(() => EndCall());

        //razerRenderer=DOTween.To(
        //    () => razerRenderer.colorGradient.SetKeys(razerAfterColor.colorKeys,razerAfterColor.alphaKeys) ,
        //    razerRenderer.colorGradient,
        //    razerAfterColor,
        //    razerExistTime)
        //    .SetEase(Ease.Linear);

        //DOTween.To(() => razerRenderer.colorGradient.alphaKeys[1].alpha, (x) => razerRenderer.colorGradient.alphaKeys[1].alpha = x, 0, razerExistTime)
        //    .SetEase(Ease.Linear)
        //    .OnComplete(() => { Debug.Log("razerRenderer.colorGradient.alphaKeys[0].alpha : " + razerRenderer.colorGradient.alphaKeys[0].alpha); EndCall(); });
    }

    //ここまでレーザーのライン部分のスクリプト

    /// <summary>
    /// 廃熱エフェクトの実体化から消滅までの管理をするメソッド
    /// </summary>
    void WasteHeatEffectManager()
    {
        m_wasteHeat = Instantiate(m_wasteHeatEffect, m_wasteHeatEffectInsPosi.transform.position,
            m_wasteHeatEffectInsPosi.transform.rotation, m_wasteHeatEffectInsPosi.transform);
        Destroy(m_wasteHeat, wasteHeatExistTime);
    }

    void ShockWaveManager()
    {
        m_shockWave = Instantiate(m_shockWaveEffect, m_shockWaveEffectInsPosi.transform.position,
            m_shockWaveEffectInsPosi.transform.rotation);

        Material shockWaveMT = m_shockWave.gameObject.GetComponent<Material>();


        Destroy(m_shockWave, shockWaveExistTime);


        //m_shockWaves = new GameObject[shockWaveInsValue];

        //m_shockWaves[0] = Instantiate(m_shockWaveEffect, m_shockWaveEffectInsPosi.transform.position,
        //    m_shockWaveEffectInsPosi.transform.rotation);
        //m_shockWaves[0].transform.DOScale(maxShockWaveSize, shockWaveExistTime).SetEase(Ease.Linear);
        //Destroy(m_shockWaves[0], shockWaveExistTime);

        //if (shockWaveInsValue >= 2)
        //{
        //    int i = 1;
        //    DOVirtual.DelayedCall(shockWaveInsInterval, () =>
        //    {
        //        m_shockWaves[i] = Instantiate(m_shockWaveEffect, m_shockWaveEffectInsPosi.transform.position,
        //    m_shockWaveEffectInsPosi.transform.rotation);
        //        m_shockWaves[i].transform.DOScale(maxShockWaveSize, shockWaveExistTime).SetEase(Ease.Linear);
        //        Destroy(m_shockWaves[i], shockWaveExistTime);
        //        i++;
        //    }).SetLoops(shockWaveInsValue - 1);
        //}
    }


    /// <summary>
    /// 攻撃したとき、TarretCommandステートがAttackになったときに一度だけ呼ばれるメソッド。
    /// </summary>
    public void BeginAttack()
    {
        FireEffectManager();
        WasteHeatEffectManager();
        ShockWaveManager();
        this.UpdateAsObservable()
            .Take(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => {
                KillEnemyFromRazer();
            });
        
        attackable = false;

        StayAttack();
    }

    void StayAttack()
    {
        Observable.Timer(TimeSpan.FromSeconds(fireInterval)).Subscribe(_ =>
        {
            EndAttack();
        }).AddTo(this);
    }

    void EndAttack()
    {
        attackable = true;
    }


#if UNITY_EDITOR

    private void Update()
    {
        
        if (Input.GetKeyDown("space"))
        {
            if (baseTarretBrain.tarretCommandState != TarretCommand.Attack)
            {
                baseTarretBrain.ChangeTarretState(TarretCommand.Attack);
            }
        }
        
    }
#endif
}