using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaderBehavior : MonoBehaviour
{

    private Material m_compositeMat;

    private int m_enemyGlowIntensity = 6;


    void OnEnable()
    {

        m_compositeMat = new Material(Shader.Find("Hidden/GlowComposite"));

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        m_compositeMat.SetFloat("_Intensity", m_enemyGlowIntensity);
        Graphics.Blit(source, destination, m_compositeMat, 0);

    }

}
