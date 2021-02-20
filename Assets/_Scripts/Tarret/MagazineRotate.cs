﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineRotate : MonoBehaviour
{
    [SerializeField] GameObject baseTarret;
    BaseTarretAttack tarretAttack;
    Animator magazineAnim;
    
    
    // Start is called before the first frame update
    void Start()
    {
        tarretAttack = baseTarret.GetComponent<BaseTarretAttack>();
        magazineAnim = GetComponent<Animator>();
    }


    public void RotateMagazine()
    {
        magazineAnim.SetTrigger("RotateMagezine");
    }

    //void AnimationComplete()
    //{
    //    tarretAttack.EndAttack();
    //}
}
