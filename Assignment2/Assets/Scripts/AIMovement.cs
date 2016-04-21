using UnityEngine;
using System.Collections;
using System;

public class AIMovement : MonoBehaviour {
    public float PatrolRadius = 1;
    public float DefaultDegreeInterval = 0.5f;
    public double KnockoutDelayInSec = 0.5;
    public double KnockoutRecoveryInSec = 3.5;
    public float FollowerOverlapRadius = 1f;
    public float FollowerStoppingDistance = 3f;
    public Collider[] colliders;

    private NavMeshAgent _agent;
    private Animator _anim;
    private float _currentDegree = 0f;
    private float _degreeInterval;
    private Vector3 _patrolCentre;
    private Vector3 _nextPos;
    private DateTime _lastKnockoutTime;
    private GameObject _master = null;

    public bool IsKnockedOut {
        get
        {
            return (DateTime.Now > _lastKnockoutTime.AddSeconds(KnockoutDelayInSec)) 
                && (DateTime.Now <= _lastKnockoutTime.AddSeconds(KnockoutRecoveryInSec));
        }
    }

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () {
        var initPos = GameObject.Find("AICharacter").transform.position;
        _patrolCentre = new Vector3(initPos.x - PatrolRadius, 0f, 0f);
        _degreeInterval = DefaultDegreeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMaster();

        if (_master != null)
        {
            GetComponent<NavMeshAgent>().SetDestination(_master.transform.position);
            GetComponent<NavMeshAgent>().stoppingDistance = FollowerStoppingDistance;
        }
        else if (!IsKnockedOut)
        {
            var currentPos = GameObject.Find("AICharacter").transform.position;

            _currentDegree += _degreeInterval;
            _currentDegree = _currentDegree % 360;
            CalculateNextPosition(PatrolRadius, _currentDegree, _patrolCentre);
            GameObject.Find("AICharacter").transform.Rotate(0, -_degreeInterval, 0);
            GameObject.Find("AICharacter").transform.position = _nextPos;
        
            //Debug.Log(String.Format("If current pos equals to next pos: {0}", Vector3.Equals(currentPos, _nextPos)));
            //Debug.Log(String.Format("CurrentPos from Update: {0}, {1}, {2}", currentPos.x, currentPos.y, currentPos.z));
            //Debug.Log(String.Format("NextPos from Update: {0}, {1}, {2}", _nextPos.x, _nextPos.y, _nextPos.z));
        }

        Animating();
    }

    private void CheckMaster()
    {
        if (_master == null)
        {
            colliders = Physics.OverlapSphere(transform.position, FollowerOverlapRadius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.name == "MasterCharacter")
                {
                    _master = collider.gameObject;
                }
            }
        }
    }

    public void KnockOut()
    {
        _lastKnockoutTime = DateTime.Now;
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
        _anim.SetBool("IsRunning", (_master == null && !IsKnockedOut) 
            || (_master != null && GetComponent<NavMeshAgent>().velocity.x != 0 && GetComponent<NavMeshAgent>().velocity.z != 0));//running);
        Debug.Log(GetComponent<NavMeshAgent>().velocity != Vector3.zero);
        Debug.Log(GetComponent<NavMeshAgent>().speed);
    }
}
