using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kaki_AssetBundleBuilder
{
    [CreateAssetMenu(menuName = "创建打包配置文件/Creat BuildConfig", fileName = "BuildBundleConfig", order = 4)]
    public class K_BuildBundleConfig : ScriptableObject
    {
        private static K_BuildBundleConfig instance;

        public static K_BuildBundleConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath("Assets/AssetBundleBuilder/BuildBundleConfig.asset",
                        typeof(K_BuildBundleConfig)) as K_BuildBundleConfig;
                }

                return instance;
            }
        }

        [SerializeField] public List<K_BundleMoudleData> assetBundleConfig = new List<K_BundleMoudleData>();

        /// <summary>
        /// Get Module Data By Name
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public K_BundleMoudleData GetBundleDataByName(string moduleName)
        {
            foreach (var VARIABLE in assetBundleConfig)
            {
                if (string.Equals(VARIABLE.moduleNmae, moduleName))
                {
                    return VARIABLE;
                }
            }

            return null;
        }

        /// <summary>
        /// Remove Module By Name
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModuleByName(string moduleName)
        {
            for (int i = 0; i < assetBundleConfig.Count; i++)
            {
                if (assetBundleConfig[i].moduleNmae == moduleName)
                {
                    assetBundleConfig.Remove(assetBundleConfig[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// Create a new module data
        /// </summary>
        /// <param name="moduleData"></param>
        public void CreateModuleData(K_BundleMoudleData moduleData)
        {
            
            assetBundleConfig.Add(moduleData);
            Save();
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}