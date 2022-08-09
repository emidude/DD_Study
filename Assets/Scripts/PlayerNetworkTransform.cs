using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;


public class PlayerNetworkTransform : NetworkBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    [SyncVar]
    Vector3 leftHandPos;
    [SyncVar]
    Quaternion leftHandRot;
    [SyncVar]
    Vector3 rightHandPos;
    [SyncVar]
    Quaternion rightHandRot;
    [SyncVar]
    Vector3 headPos;
    [SyncVar]
    Quaternion headRot;

    SteamVR_Behaviour_Pose cL, cR, cH;
    GameObject theLocalPlayer;
    [SerializeField] private GameObject localHead;
    [SerializeField] private GameObject localLeftHand;
    [SerializeField] private GameObject localRightHand;


    public override void OnStartLocalPlayer()
    {
        // this is ONLY called on local player
        // connect to rig

        Debug.Log(gameObject.name + "Entered local start player, locating rig objects");

        // find the gaming rig in the scene and link to it
        if (theLocalPlayer == null)
        {
            theLocalPlayer = GameObject.Find("Local VR Rig");// find the rig in the scene
        }


        // now link localHMD, localHands to the Rig so that they are
        // automatically filled when the rig moves
        
        localHead = GameObject.FindWithTag("FolowHead");
        localLeftHand = GameObject.FindWithTag("LeftHand");
        localRightHand = GameObject.FindWithTag("RightHand");

        cL = localLeftHand.GetComponent<SteamVR_Behaviour_Pose>();
        cR = localRightHand.GetComponent<SteamVR_Behaviour_Pose>();

    }

    void Update()
    {
		if (isLocalPlayer)
        {
            CmdSetPositionAndRotation(
                leftHand.position,
                leftHand.rotation,
                rightHand.position,
                rightHand.rotation,
                head.position,
                head.rotation
            );
        }
        else
        {
            leftHand.position = leftHandPos;
            leftHand.rotation = leftHandRot;
            rightHand.position = rightHandPos;
            rightHand.rotation = rightHandRot;
            head.position = headPos;
            head.rotation = headRot;
        }
	}


    [Command]
    void CmdSetPositionAndRotation(
        Vector3 lhp,
        Quaternion lhr,
        Vector3 rhp,
        Quaternion rhr,
        Vector3 hp,
        Quaternion hr
    )
    {
        leftHandPos = lhp;
        leftHandRot = lhr;
        rightHandPos = rhp;
        rightHandRot = rhr;
        headPos = hp;
        headRot = hr;
    }

}
