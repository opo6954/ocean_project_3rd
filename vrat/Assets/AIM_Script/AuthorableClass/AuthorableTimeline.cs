using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
     * timeline을 관리하는 authorable template임
     * 
     * timeline의 구성 
     * 
     * Player명
     * LIstOfXml with Event
     * 
     * 일단 event가 XmlTemplate이 아닌 ObjectTemplate이기 떄문에 따로 serialize와 deserialize를 만들어야 할 듯...
     * 
     * */
    public class AuthorableTimeline : ObjectTemplate
    {
        //event list임
        public List<AuthorableEvent> eventList = new List<AuthorableEvent>();

        public override void initialize()
        {
            base.initialize();
            ObjectType = OBJTYPE.TIMELINE;
            initForTimeline();
        }



        void initForTimeline()
        {
            variableContainer.addParameter(new PrimitiveXmlTemplate("Player", "string"));
        }

        public void example()
        {
            (variableContainer.getParameters("Player") as PrimitiveXmlTemplate).setparameter("Sailor");
            //event 일단 불러오기 xml로부터...
            AuthorableEvent ee = new AuthorableEvent();
            ee.initialize();
            ee.testDeserialize("MyEvent.event");

            AuthorableEvent ee2 = new AuthorableEvent();
            ee2.initialize();
            ee2.testDeserialize("tmpEvent.event");

            eventList.Add(ee);
            eventList.Add(ee2);
        }
        //파일 이름으로부터 xml serailize로 따로 저장함
        public override void testSerialize(string xmlName)
        {
            XmlDocument document = new XmlDocument();

            serialize2Xml(document, null, true);

            document.Save(xmlName);
            Debug.Log("Save xml with " + xmlName);
        }
        //내부 serialize 함수임
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

            //이 부분에서 event에 대한 serialize를 수행하면 됩니다

            //event list에 대해서 차례대로 serialize를 수행하면 됨
            //그 전에 eventList node를 만들어 놔야 할 듯

            XmlElement eventListNode = document.CreateElement("EventList");
            root.AppendChild(eventListNode);

            eventListNode.SetAttribute("name", "EventList");

            foreach (AuthorableEvent ae in eventList)
            {
                ae.serialize2Xml(document, eventListNode, false);
            }

            return true;

        }

        public override void testDeserialize(string fileName)
        {
            


            base.testDeserialize(fileName);
        }

        public override bool deserializeFromXml(XmlNode rootNode)
        {
            variableContainer.clearVariableAll();
            eventList.Clear();

            if(rootNode.Name != ObjectType.ToString())
            {
                Debug.Log("wrong with timeline file... root name is different...");
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
                else if (xn.Name == "EventList")
                {
                    XmlNodeList xnl = xn.ChildNodes;

                    foreach (XmlNode tp in xnl)
                    {
                        AuthorableEvent ae = new AuthorableEvent();
                        ae.initialize();
                        ae.deserializeFromXml(tp);
                        eventList.Add(ae);
                    }
                }
            }

            

            return true;
            

            //eventList 읽자
            

        }



    }

}