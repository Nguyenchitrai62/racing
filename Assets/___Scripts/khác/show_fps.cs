using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(200, 10, 100, 30);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 20;
        style.normal.textColor = Color.black;
        GUI.Label(rect, "FPS: " + fps, style);
    }
}
