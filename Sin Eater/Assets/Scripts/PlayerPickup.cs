using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private GameObject _hitObj;
    private bool _isHolding = false,  _outlineIsActive = false;
    private GameObject _heldObject;
    public Transform HeldObjPos;
    [SerializeField] private LayerMask _pickupableLayers, _interactableLayers;
    private Rigidbody _heldRB;
    [SerializeField] private float _throwForce = 10f;
    //private GameObject _secondCamera;
    private int _cachedLayer;
    private Pickupable _heldPickupableScript;
    [SerializeField] private float _raycastDistance = 2f;

    
    


    private void Start()
    {
        InvokeRepeating("CheckPickupAbility", 0f, 0.05f);       //Better than calling it every update frame
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }



    public void Interact()
    {
        if(!_isHolding)
        {
            Pickup();
        }
        else
        {
            Throw();
        }
    }



    //Called often
    public void CheckPickupAbility()
    {
        if (_isHolding) return;


        LayerMask outlineMask = _interactableLayers + _pickupableLayers;

        RaycastHit[] hit = Physics.SphereCastAll(Camera.main.transform.position, 0.7f, Camera.main.transform.forward, _raycastDistance, outlineMask, QueryTriggerInteraction.Collide);

        if (hit.Length > 0)
        {

            if(hit.Length > 1)
            {

                RaycastHit centerHit;

                Ray centerRay = new Ray(origin: transform.position, direction: Camera.main.transform.forward);
                Physics.Raycast(centerRay, out centerHit, _raycastDistance);


                
                _hitObj = GetClosestObject(centerHit.point, hit).gameObject;
            }
            else
            {
                _hitObj = hit[0].transform.gameObject;
            }

            RaycastHit hitPoint;

            Ray ray = new Ray(origin: transform.position, direction: (_hitObj.transform.position - transform.position));
            if (Physics.Raycast(ray, out hitPoint, _raycastDistance))
            {
                if(hitPoint.transform.gameObject != _hitObj.transform.gameObject)
                {
                    return;
                }
            }







            //If last hit is different from this hit
            if (_heldObject != null && _hitObj != _heldObject && _outlineIsActive)
            {
                RemoveOutline();
            }


            _heldObject = _hitObj;

            if (!_outlineIsActive)
            {
                AddOutline();
            }
        }
        //if raycast hit nothing
        else
        {
            if (_outlineIsActive)
            {
                RemoveOutline();
            }


        }

    }



    private void Pickup()
    {
        if(_outlineIsActive && !_isHolding)
        {
            if (Helpers.IsInLayerMask(_pickupableLayers, _heldObject.layer))  //&& _heldObject.TryGetComponent<Rigidbody>(out Rigidbody _) && _heldObject.TryGetComponent<Pickupable>(out Pickupable _)
            {
                PickupObject();
            }
            else if (Helpers.IsInLayerMask(_interactableLayers, _heldObject.layer))
            {
                _heldObject.GetComponent<Interactable>().Interact();
            }
        }
    }

    private void PickupObject()
    {
        _isHolding = true;

        _cachedLayer = _heldObject.layer;

        _heldObject = _hitObj;



        _heldObject.layer = LayerMask.NameToLayer("PickedUp");


        _heldRB = _heldObject.GetComponent<Rigidbody>();
        
        _heldRB.linearVelocity = Vector3.zero;
        _heldRB.isKinematic = true;

        _heldPickupableScript = _heldObject.GetComponent<Pickupable>();

        _heldObject.transform.SetParent(HeldObjPos.transform);      //Or set it to this.transform to follow camera movement


        _heldObject.transform.rotation = _heldPickupableScript.Rotation;
        _heldObject.transform.position = HeldObjPos.transform.position;
    }



    private void Throw()
    {
        if (!_isHolding) return;

        ThrowObject();

    }


    private void ThrowObject()
    {

        _heldObject.layer = _cachedLayer;


        _heldObject.transform.parent = null;
        _heldRB.isKinematic = false;

        //_heldRB.linearVelocity = new Vector3(_characterController.velocity.x, 0.2f * _characterController.velocity.y, _characterController.velocity.z);

        _heldRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _heldPickupableScript.IsDiscrete = false;


        //_heldObject.transform.position = new Vector3(transform.position.x, HeldObjPos.transform.position.y, transform.position.z);

        _heldRB.AddForce(gameObject.transform.forward * _throwForce, ForceMode.Impulse);


        _isHolding = false;
    }


    
    private void AddOutline()
    {
        if (_heldObject.TryGetComponent<Outline>(out Outline outline)) outline.enabled = true;
        else _heldObject.AddComponent<Outline>();



        _outlineIsActive = true;
    }



    private void RemoveOutline()
    {
        _heldObject.GetComponent<Outline>().enabled = false;

        _outlineIsActive = false;
    }


    Transform GetClosestObject(Vector3 centerPos, RaycastHit[] objects)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 directionToTarget = objects[i].collider.ClosestPointOnBounds(centerPos) - centerPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;


            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = objects[i].transform;
            }
        }

        return bestTarget;
    }
}
