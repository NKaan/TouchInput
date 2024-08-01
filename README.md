
    [Header("Swipe Settings")]
    [SerializeField] private float tapRange = 10f; // Maksimum dokunma mesafesi
    [SerializeField] private float horizontalEdgeThresholdPercentage = 10f; // Ekran kenarına minimum mesafe yatay olarak (yüzde)
    [SerializeField] private float verticalEdgeThresholdPercentage = 10f; // Ekran kenarına minimum mesafe dikey olarak (yüzde)
    [SerializeField] private float swipeTimeThreshold = 0.3f; // Çift kaydırma algılamak için zaman penceresi
    [SerializeField] private float longPressThreshold = 1f; // Uzun basmayı algılamak için süre
    [SerializeField] private bool calculateSwipeOnEnd = true; // Inspector üzerinden ayarlanabilir

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
