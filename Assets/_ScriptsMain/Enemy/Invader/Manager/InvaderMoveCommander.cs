using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class InvaderMoveCommander : MonoBehaviour
    {
        List<InvaderMover> invaderMovers= new List<InvaderMover>();
        List<InvaderStateManager> invaderStateManager=new List<InvaderStateManager>();
        public int maxMovePeriodOfFrame = 90;
        public int minMovePeriodOfFrame = 30;

        public void SetInvaders(List<GameObject> invaders)
        {
            if (invaderMovers.Count > 0) return;
            for (int i = 0; i < invaders.Count; i++)
            {
                invaderMovers.Add(invaders[i].GetComponent<InvaderMover>());
                invaderStateManager.Add(invaders[i].GetComponent<InvaderStateManager>());
            }
        }

        public void LimitMove()
        {
            if (invaderMovers == null)
            {
                return;
            }
            foreach (var item in invaderMovers)
            {
                item.LimitMove();
            }
        }
         /// <summary>
         /// 進軍する命令を出す
         /// </summary>
        public void CommenceMarch()
        {
            if (invaderStateManager == null)
            {
                return;
            }
            foreach (var item in invaderStateManager)
            {
                item.ChangeInvaderState(InvaderState.March);
            }
        }

        /// <summary>
        /// 立ち止まっている命令を出す
        /// </summary>
        public void CommenceStandby()
        {
            if (invaderStateManager == null)
            {
                return;
            }
            foreach (var item in invaderStateManager)
            {
                item.ChangeInvaderState(InvaderState.Standby);
            }
        }

        public void CommenceChangeSpeed(int speed)
        {
            if (invaderMovers == null)
            {
                return;
            }
            foreach (var item in invaderMovers)
            {
                item.ChangeMoveSpeed(speed);
            }
        }

        public void InvaderSpeedCalculate(float invaderAlivePer)
        {
            float speed=Mathf.Lerp(minMovePeriodOfFrame, maxMovePeriodOfFrame, invaderAlivePer);
            CommenceChangeSpeed((int)speed);
        }
        #region
#if UNITY_EDITOR
        //int currentSpeed;
        //private void Start()
        //{
        //    currentSpeed = movePeriodOfFrame;
        //}
        //private void Update()
        //{
        //    if (currentSpeed != movePeriodOfFrame)
        //    {
        //        currentSpeed = movePeriodOfFrame;
        //        CommenceChangeSpeed(currentSpeed);
        //    }
        //}

#endif
        #endregion
    }
}