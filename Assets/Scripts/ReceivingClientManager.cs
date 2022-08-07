using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ReceivingClientManager : MonoBehaviourPun
{
    public List<Behaviour> componentsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //LocalPlayerInstance = gameObject;
            foreach (Behaviour c in componentsToDisable)
            {
                c.enabled = true;
            }
        }
        else
        {
            foreach (Behaviour c in componentsToDisable)
            {
                c.enabled = false;
            }
        }

    }

}
