using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Separator>() != null)
        {
        GameManager.S.EndCustomer();
        }
    }
}
