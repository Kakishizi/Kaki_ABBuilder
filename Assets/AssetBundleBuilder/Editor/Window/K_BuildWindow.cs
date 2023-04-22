using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Kaki_AssetBundleBuilder
{
    public class K_BuildWindow : OdinMenuEditorWindow
    {
        [SerializeField] public KBuildKBundleWindow KBuildKBundleWindow = new KBuildKBundleWindow();

        [MenuItem("Kaki_ABBuild/BuildWindow", priority = 10)]
        public static void ShowBuildWindow()
        {
            K_BuildWindow window = GetWindow<K_BuildWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.ForceMenuTreeRebuild();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            KBuildKBundleWindow.Inization();
            OdinMenuTree menuTree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "Build", null, EditorIcons.House },
                { "Build/All Config", KBuildKBundleWindow, EditorIcons.UnityLogo },
                { "Build/Config Editor", null, EditorIcons.SettingsCog }
            };
            menuTree.AddAllAssetsAtPath("Build/Config Editor", "AssetBundleBuilder/Config", typeof(ScriptableObject),
                    true)
                .AddThumbnailIcons();
            return menuTree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            var selectItem = this.MenuTree.Selection.FirstOrDefault();
            var selected = this.MenuTree.Selection;
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            var selectAsset = selected.SelectedValue;
            if (selectAsset == null) return;
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selectItem != null)
                {
                    GUILayout.Label(selectItem.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Config")))
                {
                    string dest = "Assets/AssetBundleBuilder/Config".TrimEnd('/');
                    if (!Directory.Exists(dest))
                    {
                        Directory.CreateDirectory(dest);
                        AssetDatabase.Refresh();
                    }

                    dest = EditorUtility.SaveFilePanel("Save object as", dest,
                        "New " + typeof(ScriptableObject).GetNiceName(),
                        "asset");

                    if (!string.IsNullOrEmpty(dest) &&
                        PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                    {
                        var obj = ScriptableObject.CreateInstance<K_BundleMoudleData>();
                        K_BuildBundleConfig.Instance.CreateModuleData(obj);
                        AssetDatabase.CreateAsset(obj, dest);
                        AssetDatabase.Refresh();
                        obj.moduleNmae = obj.name;
                    }
                    else
                    {
                        UnityEngine.Object.DestroyImmediate(null);
                    }

                    ShowBuildWindow();
                }

                if (selectAsset as K_BundleMoudleData)
                {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete Config")))
                    {
                        var asset = selectAsset as K_BundleMoudleData;
                        K_BuildBundleConfig.Instance.RemoveModuleByName(asset.moduleNmae);
                        string path = AssetDatabase.GetAssetPath(asset);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                if (selectItem.Name == "All Config")
                {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete Selected Config")))
                    {
                        var removeList = new List<K_BundleMoudleData>();
                        for (int i = 0; i < K_BuildBundleConfig.Instance.assetBundleConfig.Count; i++)
                        {
                            if (K_BuildBundleConfig.Instance.assetBundleConfig[i].isBuild)
                            {
                                removeList.Add(K_BuildBundleConfig.Instance.assetBundleConfig[i]);
                            }
                        }

                        for (int i = 0; i < removeList.Count; i++)
                        {
                            K_BuildBundleConfig.Instance.RemoveModuleByName(removeList[i].moduleNmae);
                            var path = AssetDatabase.GetAssetPath(removeList[i]);
                            AssetDatabase.DeleteAsset(path);
                        }

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        ShowBuildWindow();
                    }
                }

                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }
    }
}