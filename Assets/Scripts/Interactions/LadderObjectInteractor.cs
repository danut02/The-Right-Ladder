using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LadderObjectInteractor : MonoBehaviour
{
    [SerializeField] private Transform secondInteractionPoint;
    [SerializeField] private float secondInteractionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private ILadderObjectInteractable _interactable;
    void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(secondInteractionPoint.position, secondInteractionPointRadius, _colliders,
            _interactableMask);
        
        if(_numFound >0)
        {
            _interactable = _colliders[0].GetComponent<ILadderObjectInteractable >();

            if (_interactable != null)
            {
               if (Input.GetKeyDown(KeyCode.E)) _interactable.Interact(this);
            }
                
        }
        else
        {
            _interactable = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(secondInteractionPoint.position,secondInteractionPointRadius);
    }
}