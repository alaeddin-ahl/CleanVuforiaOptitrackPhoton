using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SpatialTracking;
using System.Collections.Generic;

namespace EngineAssembly
{

    public class SenderClientManager : PlayerManager
    {
        public List<Behaviour> componentsToDisable;

        //public MouseObjectInteractor objectInteractor;

        public override void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
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

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }

    }

}
