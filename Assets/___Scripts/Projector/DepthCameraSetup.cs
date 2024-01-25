using UnityEngine;

public class DepthCameraSetup : MonoBehaviour
{
    public RenderTexture depthTexture; // Assign this in the inspector
    public Material projectorMaterial; // Assign this in the inspector
    public Shader depthShader; // Assign a shader that outputs depth

    private Camera depthCamera;

    void Start()
    {
        // Ensure this camera renders first, and only renders depth
        depthCamera = gameObject.AddComponent<Camera>();
        depthCamera.depth = -1;
        depthCamera.clearFlags = CameraClearFlags.Depth;
        depthCamera.renderingPath = RenderingPath.Forward; // Use the forward rendering path to render depth
        depthCamera.targetTexture = depthTexture;
        depthCamera.SetReplacementShader(depthShader, "RenderType");

        // Update the projector material with the depth texture
        if (projectorMaterial != null)
        {
            projectorMaterial.SetTexture("_DepthTex", depthTexture);
        }
        else
        {
            Debug.LogError("Projector material not set.");
        }
    }
}
