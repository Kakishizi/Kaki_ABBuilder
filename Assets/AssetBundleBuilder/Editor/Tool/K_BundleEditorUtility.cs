using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public class K_BundleEditorUtility
    {
        public static GUIStyle GetGUIStyle(string styleName)
        {
            GUIStyle style = null;
            foreach (var VARIABLE in GUI.skin.customStyles)
            {
                if (string.Equals(VARIABLE.name.ToLower(), styleName.ToLower()))
                {
                    style = VARIABLE;
                    break;
                }
            }

            return style;
        }
    }
}