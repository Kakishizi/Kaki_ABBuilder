using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public class KBuildKBundleWindow : K_BundleConfigBase
    {
        protected string[] buildButtonsName = new string[]
            { "Build AssetBundle", "Build AssetBundle And Copy To StreamingAssets" };

        public static void FreshWindow()
        {
        }

        public override void Inization()
        {
            base.Inization();
        }

        public override void OnGui()
        {
            base.OnGui();
        }

        public override void DrawAddModuleButtons()
        {
           
        }

        public override void DrawBuildButtons()
        {
      
        }

        [Button("Reload", 30)]
        public void Reload()
        {
            K_BuildBundleConfig.Instance.assetBundleConfig.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject",
                new[] { "Assets/AssetBundleBuilder/Config" });
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<K_BundleMoudleData>(path);
                if (asset != null)
                {
                    K_BuildBundleConfig.Instance.assetBundleConfig.Add(asset);
                }
            }
            K_BuildWindow.ShowBuildWindow();
        }

        [Button("Build AssetBundle", 30)]
        public override void BuildBundle()
        {
            base.BuildBundle();
            Debug.Log("Build AssetBundle");
            foreach (var VARIABLE in moudleDataList)
            {
                if (VARIABLE.isBuild)
                {
                    //TODO:打包
                    K_BuildBundleCompiler.BuildAssetBundle(VARIABLE);
                }
            }
        }

        [Button("Build AssetBundle And Copy To StreamingAssets", 30)]
        public void CopyBundleToStreamingAssets()
        {
            foreach (var VARIABLE in moudleDataList)
            {
                if (VARIABLE.isBuild)
                {
                    //TODO:复制到StreamingAssets
                }
            }
        }
    }
}