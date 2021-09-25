﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tarret
{
    /// <summary>
    /// タレットの仰角、底の回転を処理するクラス
    /// </summary>
    public class TarretRotater : MonoBehaviour
    {
        /// <summary>
        /// タレットの根本部分。ここを中心に横回転をする
        /// </summary>
        [SerializeField] GameObject rootPos;
        /// <summary>
        /// タレットの縦回転をする関節
        /// </summary>
        [SerializeField] GameObject muzzleFlameJointPos;
        [SerializeField] GameObject m_leftHandlePos;
        [SerializeField] GameObject m_rightHandlePos;
        /// <summary>
        /// タレットの向きを決定する矢印
        /// </summary>
        [SerializeField] GameObject m_arrowMark;

        [SerializeField] GameObject _anglePoint;
        AnglePointer anglePointer;

        [SerializeField] float maxVerticalAngle = 0.3f;
        [SerializeField] float minVerticalAngle = -0.2f;

        [SerializeField] float maxHorizontalAngle = 0.5f;
        [SerializeField] float minHorizontalAngle = -0.5f;
        /// <summary>
        /// あそびの調整
        /// </summary>
        [SerializeField] float _coodinaiteHorzontalPlay = 0.02f;
        [SerializeField] float _coodinaiteVerticalPlay = 0.02f;
        /// <summary>
        /// あそびの間隔
        /// </summary>
        [SerializeField] float _coodinatePlay = 0.001f;
        float _sqrtcoodinatePlay;
        float anglePointMoveDistance;
        float sqrtAnglePointMoveDistance;
        Vector2 anglePointPosi;

        TarretStateManager tarretStateManager;

        [SerializeField] float debugHorizontalRotate = 0.8f;
        [SerializeField] float debugVerticalRotate = 0.3f;

        [SerializeField] float rotateSpeed = 1.0f;

        [SerializeField] bool editRotateMode = false;

        private void Start()
        {
            tarretStateManager = GetComponent<TarretStateManager>();
            anglePointer = _anglePoint.GetComponent<AnglePointer>();
            _sqrtcoodinatePlay = Mathf.Sqrt(_coodinatePlay);
        }

        void FixedUpdate()
        {
            MoveManager();
        }

        /// <summary>
        /// tarretの動きを実行する命令を飛ばす関数
        /// </summary>
        void MoveManager()
        {
            switch (tarretStateManager.tarretCommandState)
            {
                case TarretState.Idle:
                    tarretStateManager.JudgeRotateTarret();
                    break;

                case TarretState.Attack:
                    break;

                case TarretState.Rotate:
                    anglePointMoveDistance = anglePointer.AnglePointMoveDistance;
                    if (anglePointMoveDistance > _coodinatePlay)
                    {
                        sqrtAnglePointMoveDistance = Mathf.Sqrt(anglePointMoveDistance);
                        anglePointPosi = anglePointer.AnglePointPosi;
                        HorizontalRotate();
                        VerticalRotate();
                    }

                    break;

                case TarretState.Break:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 横回転を制御する関数
        /// </summary>
        void HorizontalRotate()
        {
            if (rootPos.transform.localRotation.y > maxHorizontalAngle)
            {
                if (m_arrowMark.transform.localRotation.y < -_coodinaiteHorzontalPlay)
                {
                    HRotate();
                }
            }
            else if (rootPos.transform.localRotation.y < minHorizontalAngle)
            {
                if (m_arrowMark.transform.localRotation.y > _coodinaiteHorzontalPlay)
                {
                    HRotate();
                }
            }
            else
            {
                //if (m_arrowMark.transform.localRotation.y < -_coodinaiteHorzontalPlay)
                //{
                //    HRotate(-_coodinaiteHorzontalPlay);
                //}
                //else if (m_arrowMark.transform.localRotation.y > _coodinaiteHorzontalPlay)
                //{
                //    HRotate(_coodinaiteHorzontalPlay);
                //}
                HRotate();
            }
        }

        void HRotate()
        {
            rootPos.transform.Rotate(new Vector3(0, 90, 0) * rotateSpeed * Time.deltaTime
                 * (_anglePoint.transform.localPosition.x - (anglePointPosi.x * _sqrtcoodinatePlay / sqrtAnglePointMoveDistance)));
        }

        /// <summary>
        /// 縦回転を制御する関数
        /// </summary>
        void VerticalRotate()
        {
            //Debug.Log("muzzleFlameJointPos localRotation.x " + muzzleFlameJointPos.transform.localRotation.x);
            if (muzzleFlameJointPos.transform.localRotation.x > maxVerticalAngle)
            {
                if (m_arrowMark.transform.localRotation.x < -_coodinaiteVerticalPlay)
                {
                    VRotate();
                }
            }
            else if (muzzleFlameJointPos.transform.localRotation.x < minVerticalAngle)
            {
                if (m_arrowMark.transform.localRotation.x > _coodinaiteVerticalPlay)
                {
                    VRotate();
                }
            }
            else
            {
                //if (m_arrowMark.transform.localRotation.x < -_coodinaiteVerticalPlay)
                //{
                //    VRotate(-_coodinaiteVerticalPlay);
                //}
                //else if(m_arrowMark.transform.localRotation.x > _coodinaiteVerticalPlay)
                //{
                //    VRotate(_coodinaiteVerticalPlay);
                //}
                VRotate();
            }
        }

        void VRotate()
        {
            muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * rotateSpeed * Time.deltaTime
                 * (-_anglePoint.transform.localPosition.y - (-anglePointPosi.y * _sqrtcoodinatePlay / sqrtAnglePointMoveDistance)));
        }
        #region
#if UNITY_EDITOR
        void Update()
        {
            if (editRotateMode)
            {
                float dx = Input.GetAxis("Horizontal");
                float dy = Input.GetAxis("Vertical");

                DebugHorizontalRotate(dx);
                DebugVerticalRotate(dy);

            }

        }

        void DebugHorizontalRotate(float dx)
        {
            rootPos.transform.Rotate(new Vector3(0, 90, 0) * dx * debugHorizontalRotate * Time.deltaTime);
        }

        void DebugVerticalRotate(float dy)
        {
            if (muzzleFlameJointPos.transform.localRotation.x > maxVerticalAngle)
            {
                if (dy < 0)
                {
                    muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * dy * debugVerticalRotate * Time.deltaTime);
                }
            }
            else if (muzzleFlameJointPos.transform.localRotation.x < minVerticalAngle)
            {
                if (dy > 0)
                {
                    muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * dy * debugVerticalRotate * Time.deltaTime);
                }
            }
            else
            {
                muzzleFlameJointPos.transform.Rotate(new Vector3(90, 0, 0) * dy * debugVerticalRotate * Time.deltaTime);
            }
        }
#endif
        #endregion
    }
}