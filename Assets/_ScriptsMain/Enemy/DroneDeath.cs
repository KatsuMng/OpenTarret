﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneDeath : MonoBehaviour, IEnemyDeath
{
    [SerializeField] float deathTime = 0.5f;
    [SerializeField] int addScore = 100;
    [SerializeField] GameObject[] muzzle;
    [Inject]
    ISpawnable spawner;

    public void OnDead()
    {
        spawner.ChangeEnemyNum(-1); //敵のカウントを1減らす
        AddScore();
        Destroy(gameObject, deathTime);
    }

    public void BulletDead()
    {
        foreach (var item in muzzle)
        {
            if (item == null) continue;
            item.GetComponent<EnemyBulletManager>().OnDead();
        }
    }

    public void AddScore()
    {
        ScoreManager.Instance.AddScore(addScore);
    }
}