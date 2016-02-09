using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class SaveInput : MonoBehaviour {
    [SerializeField]
    private int m_ID;

    public void Save() {
        string saveFolder = PathUtility.GetInputSaveFolder(m_ID);
        if (!System.IO.Directory.Exists(saveFolder))
            System.IO.Directory.CreateDirectory(saveFolder);

        InputSaverXML saver = new InputSaverXML(saveFolder + "/input_config.xml");
        InputManager.Save(saver);
    }
}