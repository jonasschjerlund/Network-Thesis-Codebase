using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

// TODO: Convert int data to signed 8-bit integers (sbytes) to reduce network data use

/// <summary>
/// Handles networking for user hand animations. More complicated than the offline version. See:
/// <see cref="HandAnimation"/>
/// </summary>
public class NetworkedHandAnimation : NetworkBehaviour {

    /// <summary>
    /// Contains animation hashes for all animations used by the hands.
    /// </summary>
    private class HandAnimationHashes
    {
        public static int GrabLarge = Animator.StringToHash("GrabLarge");
        public static int Idle = Animator.StringToHash("Idle");
        public static int MiddleFinger = Animator.StringToHash("MiddleFinger");
        public static int Point = Animator.StringToHash("Point");
        public static int ThumbUp = Animator.StringToHash("ThumbUp");
    }

    /// <summary>
    /// Animator for the left hand.
    /// </summary>
    [Tooltip("Animator for the left hand.")]
    public Animator LeftAnimator;

    /// <summary>
    /// Animator for the right hand.
    /// </summary>
    [Tooltip("Animator for the right hand.")]
    public Animator RightAnimator;

    private ulong triggerButtonMask = SteamVR_Controller.ButtonMask.Trigger;
    private ulong touchPadButtonMask = SteamVR_Controller.ButtonMask.Touchpad;
    private Hand leftHand;
    private Hand rightHand;
    private int activeLeftHandAnimationHash;
    private int activeRightHandAnimationHash;
    private bool leftTouchPadState;
    private bool rightTouchPadState;

    // Boolean values representing different hand types (for network communication)
    private bool left = true;
    private bool right = false;

