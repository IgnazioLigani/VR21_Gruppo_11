using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCInternoInteraction : Interactable
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _maxLookDistance = 2f;
    [SerializeField] private string[] _dialog;
    [SerializeField] private GameObject _dBox;
    [SerializeField] private Text _dText;
    [SerializeField] private GameObject _dContinue;

    private Boolean startDialog = true;
    private int n = 0;

    private Boolean interact = false;


    protected void Start()
    {
        _dBox.SetActive(false);

        if (_dialog.Length == 0)
            startDialog = false;
    }

    void Update()
    {
        if (interact) 
        {
            Dialogue();

            if (!IsTargetWithinDistance(_maxLookDistance))
            {
                n = 0;
                interact = false;
                _dBox.SetActive(false);
            }
        }
        else
        {
            n = 0;
        }
    }

    private bool IsTargetWithinDistance(float distance)
    {
        return (_target.transform.position - transform.position).sqrMagnitude <= distance * distance;
    }

    public override void Interact(GameObject caller)
    {
        interact = true;
        Debug.Log("ho premuto e sto interaggendo");
    }

    private void Dialogue()
    {
        if (startDialog)
        {
            _dBox.SetActive(true);
            Debug.Log("sono in dialogue");
            if (n < _dialog.Length - 1)
            {
                _dContinue.SetActive(true);
            }
            else
            {
                _dContinue.SetActive(false);
            }
            _dText.text = _dialog[n];
            Debug.Log("L'NPC dice: " + _dialog[n]);
            if (Input.GetKeyDown(KeyCode.Space) && n < _dialog.Length - 1)
            {
                n++;
            }
        }

    }
}