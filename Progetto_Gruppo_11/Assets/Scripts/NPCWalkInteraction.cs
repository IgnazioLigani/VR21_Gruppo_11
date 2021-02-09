using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCWalkInteraction : Interactable
{
    public enum NPCState
    {
        Walk,
        LookAt,
        InPlace
    }

    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _maxLookDistance = 3f;
    [SerializeField] private Animator _animator;

    [SerializeField] private string[] _dialog;
    [SerializeField] private GameObject _dBox;
    [SerializeField] private Text _dText;
    [SerializeField] private GameObject _dContinue;

    private Boolean startDialog=true;
    private int n = 0;

    private NPCState _currentState;
    private NavMeshAgent _navMeshAgent;
    private int _currentWayPointIndex = 0;
    private Boolean interact = false;
    private Quaternion _originalRotation;     


    protected void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_waypoints.Count!=0)
        {
            _currentState = NPCState.Walk;
        }
        else
        {
            _currentState = NPCState.InPlace;
        }
        
        _originalRotation = transform.rotation;

        _dBox.SetActive(false);

        if (_dialog.Length == 0)
            startDialog = false;

}

    void Update()
    {
        //_Update();
        UpdateState();
        CheckTransition();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case NPCState.Walk:
                SetWayPointDestination();
                break;
            case NPCState.LookAt:
                Look();
                break;
            case NPCState.InPlace:
                ReturnToPosition();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckTransition()
    {
        NPCState newNPCState = _currentState;

        switch (_currentState)
        {
            case NPCState.Walk:
                if (interact)
                    newNPCState = NPCState.LookAt;
                break;

            case NPCState.LookAt:
                if (!interact) {
                    if (_waypoints.Count == 0)
                    {
                        newNPCState = NPCState.InPlace;
                        break;
                    }
                    newNPCState = NPCState.Walk;
                    break;
                }
                break;

            case NPCState.InPlace:
                if (interact) 
                {
                    newNPCState = NPCState.LookAt;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (newNPCState != _currentState)
        {
            //Debug.Log($"Changing State FROM:{_currentState} --> TO:{newNPCState}");
            _currentState = newNPCState;
        }
    }

    private void Look()
    {
        if (interact)
        {
            _animator.SetBool("Stop", false);
            _navMeshAgent.isStopped = true;
            Vector3 targetDirection = _target.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150f * Time.deltaTime);
            if (transform.rotation==targetRotation)
            { 
                _animator.SetBool("Stop", true);
                dialogue();
            }
            
            if (!IsTargetWithinDistance(_maxLookDistance))
            {
                n = 0;
                interact = false;
                _dBox.SetActive(false);
            }
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

    private void ReturnToPosition() 
    {
        _animator.SetBool("Stop", false);
        if (transform.rotation == _originalRotation)
        {
            _animator.SetBool("Stop", true);
        }
            
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _originalRotation, 150f * Time.deltaTime);
        //Debug.Log("ritorno alla mia posizione");
    }

    private bool IsTargetWithinDistance(float distance)
    {
        return (_target.transform.position - transform.position).sqrMagnitude <= distance * distance;
    }

    public override void Interact(GameObject caller)
    {
        interact = true;
        //Script per l'interfaccia grafica...
    }

    private void dialogue() 
    {
        if (startDialog) 
        {
            _dBox.SetActive(true);
            if (_dialog.Length > 0)
            {
                if (n < _dialog.Length - 1)
                {
                    _dContinue.SetActive(true);
                }
                else
                {
                    _dContinue.SetActive(false);
                }

                /*foreach(char letter in _dialog[n].ToCharArray()) 
                {
                    _dText.text += letter;
                }*/
                _dText.text = _dialog[n];
                if (Input.GetKeyDown(KeyCode.Space) && n < _dialog.Length - 1)
                {
                    n++;
                }

            }
            else
            {
                _dContinue.SetActive(false);
                _dText.text = _dialog[0];
            }
        }
    }

    /*protected void _Update()
    {
        UpdateState();
        CheckTransition();
    }*/
}