    // Unity method that gets called only for the local client
    public override void OnStartLocalPlayer()
    {
        rightHand = GetComponent<NetworkedAvatar>().rightController;
        leftHand = GetComponent<NetworkedAvatar>().leftController;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // Check for button presses
            AnimateHandOnButtonPress(leftHand, left, triggerButtonMask, HandAnimationHashes.GrabLarge);
            AnimateHandOnButtonPress(rightHand, right, triggerButtonMask, HandAnimationHashes.GrabLarge);
            AnimateHandOnButtonPress(leftHand, left, touchPadButtonMask, HandAnimationHashes.Point);
            AnimateHandOnButtonPress(rightHand, right, touchPadButtonMask, HandAnimationHashes.Point);

            // Check for touch pad
            AnimateHandOnTouchPadInput(leftHand, left, ref leftTouchPadState, HandAnimationHashes.Point, HandAnimationHashes.ThumbUp);
            AnimateHandOnTouchPadInput(rightHand, right, ref rightTouchPadState, HandAnimationHashes.Point, HandAnimationHashes.ThumbUp);
        }
    }

    /// <summary>
    /// Checks if a specific button has been pressed on a controller, then triggers an 
    /// appropriate animation for the hand. If the button is released, defaults to the idle state.
    /// </summary>
    /// <param name="hand">Hand component attached to a specific controller.</param>
    /// <param name="handSide">Corresponds to the side of the provided controller (left = true, right = false).</param>
    /// <param name="buttonMask">Button mask for a specific Vive controller button.</param>
    /// <param name="animationHash">Animation hash for animation to play on button press.</param>
    void AnimateHandOnButtonPress(Hand hand, bool handSide, ulong buttonMask, int animationHash)
    {
        if (!hand.isActiveAndEnabled || hand.controller == null)
        {
            return;
        }

        if (Teleport.instance != null)
        {
            if (Teleport.instance.visible && hand == Teleport.instance.pointerHand)
            {
                return;
            }
        }

        if (hand.controller.GetPressDown(buttonMask))
        {
            SendAnimationToNetwork(handSide, animationHash);
        }

        if (hand.controller.GetPressUp(buttonMask))
        {
            SendAnimationToNetwork(handSide, HandAnimationHashes.Idle);
        }
    }

    /// <summary>
    /// Checks if the touch pad is being touched on a controller, then activates one of two animations
    /// based on where it is being touched (horizontal split).
    /// </summary>
    /// <param name="hand">Hand component attached to a specific controller.</param>
    /// <param name="handSide">Corresponds to the side of the provided controller (left = true, right = false).</param>
    /// <param name="topAnimationHash">Animation triggered when top half of touch pad is touched.</param>
    /// <param name="bottomAnimationHash">Animation triggered when bottom half of touch pad is touched.</param>
    void AnimateHandOnTouchPadInput(Hand hand, bool handSide, ref bool touchPadState, int topAnimationHash, int bottomAnimationHash)
    {
        if (!hand.isActiveAndEnabled || hand.controller == null)
        {
            return;
        }

        if (Teleport.instance != null)
        {
            if (Teleport.instance.visible && hand == Teleport.instance.pointerHand)
            {
                // Ensure we are pointing
                SendAnimationToNetwork(handSide, HandAnimationHashes.Point);
                return;
            }
        }
        
        if (hand.controller.GetPress(triggerButtonMask))
        {
            return;
        }

        Vector2 touchPadInput = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

        // Determine whether touch pad is being touched or not
        if (!touchPadState && touchPadInput.magnitude != 0)
        {
            touchPadState = true;
        }
        else if (touchPadState && touchPadInput.magnitude == 0)
        {
            touchPadState = false;
        }

        // Handle touch pad state
        if (touchPadState)
        {
            int activeAnimationHash = GetAnimator(handSide).GetCurrentAnimatorStateInfo(0).shortNameHash;

            // Designate top half of touch pad
            if (touchPadInput.y > 0 && activeAnimationHash != topAnimationHash)
            {
                SendAnimationToNetwork(handSide, topAnimationHash);
            }

            // Designate bottom half of touch pad
            else if (touchPadInput.y < 0 && activeAnimationHash != bottomAnimationHash)
            {
                SendAnimationToNetwork(handSide, bottomAnimationHash);
            }
        }
        else
        {
            SendAnimationToNetwork(handSide, HandAnimationHashes.Idle);
        }
    }

    /// <summary>
    /// Returns the animator component for a controller based on a boolean value representing left and right.
    /// </summary>
    /// <param name="handSide">Corresponds to the side of the provided controller (left = true, right = false).</param>
    /// <returns>Animator for a controller.</returns>
    Animator GetAnimator(bool handSide)
    {
        if (handSide == left)
        {
            return LeftAnimator;
        }
        else
        {
            return RightAnimator;
        }
    }

    /// <summary>
    /// Networks an animation hash to a specific hand's animator component, 
    /// if that animator's state does not currently match that animation hash.
    /// Invokes a command if the local connection is a client, or a remote
    /// procedure call (RPC) if the local connection is a server.
    /// </summary>
    /// <param name="handSide">Corresponds to the side of the provided controller (left = true, right = false).</param>
    /// <param name="animationHash">Animation hash for animation to play.</param>
    /// <seealso cref="CmdSetAnimatorTrigger(bool, int)"/>
    /// <seealso cref="RpcSetAnimatorTrigger(bool, int)"/>
    void SendAnimationToNetwork(bool handSide, int animationHash)
    {
        if (GetAnimator(handSide).GetCurrentAnimatorStateInfo(0).shortNameHash != animationHash)
        {
            if (!Network.isServer)
            {
                CmdSetAnimatorTrigger(handSide, animationHash);
            }
            else
            {
                RpcSetAnimatorTrigger(handSide, animationHash);
            }
        }
    }

    [ClientRpc]
    void RpcSetAnimatorTrigger(bool handSide, int animationHash)
    {
        GetAnimator(handSide).SetTrigger(animationHash);
    }

    [Command]
    void CmdSetAnimatorTrigger(bool handSide, int animationHash)
    {
        RpcSetAnimatorTrigger(handSide, animationHash);
    }
}