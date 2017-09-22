using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

namespace vrat
{
    public class Location
    {
        public Vector3 position;
        public Vector3 rotation;

        public Location(Vector3 _pos, Vector3 _rot)
        {
            position = _pos;
            rotation = _rot;
        }
    }

    public class LocationXmlTemplate : XmlTemplate
    {
        Location location;

        public Vector3 Position
        {
            get
            {
                return location.position;
            }
            set
            {
                location.position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return location.rotation;
            }
            set
            {
                location.rotation = value;
            }
        }

        public LocationXmlTemplate(string _name, string _type, Location _variable) : base(_name, _type)
        {
            location = new Location(_variable.position, _variable.rotation);
            ClassName = "LocationXmlTemplate";
        }

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            string propertyName = Name;
            string propertyType = Type;

            XmlElement individualProperty = document.CreateElement(this.GetType().ToString());
            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);

            parentElement.AppendChild(individualProperty);

            

            string[] p = new string[3]{Position.x.ToString(), Position.y.ToString(), Position.z.ToString()};
            string[] r = new string[3]{Rotation.x.ToString(),Rotation.y.ToString(),Rotation.z.ToString()};

            string[] axisName = new string[3]{"x","y","z"};

            //position에 대한 serialization
            XmlElement positionProperty = document.CreateElement("Position");
            individualProperty.AppendChild(positionProperty);

            for(int i=0; i<p.Length; i++)
            {
                positionProperty.AppendChild(document.CreateElement(axisName[i])).InnerText = p[i];
            }

            //roation에 대한 serialization
            XmlElement rotationProperty = document.CreateElement("Rotation");
            individualProperty.AppendChild(rotationProperty);            

            for (int i = 0; i < r.Length; i++)
            {
                rotationProperty.AppendChild(document.CreateElement(axisName[i])).InnerText = r[i];    
            }

            return individualProperty;
        }

        public Location getVariable()
        {
            return location;
        }

        //location으로 넣어줘야 함
        public bool setParameter(Location _variable)
        {
            Location _l = _variable;

            Position = _l.position;
            Rotation = _l.rotation;

            return true;
        }






    }
}
