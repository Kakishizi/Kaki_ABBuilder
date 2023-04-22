using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    [System.Serializable]
    public class K_BundleMoudleData : ScriptableObject
    {
        public long bundleId; //模块ID
        public string moduleNmae; //模块名字
        public bool isBuild; //是否打包
        public string[] prefabPathArr;
        public string[] rootFolderPathArr;
        public BundleFileInfo[] singlePathArr;
        [HideInInspector] public float lastCilckBtnTime; //上次点击按钮时间,用于双击判断
    }

    [System.Serializable]
    public class BundleFileInfo
    {
        [HideLabel] public string abName = "AB Name";
        [HideLabel] [FilePath] public string bundlePath;
    }
}