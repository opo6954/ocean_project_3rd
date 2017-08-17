using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Approach to asset within direction and distance
 * */
namespace vrat
{
    public class ApproachTrigger : TriggerTemplate
    {

        //player의 위치 정보
        Transform playerPosition;

        float distanceTh;
        float directionTh;

        bool isSetPlayer = false;


        public override void initialize()
        {
            //파라미터 개수: 2개; 방향 thresholding, 거리 thresholing
            //현재 trigger에 필요한 모든 파라미터 세팅을 constructor에서 수행한다

            /*
            parameterNames[0] = "distanceTh";
            parameterNames[1] = "directionTh";
             * */
        }

        



        public void setMyPlayer(string roleInfo)
        {
            playerPosition = RoleInfoTemplate.getPlayerWithRoleInfo(roleInfo).gameObject.transform;
            isSetPlayer = true;
        }

        public override bool triggerSupervisor()
        {
            /*
            if (isSetPlayer == false)
            {
                Debug.Log("No player found...");
                return false;
            }

            if (myAsset == null)
            {
                Debug.Log("No asset attached found...");
                return false;
            }

            distanceTh = float.Parse(myAsset.getParameter(parameterNames[0]));
            directionTh = float.Parse(myAsset.getParameter(parameterNames[1]));

            float distance = (playerPosition.position - myAsset.transform.position).magnitude;

            float angle = Vector3.Dot((playerPosition.position - myAsset.transform.position).normalized, Camera.main.transform.forward.normalized);

            if (distance < distanceTh && angle < directionTh)
            {
                return true;
            }
             * */
            return false;
        }
    }

}