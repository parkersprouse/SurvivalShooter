using UnityEngine;
using System.Collections;

public static class PathUtility {

    public static string GetInputSaveFolder(int example) {
        return string.Format("{0}/{1}", Application.persistentDataPath, example);
    }

}