﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Players
{
    /// <summary>
    /// 実際にタレットを動かすクラス
    /// </summary>
    public class BaseTarretMove : MonoBehaviour
    {
        [SerializeField] GameObject rootPos;
        [SerializeField] GameObject muzzleFlameJointPos;
        [SerializeField] GameObject m_leftHandlePos;
        [SerializeField] GameObject m_rightHandlePos;

        [SerializeField] float maxMuzzleFlameJointRotate = 0.5f;
        [SerializeField] float minMuzzleFlameJointRotate = 0.3f;

        BaseTarretRotateFunction baseTarretControl;
        BaseTarretBrain baseTarretBrain;
        TarretAttack tarretAttack;

        private void Start()
        {
            baseTarretControl = GetComponent<BaseTarretRotateFunction>();
            baseTarretBrain = GetComponent<BaseTarretBrain>();
            tarretAttack = GetComponent<TarretAttack>();
        }

        void FixedUpdate()
        {

            //if (baseTarretBrain.tarretCommandState == TarretCommand.HorizontalRotate)
            //{
            //    HorizontalRotate();
            //}
            //else if (baseTarretBrain.tarretCommandState == TarretCommand.VerticalRotate)
            //{
            //    VerticalRotate();
            //}
            //else if (baseTarretBrain.tarretCommandState == TarretCommand.Attack)
            //{
            //}
            //else //baseTarretControl.tarretCommanfStateがIdleのステートの場合
            //{

            //}
            //baseTarretBrain.JudgeTarretCommandState();
            MoveManager();
        }

        void MoveManager()
        {
            switch (baseTarretBrain.tarretCommandState)
            {
                case TarretCommand.Idle:
                    baseTarretBrain.JudgeTarretCommandState();
                    break;
                case TarretCommand.HorizontalRotate:
                    HorizontalRotate();
                    baseTarretBrain.JudgeTarretCommandState();
                    break;
                case TarretCommand.VerticalRotate:
                    VerticalRotate();
                    baseTarretBrain.JudgeTarretCommandState();
                    break;
                case TarretCommand.Attack:
                    break;
                default:
                    break;
            }
        }

        void HorizontalRotate()
        {
            rootPos.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime * baseTarretControl
                    .SetHorizontalRotateSpeed(m_leftHandlePos.transform, m_rightHandlePos.transform));
        }

        void VerticalRotate()
        {
            Debug.Log("muzzleFlameJointPos localRotation.x " + muzzleFlameJointPos.transform.localRotation.x);
            if (muzzleFlameJointPos.transform.localRotation.x > maxMuzzleFlameJointRotate)
            {
                //Debug.Log("leftHandle.HandleRotatePer:" + baseTarretBrain.leftHandle.HandleRotatePer);
                //Debug.Log("rightHandle.HandleRotatePer:" + baseTarretBrain.rightHandle.HandleRotatePer);
                if (baseTarretBrain.leftHandle.HandleRotatePer < 0.0f) //回転の限界値を超えるときは逆回転の操作しか受け付けない
                {
                    muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * Time.deltaTime * baseTarretControl
                .SetVerticalRotateSpeed(m_leftHandlePos.transform, m_rightHandlePos.transform));
                }

            }
            else if(muzzleFlameJointPos.transform.localRotation.x < minMuzzleFlameJointRotate)
            {
                if (baseTarretBrain.leftHandle.HandleRotatePer > 0.0f)
                {
                    muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * Time.deltaTime * baseTarretControl
                    .SetVerticalRotateSpeed(m_leftHandlePos.transform, m_rightHandlePos.transform));
                }
            }
            else
            {
                muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * Time.deltaTime * baseTarretControl
                    .SetVerticalRotateSpeed(m_leftHandlePos.transform, m_rightHandlePos.transform));
            }
        }
    }
}