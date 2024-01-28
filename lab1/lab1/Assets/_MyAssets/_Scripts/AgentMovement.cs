using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] Camera m_mainCamera;
    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] float m_speed = 5f;

    public static AgentMovement s_instance;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_targetPosition = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
            m_targetPosition.z = 0;
            LookAt2D(m_targetPosition);
        }
        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_speed* Time.deltaTime);
        if (transform.position.x >= 3.2 && transform.position.x <= 4.0 && transform.position.y >= 2.2 && transform.position.y <= 3.0)
        {
            SceneManager.LoadScene(2);
        }
    }


    void LookAt2D(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }    
}
