using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Modules;
using UnityEditor;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public class K_BundleModuleConfigWindow : OdinEditorWindow
    {
        [PropertySpace(5, 5)]
        [Required("Please input module name")]
        //[GUIColor(0.3f,0.8f,0.8f,1f)]
        [LabelText("Module Name")]
        public string moduleName;

        [ReadOnly] [HideLabel] [DisplayAsString] [TabGroup("Prefab Bundle")]
        public string prefabTab = "This Folder all Prefab will be build to AssetBundle by single Bundle";

        [ReadOnly] [HideLabel] [DisplayAsString] [TabGroup("Folder Bundle")]
        public string rootFolderSubBundle = "This Folder all SubFolder will be build to AssetBundle by single Bundle";

        [ReadOnly] [HideLabel] [DisplayAsString] [TabGroup("Single Bundle")]
        public string singleBundle = "This File will be build a single AssetBundle";

        [HideLabel] [FolderPath] [LabelText("Prefabs Asset Path")] [TabGroup("Prefab Bundle")]
        public string[] prefabPathArr = new string[] { "Path..." };

        [HideLabel] [FolderPath] [LabelText("Folder Path")] [TabGroup("Folder Bundle")]
        public string[] rootFolderPathArr = new string[] { };

        [HideLabel] [LabelText("File Path")] [TabGroup("Single Bundle")]
        public BundleFileInfo[] singlePathArr = new BundleFileInfo[] { };

        public static void ShowWindow(string moduleName)
        {
            //

            K_BundleModuleConfigWindow window = GetWindowWithRect<K_BundleModuleConfigWindow>(new Rect(0, 0, 600, 600));
            window.Show();
            //update window data
            K_BundleMoudleData moudleData = K_BuildBundleConfig.Instance.GetBundleDataByName(moduleName);
            if (moudleData != null)
            {
                window.moduleName = moudleData.moduleNmae;
                window.prefabPathArr = moudleData.prefabPathArr;
                window.rootFolderPathArr = moudleData.rootFolderPathArr;
                window.singlePathArr = moudleData.singlePathArr;
            }
        }

        [OnInspectorGUI]
        public void DrawSaveConfigButton()
        {
            GUILayout.BeginArea(new Rect(0, 510, 600, 200));
            if (GUILayout.Button("DeleteConfig", GUILayout.Height(47)))
            {
                DeleteConfig();
            }

            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(0, 555, 600, 200));
            if (GUILayout.Button("SaveConfig", GUILayout.Height(47)))
            {
                SaveConfig();
            }

            GUILayout.EndArea();
        }

        /// <summary>
        /// Delete Config
        /// </summary>
        public void DeleteConfig()
        {
            K_BuildBundleConfig.Instance.RemoveModuleByName(moduleName);
            EditorUtility.DisplayDialog("Delete Config", "Delete Config Success", "OK");
            Close();
            K_BuildWindow.ShowBuildWindow();
        }

        /// <summary>
        /// Save Config
        /// </summary>
        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                EditorUtility.DisplayDialog("Save Config", "Please input module name", "OK");
                return;
            }

            K_BundleMoudleData moduleData = K_BuildBundleConfig.Instance.GetBundleDataByName(moduleName);
            if (moduleData == null)
            {
                //Add New Module
                moduleData = new K_BundleMoudleData();
                moduleData.moduleNmae = this.moduleName;
                moduleData.prefabPathArr = this.prefabPathArr;
                moduleData.rootFolderPathArr = this.rootFolderPathArr;
                moduleData.singlePathArr = this.singlePathArr;
                K_BuildBundleConfig.Instance.assetBundleConfig.Add(moduleData);
                AssetDatabase.CreateAsset(moduleData, "Assets/AssetBundleBuilder/Config/" + moduleName + ".asset");
                AssetDatabase.SaveAssets();
            }
            else
            {
                //Update Module
                moduleData.prefabPathArr = this.prefabPathArr;
                moduleData.rootFolderPathArr = this.rootFolderPathArr;
                moduleData.singlePathArr = this.singlePathArr;
            }

            EditorUtility.DisplayDialog("Save Config", "Save Config Success", "OK");
            Close();
            K_BuildWindow.ShowBuildWindow();
        }
    }
}