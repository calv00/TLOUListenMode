using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehavior : MonoBehaviour
{

    [SerializeField] private List<Transform> m_enemyTransforms;

    [SerializeField] private Camera m_cam;

    [SerializeField] private Camera m_UICamera;

    [SerializeField] private RectTransform m_enemyIndicatorPrefab;

    private bool m_onStealth = false;

    private List<RectTransform> m_enemyIndicatorTransforms;

    private List<Image> m_enemyIndicatorImages;


    // Start is called before the first frame update
    void Start()
    {

        m_enemyIndicatorTransforms = new List<RectTransform>();
        m_enemyIndicatorImages = new List<Image>();
        foreach (Transform enemyTransform in m_enemyTransforms)
        {

            RectTransform enemyIndicator;
            enemyIndicator = Instantiate(m_enemyIndicatorPrefab, transform, false);
            m_enemyIndicatorTransforms.Add(enemyIndicator);
            m_enemyIndicatorImages.Add(enemyIndicator.GetComponentInChildren<Image>());

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (m_onStealth)
        {

            for (int i = 0; i < m_enemyTransforms.Count; i++)
            {

                Vector3 dir = (m_enemyTransforms[i].position - m_cam.transform.position).normalized;
                float angle = Vector3.SignedAngle(dir, m_cam.transform.forward, Vector3.up);
                if (angle <= 45f && angle >= -45f) angle = 0f;
                if (angle <= 135f && angle >= 45f) angle = 90f;
                if (angle <= -45f && angle >= -135f) angle = -90f;
                if (angle >= 135f && angle <= 180f || angle >= -180f && angle <= -135f) angle = 180f;
                m_enemyIndicatorTransforms[i].localEulerAngles = new Vector3(0, 0, angle);

                float borderSize = 60f;
                Vector3 enemyScreenPosition = m_cam.WorldToScreenPoint(m_enemyTransforms[i].position);
                if (enemyScreenPosition.x <= borderSize || enemyScreenPosition.x >= Screen.width - borderSize ||
                    enemyScreenPosition.y <= borderSize || enemyScreenPosition.y >= Screen.height - borderSize)
                {

                    Vector3 cappedEnemyPosition = enemyScreenPosition;
                    // WorldToScreenPoint(enemyTransform.position) returns ok values when staying backwards to the enemy position, so the another condition must be implemented:
                    if (Vector3.Dot(dir, m_cam.transform.forward) < 0f)
                    {

                        cappedEnemyPosition.x = Mathf.Clamp(cappedEnemyPosition.x, borderSize, Screen.width - borderSize);
                        cappedEnemyPosition.y = Mathf.Clamp(cappedEnemyPosition.y, 0f, Screen.height);
                        // Invert the values to get an appropriate result when backwards
                        cappedEnemyPosition.x = Screen.width - cappedEnemyPosition.x;
                        cappedEnemyPosition.y = Screen.height - cappedEnemyPosition.y;

                    }
                    else
                    {

                        cappedEnemyPosition.x = Mathf.Clamp(cappedEnemyPosition.x, borderSize, Screen.width - borderSize);
                        cappedEnemyPosition.y = Mathf.Clamp(cappedEnemyPosition.y, 0f, Screen.height);

                    }

                    Vector3 pointerWorldPosition = m_UICamera.ScreenToWorldPoint(cappedEnemyPosition);
                    m_enemyIndicatorTransforms[i].position = pointerWorldPosition;
                    m_enemyIndicatorTransforms[i].localPosition = new Vector3(m_enemyIndicatorTransforms[i].localPosition.x, m_enemyIndicatorTransforms[i].localPosition.y, 0f);

                    m_enemyIndicatorImages[i].enabled = true;

                }
                // WorldToScreenPoint(enemyTransform.position) returns ok values when staying backwards to the enemy position, so the another condition must be implemented:
                else if (Vector3.Dot(dir, m_cam.transform.forward) < 0f)
                {

                    // Invert the values to get an appropriate result when backwards
                    Vector3 invertedEnemyPosition = enemyScreenPosition;
                    invertedEnemyPosition.x = Screen.width - invertedEnemyPosition.x - borderSize;
                    invertedEnemyPosition.y = Screen.height - invertedEnemyPosition.y - borderSize;

                    Vector3 _pointerWorldPosition = m_UICamera.ScreenToWorldPoint(invertedEnemyPosition);
                    m_enemyIndicatorTransforms[i].position = _pointerWorldPosition;
                    m_enemyIndicatorTransforms[i].localPosition = new Vector3(m_enemyIndicatorTransforms[i].localPosition.x, m_enemyIndicatorTransforms[i].localPosition.y, 0f);

                    m_enemyIndicatorImages[i].enabled = true;

                }
                else
                {

                    m_enemyIndicatorImages[i].enabled = false;

                }

            }

        }

    }

    public void OnStealth(bool onStealth)
    {

        if (!onStealth)
        {

            for (int i = 0; i < m_enemyTransforms.Count; i++)
            {

                m_enemyIndicatorImages[i].enabled = false;

            }

        }

        m_onStealth = onStealth;

    }
}
