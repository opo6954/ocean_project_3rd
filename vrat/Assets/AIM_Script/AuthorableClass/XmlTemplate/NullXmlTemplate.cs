using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class NullXmlTemplate : XmlTemplate
    {
        public NullXmlTemplate() : base("null", "null")
        {
        }

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            throw new System.NotImplementedException();
        }

    }
}