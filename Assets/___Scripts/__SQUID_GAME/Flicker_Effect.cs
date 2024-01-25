using UnityEngine;

public class Flicker_Effect : MonoBehaviour
{
    public float flickerDuration = 1.0f;
    public float minOpacity = 0.3f;
    public float maxOpacity = 1.0f;

    private Material playerMaterial;
    private float flickerTimer;
    private bool isCurrentlyFlickering = false;

    public static bool is_flicker = false;

    void Start()
    {
        // Lấy material từ Renderer của đối tượng
        playerMaterial = GetComponent<SkinnedMeshRenderer>().material;
        flickerTimer = 0f;
    }

    void Update()
    {
        if (is_flicker != isCurrentlyFlickering)
        {
            if (is_flicker)
            {
                SetMaterialToTransparent();
            }
            else
            {
                SetMaterialToOpaque();
            }

            isCurrentlyFlickering = is_flicker;
        }

        if (is_flicker)
        {
            flickerTimer += Time.deltaTime;

            if (flickerTimer >= flickerDuration)
            {
                flickerTimer = 0f;
            }

            // Tính toán độ trong suốt mới dựa trên thời gian
            float alpha = Mathf.Lerp(minOpacity, maxOpacity, Mathf.Abs(Mathf.Sin(flickerTimer / flickerDuration * Mathf.PI)));
            Color color = playerMaterial.color;
            color.a = alpha;
            color.g = 0f;
            color.b = 0f;
            playerMaterial.color = color;
        }
    }

    private void SetMaterialToTransparent()
    {
        //playerMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //playerMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //playerMaterial.SetInt("_ZWrite", 0);
        //playerMaterial.DisableKeyword("_ALPHATEST_ON");
        //playerMaterial.EnableKeyword("_ALPHABLEND_ON");
        //playerMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //playerMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private void SetMaterialToOpaque()
    {
        Color color = playerMaterial.color;
        color.a = 1f;
        color.g = 1;
        color.b = 1;
        playerMaterial.color = color;

        //playerMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //playerMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //playerMaterial.SetInt("_ZWrite", 1);
        //playerMaterial.DisableKeyword("_ALPHATEST_ON");
        //playerMaterial.DisableKeyword("_ALPHABLEND_ON");
        //playerMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //playerMaterial.renderQueue = -1;
    }
}
