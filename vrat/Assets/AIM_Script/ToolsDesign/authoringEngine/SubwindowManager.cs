using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace vrat
{
    /*
     * 디버깅하세요
     * 
     * 지금 문제 생김...
     * 170821 해결
     * */

    public class SubwindowManager : MonoBehaviour
    {

        public void displayStatus(string txt)
        {
            gameObject.SetActive(true);
            StartCoroutine(OnDisplayStatus(txt));
        }

        public void displaySaveStatus(string fileName)
        {
            gameObject.SetActive(true);
            StartCoroutine(OnDisplaySaveStatus(fileName));
        }

        void setComponentActive(bool isOn)
        {
            RawImage[] rawimages = GetComponentsInChildren<UnityEngine.UI.RawImage>();
            Text[] texts = GetComponentsInChildren<UnityEngine.UI.Text>();
            Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
            Image[] images = GetComponentsInChildren<UnityEngine.UI.Image>();

            foreach(RawImage r in rawimages)
            {
                r.enabled = isOn;
            }

            foreach (Text r in texts)
            {
                r.enabled = isOn;
            }

            foreach (Button r in buttons)
            {
                r.enabled = isOn;
            }

            foreach (Image r in images)
            {
                r.enabled = isOn;
            }

        }

        IEnumerator OnDisplayStatus(string txt)
        {
            setComponentActive(true);            
            transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = txt;
            transform.Find("InputField").gameObject.SetActive(false);
            transform.Find("OKButton").gameObject.SetActive(false);
            transform.Find("CancelButton").gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Input file name";
            transform.Find("InputField").gameObject.SetActive(true);
            transform.Find("OKButton").gameObject.SetActive(true);
            transform.Find("CancelButton").gameObject.SetActive(true);
            setComponentActive(false);
        }

        IEnumerator OnDisplaySaveStatus(string fileName)
        {

            setComponentActive(true);
            fileName = fileName.Replace(DirectoryUIHandler.assetSavePath, "");
            
            transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Save <" + fileName + "> complete.";
            transform.Find("InputField").gameObject.SetActive(false);
            transform.Find("OKButton").gameObject.SetActive(false);
            transform.Find("CancelButton").gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Input file name";
            transform.Find("InputField").gameObject.SetActive(true);
            transform.Find("OKButton").gameObject.SetActive(true);
            transform.Find("CancelButton").gameObject.SetActive(true);
            setComponentActive(false);
        }
   
    }
}