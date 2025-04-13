using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    IDLE,
    MOVE_TO_PLAYER,
    PATROL
};



public class AgentObject : MonoBehaviour
{
    [SerializeField] protected Transform m_target;

    public ActionState state { get; set; }
    public Vector3 TargetPosition
    {
        get { return m_target.position; }
        set { m_target.position = value; }
    }
    // Note. I only want the above property here so the class cannot be abstract.

    public void Start()
    {
        Debug.Log("Starting Agent.");
        if (m_target != null)
        {
            TargetPosition = m_target.position;

        }

        state = ActionState.IDLE;
    }
}
