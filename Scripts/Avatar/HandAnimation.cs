using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Handles hand animations for the locally. 
/// Simpler code structure than network equivalent as we do not have to handle both controllers in one script. See: 
/// <see cref="NetworkedHandAnimation"/>
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour {

    /// <summary>
    /// Contains animation hashes for all animations used by the hands.
    /// </summary>
    public class HandAnimationHashes
    {
        public const string GrabLargeString = "GrabLarge";
        public const string IdleString = "Idle";
        public const string PointString = "Point";
        public const string ThumbUpString = "ThumbUp";
        
        public readonly static int GrabLarge = Animator.StringToHash(GrabLargeString);
        public readonly static int Idle = Animator.StringToHash(IdleString);
        public readonly static int Point = Animator.StringToHash(PointString);
        public readonly static int ThumbUp = Animator.StringToHash(ThumbUpString);
    }

    /// <summary>
    /// Hand to read input from.
    /// </summary>
    [Tooltip("Hand to read input from.")]
    public Hand Hand;

    /// <summary>
    /// The current animation state of this controller.
    /// </summary>
    public int CurrentAnimationState
    {
        get;
        set;
    }

    /// <summary>
    /// All data loggers here will receive logging calls.
    /// </summary>
    [Tooltip("All data loggers here will receive logging calls.")]
    public List<Dataset> DataLoggers = new List<Dataset>();

    public delegate void OnAnimationChangeDelegate(int hash, Hand hand);
    public event OnAnimationChangeDelegate OnAnimationChange;

    private Animator animator;
    private bool touchPadState;
    private ulong touchPadButtonMask = SteamVR_Controller.ButtonMask.Touchpad;
    private ulong triggerButtonMask = SteamVR_Controller.ButtonMask.Trigger;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        CurrentAnimationState = HandAnimationHashes.Idle;
    }

    // Update is called once per frame
    void Update () {

        // Buttons
        AnimateHandOnButtonPress(triggerButtonMask, HandAnimationHashes.GrabLarge);
        AnimateHandOnButtonPress(touchPadButtonMask, HandAnimationHashes.Point);

        // Touchpad
        AnimateHandOnTouchPadInput(ref touchPadState, HandAnimationHashes.Point, HandAnimationHashes.ThumbUp);
    }

    /// <summary>
    /// Checks if a specific button has been pressed on a controller, then triggers an 
    /// appropriate animation for the hand. If the button is released, defaults to the idle state.
    /// </summary>
    /// <param name="buttonMask">Button mask for a specific Vive controller button.</param>
    /// <param name="animationHash">Animation hash for animation to play on button press.</param>
    void AnimateHandOnButtonPress(ulong buttonMask, int animationHash)
    {
        if (!Hand.isActiveAndEnabled || Hand.controller == null)
        {
            return;
        }

        if (Teleport.instance != null)
        {
            if (Teleport.instance.visible && Hand == Teleport.instance.pointerHand)
            {
                return;
            }
        }

        if (Hand.controller.GetPressDown(buttonMask))
        {
            SetAnimationState(animationHash);
        }

        if (Hand.controller.GetPressUp(buttonMask))
        {
            SetAnimationState(HandAnimationHashes.Idle);
        }
    }

    /// <summary>
    /// Checks if the touch pad is being touched on the controller, then activates one of two animations
    /// based on where it is being touched (horizontal split).
    /// </summary>
    /// <param name="touchPadState">Reference boolean for the current touch pad state.</param>
    /// <param name="topAnimationHash">Animation triggered when top half of touch pad is touched.</param>
    /// <param name="bottomAnimationHash">Animation triggered when bottom half of touch pad is touched.</param>
    void AnimateHandOnTouchPadInput(ref bool touchPadState, int topAnimationHash, int bottomAnimationHash)
    {
        if (!Hand.isActiveAndEnabled || Hand.controller == null)
        {
            return;
        }

        if (Teleport.instance != null)
        {
            if (Teleport.instance.visible && Hand == Teleport.instance.pointerHand)
            {
                // Ensure we are pointing
                SetAnimationState(HandAnimationHashes.Point);
                return;
            }
        }

        if (Hand.controller.GetPress(triggerButtonMask))
        {
            return;
        }

        Vector2 touchPadInput = Hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

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
            int activeAnimationHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            // Designate top half of touch pad
            if (touchPadInput.y > 0 && activeAnimationHash != topAnimationHash)
            {
                SetAnimationState(topAnimationHash);
            }

            // Designate bottom half of touch pad
            else if (touchPadInput.y < 0 && activeAnimationHash != bottomAnimationHash)
            {
                SetAnimationState(bottomAnimationHash);
            }
        }
        else
        {
            SetAnimationState(HandAnimationHashes.Idle);
        }
    }

    void SetAnimationState(int animationHash)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != animationHash)
        {
            animator.SetTrigger(animationHash);

            if (OnAnimationChange != null)
            {
                OnAnimationChange.Invoke(animationHash, Hand);
            }
            InvokeDataLoggers();
            CurrentAnimationState = animationHash;
        }
    }

    void InvokeDataLoggers()
    {
        foreach (Dataset dataLogger in DataLoggers)
        {
            dataLogger.LogRow();
        }
    }
}