using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
/*
 * Scenario를 serialize하는 클래스
 * 최종적으로 모든 게 필요함
 * 필요한 녀석:
 * 
 * 기타 정보
 * player 이름 및 각 역할 정보
 * 
 * main 정보
 * room
 * asset
 * timeline
    */
    public class AuthorableScenario : ObjectTemplate
    {
        //scenario에 포함된 room list
        //일단 지금은 걍 room으로 놓자
        //추후에는 authorableEnv로 해서 in-situ나 room의 배치, teleport 등도 같이 넣어야 함 지금은 시간이 없으니 걍 room으로 넣자..
        public List<AuthorableRoom> roomList = new List<AuthorableRoom>();

        //scenario에 포함된 asset list
        public List<AuthorableAsset> assetList = new List<AuthorableAsset>();

        //scenario에 포함된 timeline list
        public List<AuthorableTimeline> timelineList = new List<AuthorableTimeline>();

        public override void initialize()
        {
            base.initialize();
            ObjectType = OBJTYPE.SCENARIO;

            initForScenario();
        }

        void initForScenario()
        {
            //본 훈련에 필요한 총 역할 list
            variableContainer.addParameter(new ListOfXmlTemplate("TraineeRoleInfo", "ListOfXmlTemplate",0));

            //본 훈련의 플랫폼 환경(AR/VR/Desktop)
            variableContainer.addParameter(new ListOfXmlTemplate("TrainCondition", "ListOfXmlTemplate", 0));
        }

        public void example()
        {

            ObjectName = "FireExtinguisherTraining";
            //역할 정보
            ListOfXmlTemplate lxtRole = variableContainer.getParameters("TraineeRoleInfo") as ListOfXmlTemplate;

            lxtRole.addList(new PrimitiveXmlTemplate("Role1", "Captain", "string"));
            lxtRole.addList(new PrimitiveXmlTemplate("Role2", "Sailor1", "string"));

            //훈련 환경 정보(VR/AR/Desktop)
            ListOfXmlTemplate lxtCondition = variableContainer.getParameters("TrainCondition") as ListOfXmlTemplate;

            lxtCondition.addList(new PrimitiveXmlTemplate("Platform1", "VR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform2", "AR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform3", "Desktop", "string"));

            //room 정보 입력
            AuthorableRoom ar = new AuthorableRoom();
            ar.initialize();
            ar.testDeserialize("Cruise.room");
            roomList.Add(ar);

            //asset 정보 입력
            AuthorableAsset a1 = new AuthorableAsset();
            a1.initialize();
            a1.testDeserialize("extinguisher.asset");

            AuthorableAsset a2 = new AuthorableAsset();
            a2.initialize();
            a2.testDeserialize("fire.asset");

            AuthorableAsset a3 = new AuthorableAsset();
            a3.initialize();
            a3.testDeserialize("phone.asset");

            assetList.Add(a1);
            assetList.Add(a2);
            assetList.Add(a3);

            //timeline 정보 입력 아마 차례대로 player가 가지는 timeline이 될 듯?

            AuthorableTimeline t1 = new AuthorableTimeline();
            t1.initialize();
            t1.testDeserialize("tmpTimeline.timeline");

            AuthorableTimeline t2 = new AuthorableTimeline();
            t2.initialize();
            t2.testDeserialize("tmpTimeline2.timeline");

            timelineList.Add(t1);
            timelineList.Add(t2);
        }
        public override void testSerialize(string xmlName)
        {
            base.testSerialize(xmlName);
        }
        public override void testDeserialize(string fileName)
        {
            variableContainer.clearVariableAll();

            roomList.Clear();
            assetList.Clear();
            timelineList.Clear();

            base.testDeserialize(fileName);



        }

        public override bool serialize2Xml(XmlDocument document, XmlElement rootElement, bool isRoot)
        {
            
            XmlElement root = document.CreateElement(ObjectType.ToString());
            root.SetAttribute("name", ObjectName);

            if (isRoot == true)
            {
                document.AppendChild(root);
            }
            else
            {
                rootElement.AppendChild(root);
            }

            XmlElement xe = serializeCustomizedProperty2Xml(document, root);

            //이 부분에서 room list, asset list, timeline list에 대한 serialize를 수행하면 됩니다.

            XmlElement roomListNode = document.CreateElement("RoomList");

            root.AppendChild(roomListNode);

            roomListNode.SetAttribute("name", "RoomList");

            foreach (AuthorableRoom ar in roomList)
            {
                ar.serialize2Xml(document, roomListNode, false);
            }

            XmlElement assetListNode = document.CreateElement("AssetList");

            root.AppendChild(assetListNode);

            assetListNode.SetAttribute("name", "AssetList");
            
            foreach (AuthorableAsset aa in assetList)
            {
                aa.serialize2Xml(document, assetListNode, false);
            }

            XmlElement timelineListNode = document.CreateElement("TimelineList");

            root.AppendChild(timelineListNode);

            timelineListNode.SetAttribute("name", "TimelineList");

            foreach (AuthorableTimeline at in timelineList)
            {
                at.serialize2Xml(document, timelineListNode, false);
            }
            return true;
        }

        public override bool deserializeFromXml(XmlNode rootNode)
        {
            if (rootNode.Name != ObjectType.ToString())
            {
                Debug.Log("wron with scenario file... root name is different");
                return false;
            }

            //object 이름 설정하기
            ObjectName = rootNode.Attributes["name"].InnerText;

            foreach (XmlNode xn in rootNode.ChildNodes)
            {
                
                if (xn.Name == "CustomizedProperty")
                {
                    deserializeChildEachInside(xn.ChildNodes);
                }
                else if (xn.Name == "RoomList")
                {
                    XmlNodeList xnl = xn.ChildNodes;

                    foreach (XmlNode tp in xnl)
                    {
                        AuthorableRoom ar = new AuthorableRoom();
                        ar.initialize();
                        ar.deserializeFromXml(tp);

                        roomList.Add(ar);
                    }
                }
                else if (xn.Name == "AssetList")
                {
                    XmlNodeList xnl = xn.ChildNodes;

                    foreach (XmlNode tp in xnl)
                    {
                        AuthorableAsset aa = new AuthorableAsset();
                        aa.initialize();
                        aa.deserializeFromXml(tp);

                        assetList.Add(aa);
                    }
                }
                else if (xn.Name == "TimelineList")
                {
                    XmlNodeList xnl = xn.ChildNodes;

                    foreach (XmlNode tp in xnl)
                    {
                        AuthorableTimeline at = new AuthorableTimeline();
                        at.initialize();
                        at.deserializeFromXml(tp);

                        timelineList.Add(at);
                    }
                }
            }
            return true;
        }
    }
}