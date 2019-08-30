using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowPrePass : MonoBehaviour
{

    private static RenderTexture m_prePass;

    private static RenderTexture m_blurred;

    private Material m_blurMat;


    void OnEnable()
    {
        m_prePass = new RenderTexture(Screen.width, Screen.height, 24);
        m_prePass.antiAliasing = QualitySettings.antiAliasing;
        m_blurred = new RenderTexture(Screen.width >> 1, Screen.height >> 1, 0);

        var camera = GetComponent<Camera>();
        var glowShader = Shader.Find("Hidden/GlowReplace");
        camera.targetTexture = m_prePass;
        camera.SetReplacementShader(glowShader, "Glowable");
        Shader.SetGlobalTexture("_GlowPrePassTex", m_prePass);

        Shader.SetGlobalTexture("_GlowBlurredTex", m_blurred);

        m_blurMat = new Material(Shader.Find("Hidden/Blur"));
        m_blurMat.SetVector("_BlurSize", new Vector2(m_blurred.texelSize.x * 1.5f, m_blurred.texelSize.y * 1.5f));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst);

        Graphics.SetRenderTarget(m_blurred);
        GL.Clear(false, true, Color.clear);

        Graphics.Blit(src, m_blurred);

        for (int i = 0; i < 4; i++)
        {
            var temp = RenderTexture.GetTemporary(m_blurred.width, m_blurred.height);
            Graphics.Blit(m_blurred, temp, m_blurMat, 0);
            Graphics.Blit(temp, m_blurred, m_blurMat, 1);
            RenderTexture.ReleaseTemporary(temp);
        }
    }

}
