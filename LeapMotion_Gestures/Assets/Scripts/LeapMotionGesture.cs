using Leap;
using Leap.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LeapMotionGesture : MonoBehaviour
{
    public LeapProvider leapProvider;
   
    [Header("HANDS DATA")]
    float palmVelocity;
    Vector3 palmDirection;
    
    [Header("SWIPE DATA")]
    [SerializeField] float distanceThreshold = 2f;
    [SerializeField] UnityEvent OnSwipeLeft;
    [SerializeField] UnityEvent OnSwipeRight;

    [Header("WAVE DATA")]
    [SerializeField] bool isWaving = false;
    [SerializeField] float waveStartTime;
    [SerializeField] float waveThreshold = 0.01f;
    [SerializeField] UnityEvent OnWaveTrigger;

    [Header("FINGER DATA")]
    public float fingerMagnitude;
    public Vector3 fingerTipPosition;

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }
    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        if (frame.Hands.Count > 0)
        {
            foreach (var hand in frame.Hands)
            {
                if (hand.IsRight)
                {
                    OnUpdateHand(hand);
                }
            }
        }
        else
        {
            isWaving = false;
        }
    }


    void OnUpdateHand(Hand _hand)
    {
        palmVelocity = _hand.PalmVelocity.magnitude;
        palmDirection = _hand.PalmVelocity;

        Swipe(palmVelocity, palmDirection);
        Waving(palmVelocity, palmDirection);
        Finger(_hand);
    }

    #region Swipe
    void Swipe(float palmVelocity, Vector3 palmDirection)
    {
        if (!isWaving)
        {
            if (palmVelocity >= 1.0f && palmDirection.x > distanceThreshold)
            {
                OnSwipeRight.Invoke();
                print("Swipe right");
            }
            else if (palmVelocity >= 1.0f && palmDirection.x < -distanceThreshold)
            {
                OnSwipeLeft.Invoke();
                print("Swipe left");
            }
        }
    }
   
    #endregion

    #region Wave
    void Waving(float palmVelocity, Vector3 palmDirection)
    {
        if (palmVelocity > waveThreshold && palmDirection.x < 1 && palmDirection.x > -1)
        {
            StartCoroutine(InitializeWave());
        }
        else
        {

            StopAllCoroutines();
            isWaving = false;
        }
    }

    IEnumerator InitializeWave()
    {
        yield return new WaitForSeconds(waveStartTime);
        isWaving = true;
        print("Waving");
        OnWaveTrigger.Invoke();
    }
    #endregion

    #region CircularIndexMotion
    void Finger(Hand _hand)
    {
        Finger _index = _hand.GetIndex();

        fingerTipPosition = _index.TipPosition;
        fingerMagnitude = _index.TipPosition.magnitude;
    }
    #endregion

    #region Pinch
    public void Pinched()
    {
        print("Pinched");
    }
    #endregion
}
