using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ShaderManager : MonoBehaviour
{

    [SerializeField] private CameraShaderBehavior m_playerCamera;

    [SerializeField] private PostProcessingProfile m_ppProfile;

    [SerializeField] private List<GlowEnemyBehavior> m_glowBehaviors;

    [SerializeField] private CanvasBehavior m_canvasBehavior;

    private Vector3 m_cameraPosition;

    private Vector3 m_cameraStealthPosition;

    private float m_cameraMoveSpeed = 2f;

    private bool m_stealthModeActivated = false;

    private ColorGradingModel.Settings m_greyscaleSettings;

    private float m_greyScaleFinalSaturation = 0.2f;

    private float m_greyScaleSaturationSpeed = 1f;

    private VignetteModel.Settings m_vignetteSettings;

    private float m_vignetteFinalIntensity = 0.4f;

    private float m_vignetteIntensitySpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {

        m_ppProfile.colorGrading.enabled = true;
        m_ppProfile.vignette.enabled = true;
        m_cameraPosition = m_playerCamera.gameObject.transform.localPosition;
        m_cameraStealthPosition = new Vector3(m_cameraPosition.x, 1.119f, -0.668f);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_stealthModeActivated)
        {

            if (m_playerCamera.gameObject.transform.localPosition != m_cameraStealthPosition)
                m_playerCamera.gameObject.transform.localPosition = Vector3.Lerp(m_playerCamera.gameObject.transform.localPosition, m_cameraStealthPosition, m_cameraMoveSpeed * Time.deltaTime);

            if (m_ppProfile.colorGrading.settings.basic.saturation != m_greyScaleFinalSaturation)
            {

                m_greyscaleSettings = m_ppProfile.colorGrading.settings;
                m_greyscaleSettings.basic.saturation = Mathf.Lerp(m_ppProfile.colorGrading.settings.basic.saturation, m_greyScaleFinalSaturation, m_greyScaleSaturationSpeed * Time.deltaTime);
                m_ppProfile.colorGrading.settings = m_greyscaleSettings;

            }

            if (m_ppProfile.vignette.settings.intensity != m_vignetteFinalIntensity)
            {

                m_vignetteSettings = m_ppProfile.vignette.settings;
                m_vignetteSettings.intensity = Mathf.Lerp(m_ppProfile.vignette.settings.intensity, m_vignetteFinalIntensity, m_vignetteIntensitySpeed * Time.deltaTime);
                m_ppProfile.vignette.settings = m_vignetteSettings;

            }

        }
        else
        {
            if  (m_playerCamera.gameObject.transform.localPosition != m_cameraPosition)
                m_playerCamera.gameObject.transform.localPosition = Vector3.Lerp(m_playerCamera.gameObject.transform.localPosition, m_cameraPosition, m_cameraMoveSpeed * Time.deltaTime);

            if (m_ppProfile.colorGrading.settings.basic.saturation != 1f)
            {

                m_greyscaleSettings = m_ppProfile.colorGrading.settings;
                m_greyscaleSettings.basic.saturation = Mathf.Lerp(m_ppProfile.colorGrading.settings.basic.saturation, 1f, m_greyScaleSaturationSpeed * Time.deltaTime);
                m_ppProfile.colorGrading.settings = m_greyscaleSettings;

            }

            if (m_ppProfile.vignette.settings.intensity != 0f)
            {

                m_vignetteSettings = m_ppProfile.vignette.settings;
                m_vignetteSettings.intensity = Mathf.Lerp(m_ppProfile.vignette.settings.intensity, 0f, m_vignetteIntensitySpeed * Time.deltaTime);
                m_ppProfile.vignette.settings = m_vignetteSettings;

            }

        }

    }

    public void StealthModeEffect(bool activate)
    {

        m_stealthModeActivated = activate;

        if (activate)
        {
            
            runGlowOnEnemies(true);
            m_canvasBehavior.OnStealth(true);

        }
        else
        {

            runGlowOnEnemies(false);
            m_canvasBehavior.OnStealth(false);

        }

    }

    private void runGlowOnEnemies(bool glow)
    {

        foreach (GlowEnemyBehavior glowEnemy in m_glowBehaviors)
        {

            if (glow)
            {

                glowEnemy.onStealthMode();

            }
            else
            {

                glowEnemy.onNormalMode();

            }

        }

    }

}
