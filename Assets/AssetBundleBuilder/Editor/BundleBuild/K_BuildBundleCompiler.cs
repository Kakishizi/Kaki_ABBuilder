using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Packages.Rider.Editor.Util;
using PlasticPipe.PlasticProtocol.Messages;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor.Modules;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public enum BuildType
    {
        AssetBundle
    }

    public class K_BuildBundleCompiler
    {
        private static string mUpdateNotice; //Update Notice
        private static int mHotPatchVersion; //Hot Patch Version
        private static BuildType mBuildType; //Build Type
        private static K_BundleMoudleData mBundleMoudleData; //Bundle Moudle Data
        private static K_BundleModuleEnum _mKBundleModuleEnum; //Bundle Module Enum

        private static List<string> mAllBundlePathList = new List<string>(); //All Bundle Path List

        private static Dictionary<string, List<string>>
            mAllFolderBundleDic = new Dictionary<string, List<string>>(); //All Folder Bundle Dic

        private static Dictionary<string, List<string>>
            mAllPrefabBundleDic = new Dictionary<string, List<string>>(); //All Prefab Bundle Dic

        /// <summary>
        /// Build AssetBundle
        /// </summary>
        /// <param name="moudleData"></param>
        /// <param name="buildType"></param>
        public static void BuildAssetBundle(K_BundleMoudleData moudleData, BuildType buildType = BuildType.AssetBundle)
        {
            //initialization build data
            Initlization(moudleData, buildType);
            BuildAllFolder();
            BuildRootSubFolder();
            BuildAllPrefabs();
            //start build Assetbundle
            BuildAllAssetBundle();
        }

        public static void Initlization(K_BundleMoudleData moudleData, BuildType buildType = BuildType.AssetBundle)
        {
            mAllBundlePathList.Clear();
            mAllFolderBundleDic.Clear();
            mAllPrefabBundleDic.Clear();

            mBuildType = buildType;
            mBundleMoudleData = moudleData;
            _mKBundleModuleEnum = (K_BundleModuleEnum)Enum.Parse(typeof(K_BundleModuleEnum), moudleData.moduleNmae);
        }

        /// <summary>
        /// Build All Folder
        /// </summary>
        public static void BuildAllFolder()
        {
            if (mBundleMoudleData.singlePathArr == null || mBundleMoudleData.singlePathArr.Length == 0)
            {
                return;
            }

            for (int i = 0; i < mBundleMoudleData.singlePathArr.Length; i++)
            {
                string path = mBundleMoudleData.singlePathArr[i].bundlePath.Replace(@"\", "/");
                if (!IsRepeatBundleFile(path))
                {
                    mAllBundlePathList.Add(path);
                    //Set AssetBundleName
                    //example: "moduleName_abName"    
                    string bundleName = GenerateBundleName(mBundleMoudleData.singlePathArr[i].abName);
                    if (!mAllFolderBundleDic.ContainsKey(bundleName))
                    {
                        mAllFolderBundleDic.Add(bundleName, new List<string>() { path });
                    }
                    else
                    {
                        mAllFolderBundleDic[bundleName].Add(path);
                    }
                }
                else
                {
                    Debug.LogError("RepeatBundleFile: " + path);
                }
            }
        }

        /// <summary>
        /// Build Root Sub Folder
        /// </summary>
        public static void BuildRootSubFolder()
        {
            if (mBundleMoudleData.rootFolderPathArr == null || mBundleMoudleData.rootFolderPathArr.Length == 0)
            {
                return;
            }

            for (int i = 0; i < mBundleMoudleData.rootFolderPathArr.Length; i++)
            {
                string path = mBundleMoudleData.rootFolderPathArr[i] + "/";
                //Get All Folder
                string[] folderArr = Directory.GetDirectories(path);
                foreach (var folder in folderArr)
                {
                    path = folder.Replace(@"\", "/");
                    int nameIndex = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    string bundleName = GenerateBundleName(path.Substring(nameIndex, path.Length - nameIndex));
                    if (!IsRepeatBundleFile(path))
                    {
                        mAllBundlePathList.Add(path);
                        if (!mAllFolderBundleDic.ContainsKey(bundleName))
                        {
                            mAllFolderBundleDic.Add(bundleName, new List<string>() { path });
                        }
                        else
                        {
                            mAllFolderBundleDic[bundleName].Add(path);
                        }
                    }
                    else
                    {
                        Debug.LogError("RepeatBundle File FolderPath: " + path);
                    }

                    //Sub Folder 
                    string[] filePathArr = Directory.GetFiles(path, "*");
                    foreach (var filePath in filePathArr)
                    {
                        if (!filePath.EndsWith(".meta"))
                        {
                            string abFilePath = filePath.Replace(@"\", "/");
                            if (!IsRepeatBundleFile(abFilePath))
                            {
                                mAllBundlePathList.Add(abFilePath);
                                if (!mAllFolderBundleDic.ContainsKey(bundleName))
                                {
                                    mAllFolderBundleDic.Add(bundleName, new List<string>() { abFilePath });
                                }
                                else
                                {
                                    mAllFolderBundleDic[bundleName].Add(abFilePath);
                                }
                            }
                            else
                            {
                                Debug.LogError("RepeatBundle File FolderPath: " + abFilePath);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build All Prefabs
        /// </summary>
        public static void BuildAllPrefabs()
        {
        }

        /// <summary>
        /// Build AssetBundle
        /// </summary>
        public static void BuildAllAssetBundle()
        {
        }

        public static bool IsRepeatBundleFile(string path)
        {
            foreach (var VARIABLE in mAllBundlePathList)
            {
                if (string.Equals(VARIABLE, path) || VARIABLE.Contains(path) || path.EndsWith(".cs"))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GenerateBundleName(string abName)
        {
            return _mKBundleModuleEnum.ToString() + "_" + abName;
        }
    }
}