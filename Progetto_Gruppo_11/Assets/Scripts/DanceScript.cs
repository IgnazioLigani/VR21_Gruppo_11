using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class DanceScript : MonoBehaviour
{
    public enum GuardState
    {
        Walk,
        LookAt
    }

    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _minLookDistance = 2f;
    [SerializeField] private float _stoppingDistance = 2f;
    [SerializeField] private Animator _animator;

    private GuardState _currentState;
    private NavMeshAgent _navMeshAgent;
    private int _currentWayPointIndex = 0;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = GuardState.Walk;

    }

    void Update()
    {
        UpdateState();
        CheckTransition();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GuardState.Walk:
                SetWayPointDestination();
                break;
            case GuardState.LookAt:
                //FollowTarget();
                Look();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckTransition()
    {
        GuardState newGuardState = _currentState;

        switch (_currentState)
        {
            case GuardState.Walk:
                if (IsTargetWithinDistance(_minLookDistance))
                    newGuardState = GuardState.LookAt;
                break;

            case GuardState.LookAt:
                if (!IsTargetWithinDistance(_minLookDistance))
                {
                    newGuardState = GuardState.Walk;
                    break;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (newGuardState != _currentState)
        {
            Debug.Log($"Changing State FROM:{_currentState} --> TO:{newGuardState}");
            _currentState = newGuardState;
        }
    }

    private void Look()
    {
        if (IsTargetWithinDistance(_stoppingDistance))
        {
            _navMeshAgent.isStopped = true;
            Vector3 targetDirection = _target.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150f * Time.deltaTime);
            _animator.SetBool("Stop", true);
        }
    }

    private void SetWayPointDestination()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool("Stop", false);
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _navMeshAgent.velocity.sqrMagnitude <= 0f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Count;
            Vector3 nextWayPointPos = _waypoints[_currentWayPointIndex].position;
            _navMeshAgent.SetDestination(new Vector3(nextWayPointPos.x, transform.position.y, nextWayPointPos.z));
        }
    }

    private bool IsTargetWithinDistance(float distance)
    {
        return (_target.transform.position - transform.position).sqrMagnitude <= distance * distance;
    }


}
