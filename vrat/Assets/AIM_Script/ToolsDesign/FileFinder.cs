using UnityEngine;

namespace vrat
{

    public class FileFinder : MonoBehaviour
    {

        protected string m_textPath;

        protected FileBrowser m_fileBrowser;

        [SerializeField]
        protected Texture2D m_directoryImage,
                            m_fileImage;

        public Texture2D texture;
        public GameObject m_goCube;

        void Start()
        {
            m_goCube = GameObject.Find("Cube");
        }

        protected void OnGUI()
        {
            if (m_fileBrowser != null)
            {
                m_fileBrowser.OnGUI();
            }
            else
            {
                OnGUIMain();
            }
        }

        protected void OnGUIMain()
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Text File", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            GUILayout.Label(m_textPath ?? "none selected");
            if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
            {
                m_fileBrowser = new FileBrowser(
                    new Rect(10, 10, 1000, 500),
                    "Choose Text File",
                    FileSelectedCallback
                );
                m_fileBrowser.SelectionPattern = "*.png";
                //m_fileBrowser.SelectionPattern = "*.txt";
                m_fileBrowser.DirectoryImage = m_directoryImage;
                m_fileBrowser.FileImage = m_fileImage;
            }
            GUILayout.EndHorizontal();
        }

        //실제 file이 선택될 때의 callback 함수

        protected void FileSelectedCallback(string path)
        {
            m_fileBrowser = null;
            /*
            if (path.Length != 0)
            {
                WWW www = new WWW("file://" + path);
                texture = new Texture2D(64, 64);
                www.LoadImageIntoTexture(texture);
                m_goCube.renderer.material.mainTexture = texture;
            }
            */
            m_textPath = path;
        }
    }
}