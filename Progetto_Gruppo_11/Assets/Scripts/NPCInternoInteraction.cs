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
    private Boolean inDialog = false;
    private int n = 0;

    private Boolean interact = false;


    protected void Start()
    {
        _dBox.SetActive(false);
        _dContinue.SetActive(false);
        if (_dialog.Length == 0)
            startDialog = false;
    }

    void Update()
    {
        //Debug.Log(gameObject.name +" _interact:  "+interact);
        if (interact) 
        {
            /*Debug.Log(gameObject.name + " _interact:  " + interact);
            if (!_dContinue.activeSelf && n < _dialog.Length - 1)
            {
                Dialogue();
            }

            
            if (Input.GetKeyDown(KeyCode.Space) && _dContinue.activeSelf)
            {
                Dialogue();
            }*/
                

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
        interact = !interact;
        //Debug.Log("Interact " + interact);
    }

    private void Dialogue()
    {
        if (startDialog)
        {
            
            _dBox.SetActive(true);
            //Debug.Log("Dialogue");
            /*if (n < _dialog.Length - 1)
            {
                StartCoroutine(TypingLetters());
                _dContinue.SetActive(true);
            }
            else
            {
                StartCoroutine(TypingLetters());
                _dContinue.SetActive(false);
            }
            _dText.text = _dialog[n];
            if (Input.GetKeyDown(KeyCode.Space) && n < _dialog.Length - 1)
            {
                n++;
            }*/

            /*if (Input.GetKeyDown(KeyCode.Space) && n < _dialog.Length - 1)
            {
                StartCoroutine(TypingLetters());
                n++;
            }*/



            //Originale
            //Debug.Log("Dialogue");
            if (n < _dialog.Length - 1)
            {
                _dContinue.SetActive(true);
            }
            else
            {
                _dContinue.SetActive(false);
            }
            _dText.text = _dialog[n];
            if (Input.GetKeyDown(KeyCode.Space) && n < _dialog.Length - 1)
            {
                n++;
            }
        }

    }

    public override bool GetInteract()
    {
        return interact;
    }

    public IEnumerator TypingLetters()
    {
        foreach(char letter in _dialog[n].ToCharArray())
        {
            _dText.text += letter;
            //Debug.Log(letter); 
            yield return new WaitForSeconds(0.03f);
        }
    }
}