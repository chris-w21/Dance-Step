using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class REloader : MonoBehaviour
{
    public bool reload = false;

    private void OnValidate()
    {
        if (reload)
        {
            reload = false;
            EditorUtility.RequestScriptReload();
        }
    }
}
