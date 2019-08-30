using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{

    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class EnemyAIBehavior : MonoBehaviour
    {

        [SerializeField] private List<Transform> m_patrolWaypoints;

        [SerializeField] private float m_walkSpeed = 0.5f;

        [SerializeField] private float m_runSpeed = 2f;

        [Range(0, 1)]
        [SerializeField] private float m_runProbabilityPercentage = 0.3f;

        [SerializeField] private GlowEnemyBehavior m_glowBehavior;

        [SerializeField] private ParticleSystem m_footstepsParticleSystem;

        private int m_waypointIndex = 0;

        private int m_waypointsCount = 0;

        private UnityEngine.AI.NavMeshAgent m_agent;

        private ThirdPersonCharacter m_character;

        private bool m_Running = false;


        void Awake()
        {

            m_waypointsCount = m_patrolWaypoints.Count;

        }

        // Start is called before the first frame update
        void Start()
        {

            m_agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            m_character = GetComponent<ThirdPersonCharacter>();

            m_agent.updateRotation = false;
            m_agent.updatePosition = true;

            m_agent.SetDestination(m_patrolWaypoints[m_waypointIndex].position);

            m_footstepsParticleSystem.Stop();

        }

        // Update is called once per frame
        void Update()
        {

            if (!m_agent.pathPending &&
                m_agent.remainingDistance > m_agent.stoppingDistance)
            {

                m_character.Move(m_agent.desiredVelocity, false, false);
                if (m_Running)
                {

                    var main = m_footstepsParticleSystem.main;
                    main.simulationSpeed = 2f;

                }
                else
                {

                    var main = m_footstepsParticleSystem.main;
                    main.simulationSpeed = 1f;

                }

            }
            else
            {

                m_footstepsParticleSystem.Stop();
                m_agent.SetDestination(getNextDestination());
                setWalkOrRun();
                m_character.Move(m_agent.desiredVelocity, false, false);

            }

            if (m_glowBehavior.OnStealth())
            {

                m_footstepsParticleSystem.Play();

            }
            else
            {

                m_footstepsParticleSystem.Stop();

            }

        }

        private Vector3 getNextDestination()
        {

            return m_patrolWaypoints[Random.Range(0, m_waypointsCount)].position;

        }

        private void setWalkOrRun()
        {

            int _randomNum = Random.Range(1, 10);
            if (_randomNum > (m_runProbabilityPercentage * 10))
            {

                // Walk
                m_agent.speed = m_walkSpeed;
                m_glowBehavior.enemyRunning(false);
                m_Running = false;

            }
            else
            {

                // Run
                m_agent.speed = m_runSpeed;
                m_glowBehavior.enemyRunning(true);
                m_Running = true;

            }

        }

    }

}
