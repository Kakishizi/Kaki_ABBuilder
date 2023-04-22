using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public class K_BundleConfigBase
    {
        //模块配置列表
        protected List<K_BundleMoudleData> moudleDataList;
        protected List<List<K_BundleMoudleData>> rowModuleDataList;

        public virtual void Inization()
        {
            moudleDataList = K_BuildBundleConfig.Instance.assetBundleConfig;
            rowModuleDataList = new List<List<K_BundleMoudleData>>();
            for (int i = 0; i < moudleDataList.Count; i++)
            {
                int rowIndex = Mathf.FloorToInt(i / 6);
                //换行
                if (rowModuleDataList.Count < rowIndex + 1)
                {
                    rowModuleDataList.Add(new List<K_BundleMoudleData>());
                }

                rowModuleDataList[rowIndex].Add(moudleDataList[i]);
            }
        }

        [OnInspectorGUI]
        public virtual void OnGui()
        {
            if (rowModuleDataList == null)
            {
                return;
            }

            GUIContent content = EditorGUIUtility.IconContent("ScriptableObject Icon".Trim(), "测试");
            content.tooltip = "单击可选中/取消，双击打开配置窗口";
            for (int i = 0; i < rowModuleDataList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < rowModuleDataList[i].Count; j++)
                {
                    K_BundleMoudleData moduleData = rowModuleDataList[i][j];

                    if (GUILayout.Button(content, GUILayout.Width(130), GUILayout.Height(160)))
                    {
                        moduleData.isBuild = moduleData.isBuild == false ? true : false;
                        if (Time.realtimeSinceStartup - moduleData.lastCilckBtnTime <= 0.18f)
                        {
                            //  BuildBundleConfigWindow.ShowBuildBundleConfigWindow(moudleData);
                            K_BundleModuleConfigWindow.ShowWindow(moduleData.moduleNmae);
                        }

                        moduleData.lastCilckBtnTime = Time.realtimeSinceStartup;
                    }

                    if (moduleData.isBuild)
                    {
                        GUIStyle style = K_BundleEditorUtility.GetGUIStyle("LightmapEditorSelectedHighlight");
                        style.contentOffset = new Vector2(100, -70);
                        GUI.Toggle(new Rect(10 + (j * 133), -137 + ((i + 1) * 165), 120, 153), true,
                            EditorGUIUtility.IconContent("Collab"), style);
                    }

                    GUI.Label(new Rect((j + 1) * 10 + (j * 125), (i + 1) * 130 + i * 30, 115, 20),
                        moduleData.moduleNmae,
                        new GUIStyle { alignment = TextAnchor.MiddleCenter });
                }

                if (i == rowModuleDataList.Count - 1)
                {
                    DrawAddModuleButtons();
                }

                GUILayout.EndHorizontal();
            }

            if (rowModuleDataList.Count == 0)
            {
                DrawAddModuleButtons();
            }

            DrawBuildButtons();
        }

        /// <summary>
        /// 绘制打包按钮
        /// </summary>
        public virtual void DrawBuildButtons()
        {
        }


        /// <summary>
        /// 绘制添加模块按钮
        /// </summary>
        public virtual void DrawAddModuleButtons()
        {
        }

        /// <summary>
        /// 打包资源
        /// </summary>
        public virtual void BuildBundle()
        {
        }
    }
}