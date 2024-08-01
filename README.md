# NetTouchInput

`NetTouchInput` Unity için bir dokunma ve kaydırma işleyici sınıfıdır. Bu sınıf, farklı dokunma türlerini ve kaydırmaları algılamak ve bunlara tepki vermek için çeşitli özellikler ve olaylar sağlar.

## Özellikler

- Tek, çift ve üçlü dokunma algılama
- Kaydırma yönlerini ve yüzdelerini algılama
- Uzun basma algılama
- Multi-touch kaydırma ve dokunma destekleri

## Kurulum

1. Unity projenize `NetTouchInput` script dosyasını ekleyin.
2. `NetTouchInput` bileşenini bir GameObject'e ekleyin.

## Kullanım

### Olaylar

`NetTouchInput` sınıfı, çeşitli dokunma ve kaydırma olaylarını tetikler:

- **Swipe Events**
  - `OnRightToLeftSwipe(float swipePercentage)`: Sağdan sola kaydırma algılandığında tetiklenir.
  - `OnLeftToRightSwipe(float swipePercentage)`: Soldan sağa kaydırma algılandığında tetiklenir.
  - `OnDownToUpSwipe(float swipePercentage)`: Aşağıdan yukarıya kaydırma algılandığında tetiklenir.
  - `OnUpToDownSwipe(float swipePercentage)`: Yukarıdan aşağıya kaydırma algılandığında tetiklenir.
  - `OnDoubleSwipe(SwipeType swipeType)`: Çift kaydırma algılandığında tetiklenir.
  - `OnSwipeDetected(SwipeType swipeType, float swipePercentage)`: Kaydırma algılandığında tetiklenir.

- **Tap Events**
  - `OnSingleTap()`: Tek dokunma algılandığında tetiklenir.
  - `OnDoubleTap()`: Çift dokunma algılandığında tetiklenir.
  - `OnTripleTap()`: Üçlü dokunma algılandığında tetiklenir.
  - `OnLongPress()`: Uzun basma algılandığında tetiklenir.

- **Multi-Touch Events**
  - `OnTwoFingerSwipe(SwipeType swipeType)`: İki parmakla kaydırma algılandığında tetiklenir.
  - `OnThreeFingerSwipe(SwipeType swipeType)`: Üç parmakla kaydırma algılandığında tetiklenir.
  - `OnTwoFingerTap()`: İki parmakla dokunma algılandığında tetiklenir.
  - `OnThreeFingerTap()`: Üç parmakla dokunma algılandığında tetiklenir.
  - `OnTwoFingerLongPress()`: İki parmakla uzun basma algılandığında tetiklenir.
  - `OnThreeFingerLongPress()`: Üç parmakla uzun basma algılandığında tetiklenir.

### Statik Metodlar

Sınıfın statik metodları, çeşitli dokunma ve kaydırma durumlarını kontrol etmek için kullanılabilir:

- **SingleTap()**: Tek dokunma algılandı mı?
- **DoubleTap()**: Çift dokunma algılandı mı?
- **TripleTap()**: Üçlü dokunma algılandı mı?
- **LongPress()**: Uzun basma algılandı mı?
- **LongPress(float duration)**: Belirtilen sürede uzun basma algılandı mı?
- **Swipe(SwipeType swipeType)**: Belirtilen kaydırma türü algılandı mı?
- **TwoFingerSwipe(SwipeType swipeType)**: İki parmakla belirtilen kaydırma türü algılandı mı?
- **ThreeFingerSwipe(SwipeType swipeType)**: Üç parmakla belirtilen kaydırma türü algılandı mı?
- **TwoFingerTap()**: İki parmakla dokunma algılandı mı?
- **ThreeFingerTap()**: Üç parmakla dokunma algılandı mı?
- **TwoFingerLongPress(float duration)**: İki parmakla belirtilen sürede uzun basma algılandı mı?
- **ThreeFingerLongPress(float duration)**: Üç parmakla belirtilen sürede uzun basma algılandı mı?

## Örnekler

### Olay Kullanımı

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private NetTouchInput netTouchInput;

    void Start()
    {
        netTouchInput.OnSingleTap.AddListener(OnSingleTap);
        netTouchInput.OnDoubleTap.AddListener(OnDoubleTap);
        netTouchInput.OnSwipeDetected.AddListener(OnSwipeDetected);
    }

    private void OnSingleTap()
    {
        Debug.Log("Single Tap Detected");
    }

    private void OnDoubleTap()
    {
        Debug.Log("Double Tap Detected");
    }

    private void OnSwipeDetected(SwipeType swipeType, float swipePercentage)
    {
        Debug.Log($"Swipe Detected: {swipeType} with percentage {swipePercentage}");
    }
}

