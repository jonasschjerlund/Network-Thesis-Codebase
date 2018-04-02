using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Scales a Unity game object to a specific value.
/// </summary>
public class GenericScaler : NetworkBehaviour {

    [Range(0,10)]
    [SyncVar (hook = "OnChangeScale")]
    [Tooltip("Represents the X, Y and Z scale for this object.")]
    public float Scale = 1.0f;

    public float TimeInSeconds = 3.0f;

    public bool InstantScale;

    public bool ScaleOverTime;

    private bool changingScale;

	// Update is called once per frame
	void Update () {

        if (InstantScale)
        {
            CmdApplyScale(Scale);
            InstantScale = false;
        }

        if (ScaleOverTime)
        {
            CmdGrowToScaleOverTime(Scale);
            ScaleOverTime = false;
        }
	}

    [Command]
    public void CmdApplyScale(float scale)
    {
        RpcApplyScale(scale);
    }

    // "Event" method triggered by the SyncVar hook of the Scale attribute that does an RPC call to apply scale changes across clients
    void OnChangeScale(float scale)
    {
        CmdApplyScale(scale);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        CmdApplyScale(Scale);
    }

    [ClientRpc] 
    public void RpcApplyScale(float scale)
    {
        Scale = scale;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    [Command]
    public void CmdGrowToScaleOverTime(float scale)
    {
        RpcGrowToScaleOverTime(scale);
    }

    [ClientRpc]
    public void RpcGrowToScaleOverTime(float scale)
    {
        StartCoroutine(GrowToSpecificScale(scale));
    }

    IEnumerator GrowToSpecificScale(float targetScale)
    {
        if (changingScale) yield return null;
        changingScale = true;

        if (targetScale > transform.localScale.x)
        {
            while (targetScale > transform.localScale.x)
            {
                transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            while (targetScale < transform.localScale.x)
            {
                transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                yield return new WaitForSeconds(0.02f);
            }
        }

        changingScale = false;
    }
}