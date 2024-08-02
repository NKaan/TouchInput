using UnityEngine;
using UnityEngine.Events;


// Enum Tanımı
public enum SwipeType
{
    RightToLeft,
    LeftToRight,
    DownToUp,
    UpToDown
}

// UnityEvent Tanımı
[System.Serializable]
public class SwipeEvent : UnityEvent<SwipeType, float> { }

public class NetTouchInput : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float horizontalTapRangePercentage = 10f; // Yüzde olarak, örneğin %10
    [SerializeField] private float verticalTapRangePercentage = 10f; // Yüzde olarak, örneğin %10
    [SerializeField] private float horizontalEdgeThresholdPercentage = 10f; // Ekran kenarına minimum mesafe yatay olarak (yüzde)
    [SerializeField] private float verticalEdgeThresholdPercentage = 10f; // Ekran kenarına minimum mesafe dikey olarak (yüzde)
    [SerializeField] private float swipeTimeThreshold = 0.3f; // Çift kaydırma algılamak için zaman penceresi
    [SerializeField] private float longPressThreshold = .7f; // Uzun basmayı algılamak için süre
    [SerializeField] private bool calculateSwipeOnEnd = true;

    [Header("Swipe Events")]
    public UnityEvent<float> OnRightToLeftSwipe;
    public UnityEvent<float> OnLeftToRightSwipe;
    public UnityEvent<float> OnDownToUpSwipe;
    public UnityEvent<float> OnUpToDownSwipe;
    public UnityEvent<SwipeType> OnDoubleSwipe; // Çift kaydırma olayı
    public SwipeEvent OnSwipeDetected; // SwipeType ve yüzde ile olay

    [Header("Tap Events")]
    public UnityEvent OnSingleTap;
    public UnityEvent OnDoubleTap;
    public UnityEvent OnTripleTap;
    public UnityEvent OnLongPress;

    [Header("Multi-Touch Events")]
    public UnityEvent<SwipeType> OnTwoFingerSwipe;
    public UnityEvent<SwipeType> OnThreeFingerSwipe;
    public UnityEvent OnTwoFingerTap;
    public UnityEvent OnThreeFingerTap;
    public UnityEvent OnTwoFingerLongPress;
    public UnityEvent OnThreeFingerLongPress;

    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private bool stopTouch;

    private float lastSwipeTime = -1f;
    private SwipeType lastSwipeType;

    private int tapCount = 0;
    private float lastTapTime = -1f;
    private float tapTimeThreshold = 0.23f; // Birden fazla dokunmayı algılamak için zaman penceresi

    private bool longPressDetected = false;
    private float touchStartTime = -1f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouch(Input.GetTouch(0));
        }
        else if (Application.isEditor)
        {
            HandleMouseInput();
        }

        // Multi-touch olaylarını kontrol edin ve eventleri tetikleyin
        CheckMultiTouchEvents();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackPressed();
        }
    }

    private void CheckMultiTouchEvents()
    {
        // İki parmak kaydırma
        if (Input.touchCount == 2)
        {
            SwipeType swipeType = DetermineSwipeType(currentPosition - startTouchPosition);
            if (swipeType == SwipeType.RightToLeft ||
                swipeType == SwipeType.LeftToRight ||
                swipeType == SwipeType.UpToDown ||
                swipeType == SwipeType.DownToUp)
            {
                OnTwoFingerSwipe?.Invoke(swipeType);
            }
        }

        // Üç parmak kaydırma
        if (Input.touchCount == 3)
        {
            SwipeType swipeType = DetermineSwipeType(currentPosition - startTouchPosition);
            if (swipeType == SwipeType.RightToLeft ||
                swipeType == SwipeType.LeftToRight ||
                swipeType == SwipeType.UpToDown ||
                swipeType == SwipeType.DownToUp)
            {
                OnThreeFingerSwipe?.Invoke(swipeType);
            }
        }

        // İki parmak dokunma
        if (Input.touchCount == 2 && tapCount == 1)
        {
            OnTwoFingerTap?.Invoke();
        }

        // Üç parmak dokunma
        if (Input.touchCount == 3 && tapCount == 1)
        {
            OnThreeFingerTap?.Invoke();
        }

        // İki parmak uzun basma
        if (Input.touchCount == 2 && LongPress(longPressThreshold))
        {
            OnTwoFingerLongPress?.Invoke();
        }

        // Üç parmak uzun basma
        if (Input.touchCount == 3 && LongPress(longPressThreshold))
        {
            OnThreeFingerLongPress?.Invoke();
        }
    }

    private void HandleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startTouchPosition = touch.position;
                stopTouch = false;
                touchStartTime = Time.time;
                longPressDetected = false;
                ProcessTap();
                break;

            case TouchPhase.Moved:
                currentPosition = touch.position;
                if (calculateSwipeOnEnd)
                {
                    DetectSwipe();
                }
                else
                {
                    // Kaydırma işlemi için geçici bir işaretçi ayarla
                    stopTouch = false;
                }
                if (Time.time - touchStartTime >= longPressThreshold && !longPressDetected)
                {
                    longPressDetected = true;
                    OnLongPress?.Invoke();
                }
                break;

            case TouchPhase.Ended:
                if (!calculateSwipeOnEnd)
                {
                    DetectSwipe();
                }
                stopTouch = true;
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            stopTouch = false;
            touchStartTime = Time.time;
            longPressDetected = false;
            ProcessTap();
        }
        else if (Input.GetMouseButton(0))
        {
            currentPosition = Input.mousePosition;
            if (calculateSwipeOnEnd)
            {
                DetectSwipe();
            }
            else
            {
                // Kaydırma işlemi için geçici bir işaretçi ayarla
                stopTouch = false;
            }
            if (Time.time - touchStartTime >= longPressThreshold && !longPressDetected)
            {
                longPressDetected = true;
                OnLongPress?.Invoke();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!calculateSwipeOnEnd)
            {
                DetectSwipe();
            }
            stopTouch = true;
        }
    }

    private SwipeType DetermineSwipeType(Vector2 direction)
    {
        float horizontalMovement = direction.x;
        float verticalMovement = direction.y;
        float horizontalAbs = Mathf.Abs(horizontalMovement);
        float verticalAbs = Mathf.Abs(verticalMovement);

        float horizontalTapRange = GetHorizontalTapRange();
        float verticalTapRange = GetVerticalTapRange();

        if (horizontalAbs > verticalAbs)
        {
            // Horizontal Swipe
            if (horizontalMovement > horizontalTapRange)
            {
                return SwipeType.LeftToRight;
            }
            if (horizontalMovement < -horizontalTapRange)
            {
                return SwipeType.RightToLeft;
            }
        }
        else if (verticalAbs > horizontalAbs)
        {
            // Vertical Swipe
            if (verticalMovement > verticalTapRange)
            {
                return SwipeType.DownToUp;
            }
            if (verticalMovement < -verticalTapRange)
            {
                return SwipeType.UpToDown;
            }
        }

        return SwipeType.RightToLeft; // Default
    }

    private void DetectSwipe()
    {
        if (stopTouch) return;

        float distance = (currentPosition - startTouchPosition).magnitude;
        float horizontalTapRange = GetHorizontalTapRange();
        float verticalTapRange = GetVerticalTapRange();

        if (distance <= horizontalTapRange || distance <= verticalTapRange) return;

        Vector2 direction = currentPosition - startTouchPosition;
        float swipePercentage = CalculateSwipePercentage(startTouchPosition, currentPosition);

        if (!IsWithinEdgeThreshold(startTouchPosition)) return;

        SwipeType swipeType = DetermineSwipeType(direction);

        if (IsDoubleSwipe(swipeType))
        {
            OnDoubleSwipe?.Invoke(swipeType);
        }
        else
        {
            SwipeTrigger(swipeType, swipePercentage);
            OnSwipeDetected?.Invoke(swipeType, swipePercentage);
            lastSwipeType = swipeType;
            lastSwipeTime = Time.time;
        }

        stopTouch = true;
    }

    private void ProcessTap()
    {
        if (Time.time - lastTapTime <= tapTimeThreshold)
        {
            tapCount++;
        }
        else
        {
            tapCount = 1;
        }

        lastTapTime = Time.time;

        if (tapCount == 1)
        {
            Invoke("CheckSingleTap", tapTimeThreshold);
        }
        else if (tapCount == 2)
        {
            CancelInvoke("CheckSingleTap");
            Invoke("CheckDoubleTap", tapTimeThreshold);
        }
        else if (tapCount == 3)
        {
            CancelInvoke("CheckSingleTap");
            CancelInvoke("CheckDoubleTap");
            OnTripleTap?.Invoke();
            tapCount = 0;
        }
    }

    // Ekran boyutlarına göre yüzdesel aralıkları hesapla
    private float GetHorizontalTapRange()
    {
        return Screen.width * (horizontalTapRangePercentage / 100f);
    }

    private float GetVerticalTapRange()
    {
        return Screen.height * (verticalTapRangePercentage / 100f);
    }

    private void CheckSingleTap()
    {
        if (tapCount == 1)
        {
            OnSingleTap?.Invoke();
        }
    }

    private void CheckDoubleTap()
    {
        if (tapCount == 2)
        {
            OnDoubleTap?.Invoke();
        }
    }

    private bool IsDoubleSwipe(SwipeType swipeType)
    {
        if (lastSwipeType == swipeType && Time.time - lastSwipeTime <= swipeTimeThreshold)
        {
            return true;
        }
        return false;
    }

    private bool IsWithinEdgeThreshold(Vector2 position)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float horizontalThreshold = screenWidth * (horizontalEdgeThresholdPercentage / 100f);
        float verticalThreshold = screenHeight * (verticalEdgeThresholdPercentage / 100f);

        return position.x < horizontalThreshold || position.x > screenWidth - horizontalThreshold ||
                position.y < verticalThreshold || position.y > screenHeight - verticalThreshold;
    }

    private float CalculateSwipePercentage(Vector2 start, Vector2 end)
    {
        float distanceX = Mathf.Abs(end.x - start.x);
        float distanceY = Mathf.Abs(end.y - start.y);

        float percentageX = distanceX / Screen.width * 100f;
        float percentageY = distanceY / Screen.height * 100f;

        return Mathf.Max(percentageX, percentageY);
    }

    private void SwipeTrigger(SwipeType swipeType, float swipePercentage)
    {
        switch (swipeType)
        {
            case SwipeType.RightToLeft:
                OnRightToLeftSwipe?.Invoke(swipePercentage);
                break;
            case SwipeType.LeftToRight:
                OnLeftToRightSwipe?.Invoke(swipePercentage);
                break;
            case SwipeType.DownToUp:
                OnDownToUpSwipe?.Invoke(swipePercentage);
                break;
            case SwipeType.UpToDown:
                OnUpToDownSwipe?.Invoke(swipePercentage);
                break;
        }
    }

    private void OnBackPressed()
    {
        // Back button handling
    }

    #region Statik Fonksiyonlar

    public static bool SingleTap()
    {
        return Instance?.tapCount == 1;
    }

    public static bool DoubleTap()
    {
        return Instance?.tapCount == 2;
    }

    public static bool TripleTap()
    {
        return Instance?.tapCount == 3;
    }

    public static bool LongPress()
    {
        return Instance?.longPressDetected ?? false;
    }

    public static bool LongPress(float duration)
    {
        return Instance != null && Instance.longPressDetected && Time.time - Instance.touchStartTime >= duration;
    }

    public static bool Swipe(SwipeType swipeType)
    {
        return Instance?.lastSwipeType == swipeType && Time.time - Instance?.lastSwipeTime <= Instance?.swipeTimeThreshold;
    }

    public static bool TwoFingerSwipe(SwipeType swipeType)
    {
        if (Instance == null) return false;
        if (Input.touchCount != 2) return false;

        SwipeType detectedSwipeType = Instance.DetermineSwipeType(Instance.currentPosition - Instance.startTouchPosition);
        if (detectedSwipeType == swipeType)
        {
            return true;
        }
        return false;
    }

    public static bool ThreeFingerSwipe(SwipeType swipeType)
    {
        if (Instance == null) return false;
        if (Input.touchCount != 3) return false;

        SwipeType detectedSwipeType = Instance.DetermineSwipeType(Instance.currentPosition - Instance.startTouchPosition);
        if (detectedSwipeType == swipeType)
        {
            return true;
        }
        return false;
    }

    public static bool TwoFingerTap()
    {
        if (Instance != null && Input.touchCount == 2 && Instance.tapCount == 1)
        {
            return true;
        }
        return false;
    }

    public static bool ThreeFingerTap()
    {
        if (Instance != null && Input.touchCount == 3 && Instance.tapCount == 1)
        {
            return true;
        }
        return false;
    }

    public static bool TwoFingerLongPress(float duration)
    {
        if (Instance != null && Input.touchCount == 2 && LongPress(duration))
        {
            return true;
        }
        return false;
    }

    public static bool ThreeFingerLongPress(float duration)
    {
        if (Instance != null && Input.touchCount == 3 && LongPress(duration))
        {
            Instance.OnThreeFingerLongPress?.Invoke();
            return true;
        }
        return false;
    }

    #endregion

    private static NetTouchInput _instance;
    public static NetTouchInput Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetTouchInput>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("NetTouchInput");
                    _instance = obj.AddComponent<NetTouchInput>();
                }
            }
            return _instance;
        }
    }
}
