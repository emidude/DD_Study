﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;


public class NetworkingPlayer : NetworkBehaviour
{
    //source gameobjects head, left and right controller object of local/host/first player
    [SerializeField] private GameObject localHead;
    [SerializeField] private GameObject localLeftHand;
    [SerializeField] private GameObject localRightHand;

    //prefabs to assign head, left, right controller for Network visbile LAN/client/second player
    [SerializeField] private GameObject networkedHead;
    [SerializeField] private GameObject networkedLeftHand;
    [SerializeField] private GameObject networkedRightHand;

    GameObject theLocalPlayer;
    SteamVR_TrackedObject trackedObjHead, trackedObjLeft, trackedObjRight;
    bool isLinkedToVR;

    GameObject defaultHead, defaultLeftHand, defaultRightHand;

    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }


        Debug.Log("Start of the vr player");

        //disabled conroller meshes at VR player side so it cannont be viewed by local player
        /*networkedHead.GetComponent<MeshRenderer>().enabled = false; //commented out for testing    
        networkedLeftHand.GetComponent<MeshRenderer>().enabled = false;
        networkedRightHand.GetComponent<MeshRenderer>().enabled = false;*/
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

      /*  if (localHead = null)
        {
            localHead = GameObject.Find("FollowHead");
        }

        if (localLeftHand = null)
        {
            localLeftHand = GameObject.Find("LeftHand");
        }

        if (localRightHand = null)
        {
            localRightHand = GameObject.Find("RightHand");
        }*/

        updateHeadAndHands();
    }


    public override void OnStartLocalPlayer()
    {
        // this is ONLY called on local player
        // connect to rig

        Debug.Log(gameObject.name + "Entered local start player, locating rig objects");
        isLinkedToVR = true; 

        // find the gaming rig in the scene and link to it
        if (theLocalPlayer == null)
        {
            theLocalPlayer = GameObject.Find("Local VR Rig");// find the rig in the scene
        }


        // now link localHMD, localHands to the Rig so that they are
        // automatically filled when the rig moves
        localHead = Camera.main.gameObject; // get HMD
        Debug.Log("local head = " + localHead);
        localLeftHand = theLocalPlayer.transform.Find("LeftHand").gameObject;
        //localLeftHand = GameObject.FindWithTag("LeftHand");
        localRightHand = theLocalPlayer.transform.Find("RightHand").gameObject;
        //localRightHand = GameObject.FindWithTag("RightHand");

        trackedObjRight = localRightHand.GetComponent<SteamVR_TrackedObject>();
        trackedObjLeft = localLeftHand.GetComponent<SteamVR_TrackedObject>();

    }

    void updateHeadAndHands()
    {

        if (!isLocalPlayer)
        {
            // do nothing, net transform does all the work for us
        }
        else
        {
            // we are the local player.
            // we copy the values from the Rig's HMD
            // and hand positions so they can be
            // used for local positioning

            // prevent headless version of app from crashing
            // depends on SteamVR version if HMD is null or simply won't move
            if (localHead == null)
            {
                localHead = defaultHead;// when running as headless, provide default non-moving objects instead
                localLeftHand = defaultLeftHand;
                localRightHand = defaultRightHand;
                Debug.Log("HEADLESS detected");
            }

                networkedHead.transform.position = localHead.transform.position;
                networkedHead.transform.rotation = localHead.transform.rotation;

                if (localLeftHand) //should return true if left controller connected
                {
                    // we need to check in case player left the hand unconnected
                    networkedLeftHand.transform.position = localLeftHand.transform.position;
                    networkedLeftHand.transform.rotation = localLeftHand.transform.rotation;
                }

                if (localRightHand)
                {
                    // only if right hand is connected
                    networkedRightHand.transform.position = localRightHand.transform.position;
                    networkedRightHand.transform.rotation = localRightHand.transform.rotation;
                }

            
        }

    }




}