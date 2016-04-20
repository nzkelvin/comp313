using UnityEngine;
using System.Collections;
using System;

public class AIMovement : MonoBehaviour {
    private NavMeshAgent _agent;
    private Animator _anim;
    private float _currentDegree = 0f;
    private float _angleInterval = 0.5f;
    private Vector3 _patrolCentre;
    private Vector3 _nextPos;

    public float PatrolRadius = 1;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () {
        var initPos = GameObject.Find("AICharacter").transform.position;
        _patrolCentre = new Vector3(initPos.x - PatrolRadius, 0f, 0f);
        CalculateNextPosition(PatrolRadius, _currentDegree, _patrolCentre);
        GameObject.Find("AICharacter").transform.position = _nextPos;
        //Debug.Log(String.Format("NextPos from Start: {0}, {1}, {2}", _nextPos.x, _nextPos.y, _nextPos.z));
    }

    // Update is called once per frame
    void Update()
    {
        var currentPos = GameObject.Find("AICharacter").transform.position;

        _currentDegree += _angleInterval;
        _currentDegree = _currentDegree % 360;
        CalculateNextPosition(PatrolRadius, _currentDegree, _patrolCentre);
        GameObject.Find("AICharacter").transform.Rotate(0, -_angleInterval, 0);
        GameObject.Find("AICharacter").transform.position = _nextPos;
        
        //Debug.Log(String.Format("If current pos equals to next pos: {0}", Vector3.Equals(currentPos, _nextPos)));
        //Debug.Log(String.Format("CurrentPos from Update: {0}, {1}, {2}", currentPos.x, currentPos.y, currentPos.z));
        //Debug.Log(String.Format("NextPos from Update: {0}, {1}, {2}", _nextPos.x, _nextPos.y, _nextPos.z));

        Animating();
    }

    private void CalculateNextPosition(float radius, float degrees, Vector3 offset)
    {
        double angle = Math.PI * degrees / 180.0;
        var x = (float)Math.Cos(angle) * radius + offset.x;
        var z = (float)Math.Sin(angle) * radius;
        _nextPos.x = x;
        _nextPos.y = 0;
        _nextPos.z = z;
    }

    void Animating()
    {
        //bool running = _agent.velocity.x != 0 || _agent.velocity.z != 0;
        _anim.SetBool("IsRunning", true);//running);
    }
}
