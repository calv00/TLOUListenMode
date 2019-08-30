using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEnemyBehavior : MonoBehaviour
{

    [SerializeField] private Color m_glowColor;

    [SerializeField] private float m_lerpFactor = 10f;

    public Renderer[] Renderers
    {
        get;
        private set;
    }

    public Color CurrentColor
    {
        get { return m_currentColor; }
    }

    private List<Material> m_materials = new List<Material>();

    private Color m_currentColor;

    private Color m_targetColor;

    private bool m_onStealth = false;

    private bool m_onRunning = false;


    // Start is called before the first frame update
    void Start()
    {

        Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
        {
            m_materials.AddRange(renderer.materials);
        }

    }

    public void onStealthMode()
    {
        
        if (m_onRunning)
        {

            m_targetColor = m_glowColor;

        }
        else
        {

            m_targetColor = new Color(.2f, .2f, .2f);

        }


        enabled = true;
        m_onStealth = true;

    }

    public void onNormalMode()
    {

        m_targetColor = Color.black;
        enabled = true;
        m_onStealth = false;

    }

    public void enemyRunning(bool running)
    {

        if (running)
        {

            if (m_onStealth)
            {

                m_targetColor = m_glowColor;

            }

            m_onRunning = true;

        }
        else
        {

            if (m_onStealth)
            {

                m_targetColor = new Color(.2f, .2f, .2f);

            }

            m_onRunning = false;

        }


    }

    public bool OnStealth()
    {

        return m_onStealth;

    }

    /// Loop over all cached materials and update their color, disable self if we reach our target color.
    /// </summary>
    private void Update()
    {
        m_currentColor = Color.Lerp(m_currentColor, m_targetColor, Time.deltaTime * m_lerpFactor);

        for (int i = 0; i < m_materials.Count; i++)
        {
            m_materials[i].SetColor("_GlowColor", m_currentColor);
        }

        if (m_currentColor.Equals(m_targetColor))
        {
            enabled = false;
        }
    }
}
