using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace vrat
{
    /*
     * node line을 관리하는 녀석
     * */

    public enum NODETYPE
    {
        IN_NODE, OUT_NODE
    }

    public class NodeLineManager : MonoBehaviour
    {

        //이 node의 line renderer임
        [SerializeField]
        LineRenderer lineRenderer;

        //현재 이 node의 type, in과 out으로 나눠짐
        [SerializeField]
        NODETYPE _myNodeType;

        //연결된 eventUIManager임
        [SerializeField]
        eventUIManager attachedEventUIManager;

        //반대편 node line manager를 가지고 있자
        [SerializeField]
        NodeLineManager oppositeNodeLineManager;

        [SerializeField]
        bool isStart = false;


        //in node와 out node가 연결되었는지 안되었는지 확인함
        bool isInNodeConnect = false;
        bool isOutNodeConnect = false;

        

        public static bool onConnecting = false;

        //from node의 lineManager를 global로 가지고 있자
        public static NodeLineManager onFromLineManager;
        

        public bool isPressedInNode = false;

        public int fromIdx=-1;
        public int toIdx=-1;



        //node 부분을 누를 경우
        public void OnClickNode()
        {
            //현재 연결중인지 확인해야 함
            //onConnecting인지 확인해야 함

            //처음 눌리는 경우
            if (NodeLineManager.onConnecting == false)
            {
                if (_myNodeType == NODETYPE.OUT_NODE)
                {
                    NodeLineManager.onConnecting = true;
                    NodeLineManager.onFromLineManager = this;
                    isPressedInNode = true;
                }
            }
                //이미 다른 node에서 눌려진 경우, 이 node은 끝 node가 되겠지?
            else if (NodeLineManager.onConnecting == true)
            {
                //본인의 end node를 누르진 않았는지 확인해야함
                if (oppositeNodeLineManager.isPressedInNode == false)
                {
                    if (_myNodeType == NODETYPE.IN_NODE)
                    {
                        //여기서 from에 대한 callback을 보내야 되는데
                        OnUpdateConnect();
                        NodeLineManager.onConnecting = false;
                        isPressedInNode = false;
                        NodeLineManager.onFromLineManager.isPressedInNode = false;

                        if (NodeLineManager.onFromLineManager.isStart == false)
                            fromIdx = NodeLineManager.onFromLineManager.attachedEventUIManager.idx;
                        else
                            fromIdx = -1;

                        NodeLineManager.onFromLineManager.toIdx = attachedEventUIManager.idx;

                        //이전 시작점의 to idx 저장하기 요래야 나중에 편할 듯
                        NodeLineManager.onFromLineManager.attachedEventUIManager.setToIdx(attachedEventUIManager.idx);

                        //끝나는 점의 from idx 저장하기
                        attachedEventUIManager.setFromIdx(fromIdx);


                        
                        


                        NodeLineManager.onFromLineManager = null;
                    }
                }
                
                
            }
        }

        //일단 대충 Connect update함수임
        public void OnUpdateConnect()
        {
            Debug.Log("It is end node..");
        }

        void Update()
        {
            if (NodeLineManager.onConnecting == true && isPressedInNode == true)
            {
                //이제 제대로 mouse position을 hooking해야 함

                Vector2 mousePosOnTimeEditor = new Vector2();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out mousePosOnTimeEditor);

                Vector3 linePos = new Vector3(mousePosOnTimeEditor.x, mousePosOnTimeEditor.y);

                lineRenderer.SetPosition(1, linePos);
            }
                
        }

    }
}
