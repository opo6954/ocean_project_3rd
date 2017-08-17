using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class AuthoringTest : MonoBehaviour {

        
    

	    // Use this for initialization
	    void Start () {
            AuthorableAsset aa;

            aa = new AuthorableAsset();

            AuthorableAsset bb;

            bb = new AuthorableAsset();

            
            aa.initialize();
            aa.exampleSerialize();
            aa.testSerialize("powerover.asset");
            

            bb.initialize();
            
            bb.testDeserialize("powerover.asset");

            bb.testSerialize("kiki.asset");


	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}