using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthModeBehavior : MonoBehaviour
{

    [SerializeField] private KeyCode m_stealthModeKey;

    [SerializeField] private ShaderManager m_shaderManager;


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(m_stealthModeKey))
        {

            enterStealthMode();

        }

        if (Input.GetKeyUp(m_stealthModeKey))
        {

            exitStealthMode();

        }

    }

    private void enterStealthMode()
    {

        m_shaderManager.StealthModeEffect(true);

    }

    private void exitStealthMode()
    {

        m_shaderManager.StealthModeEffect(false);

    }

}
