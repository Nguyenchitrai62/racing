//using UnityEngine;
//using UnityEditor;
//using TMPro;

//public class TMPTextAdjustmentsEditor : EditorWindow
//{
//    public TMP_FontAsset newFontAsset; // Chọn font mới từ Editor

//    [MenuItem("Tools/Adjust TMP Texts (Including Inactive)")]
//    public static void ShowWindow()
//    {
//        GetWindow<TMPTextAdjustmentsEditor>("Adjust TMP Texts (Including Inactive)");
//    }

//    void OnGUI()
//    {
//        GUILayout.Label("Adjust All TMP Texts (Including Inactive)", EditorStyles.boldLabel);

//        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newFontAsset, typeof(TMP_FontAsset), false);

//        if (GUILayout.Button("Apply Adjustments"))
//        {
//            ApplyTextAdjustments();
//        }
//    }

//    void ApplyTextAdjustments()
//    {
//        if (newFontAsset == null)
//        {
//            Debug.LogError("No TMP font asset selected.");
//            return;
//        }

//        var allTMPTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>(); // Tìm cả các đối tượng không active

//        foreach (var tmpText in allTMPTexts)
//        {
//            if (tmpText.hideFlags == HideFlags.None || tmpText.hideFlags == HideFlags.NotEditable) // Kiểm tra không phải là prefab
//            {
//                if (tmpText.gameObject.scene.isLoaded) // Kiểm tra đối tượng có trong scene đã load
//                {
//                    tmpText.font = newFontAsset; // Thay đổi font
//                    tmpText.fontStyle = FontStyles.Normal; // Bỏ tô đậm
//                    tmpText.alignment = TextAlignmentOptions.Center; // Căn chính giữa
//                    EditorUtility.SetDirty(tmpText); // Đánh dấu sự thay đổi để lưu
//                }
//            }
//        }
//    }
//}
