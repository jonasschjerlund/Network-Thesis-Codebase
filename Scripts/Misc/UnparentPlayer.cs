using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UnparentPlayer : MonoBehaviour {

    public void Invoke()
    {
        Player.instance.transform.parent = null;
        DontDestroyOnLoad(Player.instance.gameObject);
    }

}
