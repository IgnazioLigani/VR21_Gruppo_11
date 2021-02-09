using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class ObjectGrab : Grabbable
{

    [SerializeField] private GameObject _controllerMov;

    private Rigidbody _rigidbody;
    private Collider[] _collider;
    

    [SerializeField] private GameObject _infoBox;
    [SerializeField] private Text _infoText;
    [SerializeField] private string _dialog;

    public Vector3 _distanzaDallaCamera = new Vector3(0f,0f, 1f);
    public Vector3 _rotate = new Vector3(0,0,0);

    private bool isGrab = false;

    protected override void Start ()
    {
        base.Start();
        _infoBox.SetActive(false);
        _collider = GetComponents<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        

    }

    private void Update()
    {
        if(isGrab)
        {
            //UpdatePosition();
            Vector3 x = transform.parent.position - transform.position;
            //Debug.Log("camera position  " + transform.parent.position + "object position " + transform.position + "differenza= "+ x);
            Vector3 relative = transform.parent.InverseTransformPoint(transform.position);
            Debug.Log("relative " + relative);
            transform.localPosition=_distanzaDallaCamera;
            transform.localRotation = Quaternion.Euler(_rotate);
            //transform.TransformPoint(_distanzaDallaCamera);
            FirstPersonCharacterController other = (FirstPersonCharacterController)_controllerMov.GetComponent(typeof(FirstPersonCharacterController));
            other.SetSpeed(0f);
        }
            

    }

    public override void Grab(GameObject grabber)
    {
        isGrab = true;
        foreach(Collider col in _collider)
        {
            col.enabled = false;
        }
        _rigidbody.isKinematic = true;
        _infoBox.SetActive(true);
        _infoText.text = _dialog;
        Debug.Log("oggetto preso  "+ transform.position);
    }

    public override void Drop()
    {
        isGrab = false;
        foreach (Collider col in _collider)
        {
            col.enabled = true;
        }
        _rigidbody.isKinematic = false;
        _infoBox.SetActive(false);
        FirstPersonCharacterController other = (FirstPersonCharacterController)_controllerMov.GetComponent(typeof(FirstPersonCharacterController));
        other.SetSpeed(5f);
    }

    
}
