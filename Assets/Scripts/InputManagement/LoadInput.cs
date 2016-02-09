using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class LoadInput : MonoBehaviour {
    [SerializeField]
    private int m_ID;

    private void Awake() {
        string savePath = PathUtility.GetInputSaveFolder(m_ID) + "/input_config.xml";
        if (System.IO.File.Exists(savePath)) {
            InputLoaderXML loader = new InputLoaderXML(savePath);
            InputManager.Load(loader);
        }
    }
}