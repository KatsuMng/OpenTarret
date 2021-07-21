using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Enemy
{
    public class InvaderCounter : MonoBehaviour
    {
        GameObject gameManager;
        InvaderGameStateManager InvaderGameStateManager;
        InvaderMoveCommander invaderMoveCommander;
        float maxInvaderNum;
        public int invaderNum = 0;
        // Start is called before the first frame update
        void Start()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.Find("GameManager");
            }
            InvaderGameStateManager = gameManager.GetComponent<InvaderGameStateManager>();
            invaderMoveCommander = GetComponent<InvaderMoveCommander>();
        }

        public void CountInvader(int num)
        {
            invaderMoveCommander.InvaderSpeedCalculate(invaderNum / maxInvaderNum);
            invaderNum += num;
            IsCompleteDestruction();
        }

        public int GetInvaderNum()
        {
            return invaderNum;
        }

        public void InvaderCountZero()
        {
            invaderNum = 0;
        }

        public void SetMaxInvaderNum()
        {
            maxInvaderNum = invaderNum;
        }

        /// <summary>
        /// 敵が全滅したかどうか判定し、全滅した場合ゲームを勝ちで終了する
        /// </summary>
        void IsCompleteDestruction()
        {
            if (invaderNum <= 0)
            {
                InvaderGameStateManager.FinishGame(true);
            }
        }
    }
}