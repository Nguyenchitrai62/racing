//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(AI_BOT))]
//public class AI_BOTEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); // Vẽ giao diện mặc định

//        AI_BOT script = (AI_BOT)target; // Lấy tham chiếu đến AI_BOT script

//        if (GUILayout.Button("Assign Children"))
//        {
//            Undo.RecordObject(script, "Assign Children"); // Ghi lại thao tác để có thể undo
//            script.AssignChildrenToTarget(); // Gọi phương thức để gán các child
//            EditorUtility.SetDirty(script); // Đánh dấu object đã thay đổi
//        }
//    }
//}
