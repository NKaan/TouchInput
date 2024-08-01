# NetTouchInput

`NetTouchInput` is a Unity component designed to handle touch and mouse input, including swipe detection, multi-touch events, and tap events. This script supports various touch and swipe functionalities, providing flexibility for input management in Unity applications.

## Features

- **Swipe Detection**: Detects swipes in four directions (RightToLeft, LeftToRight, DownToUp, UpToDown) with customizable thresholds.
- **Tap Detection**: Supports single, double, and triple tap detection with customizable time thresholds.
- **Long Press Detection**: Detects long presses with adjustable duration.
- **Multi-Touch Support**: Handles events for two-finger and three-finger gestures, including swipe, tap, and long press.

## Getting Started

1. **Add the Component**: Attach the `NetTouchInput` script to any GameObject in your Unity scene.

2. **Configure Settings**: Adjust the settings in the Inspector window:
   - `tapRange`: Maximum distance to detect a tap.
   - `horizontalEdgeThresholdPercentage`: Minimum horizontal distance from screen edges for swipe detection.
   - `verticalEdgeThresholdPercentage`: Minimum vertical distance from screen edges for swipe detection.
   - `swipeTimeThreshold`: Time window to detect double swipes.
   - `longPressThreshold`: Duration required to detect a long press.
   - `calculateSwipeOnEnd`: If `true`, swipe detection is calculated when touch ends; if `false`, swipe detection is calculated during touch.
  
3. **Static Methods
   SingleTap(): Checks if a single tap occurred.
   DoubleTap(): Checks if a double tap occurred.
   TripleTap(): Checks if a triple tap occurred.
   LongPress(): Checks if a long press occurred.
   LongPress(float duration): Checks if a long press occurred for a specified duration.
   Swipe(SwipeType swipeType): Checks if a specific swipe type occurred.
   TwoFingerSwipe(SwipeType swipeType): Checks if a two-finger swipe of a specific type occurred.
   ThreeFingerSwipe(SwipeType swipeType): Checks if a three-finger swipe of a specific type occurred.
   TwoFingerTap(): Checks if a two-finger tap occurred.
   ThreeFingerTap(): Checks if a three-finger tap occurred.
   TwoFingerLongPress(float duration): Checks if a two-finger long press occurred for a specified duration.
   ThreeFingerLongPress(float duration): Checks if a three-finger long press occurred for a specified duration.
   Instance Methods
   Update(): Checks for input each frame and processes it.
   HandleTouch(Touch touch): Handles touch input events.
   HandleMouseInput(): Handles mouse input events.
   DetectSwipe(): Detects and triggers swipe events.
   ProcessTap(): Processes tap input and triggers appropriate events.

4. **Subscribe to Events**: Use Unity's event system to subscribe to the provided events in your scripts. For example:

   ```csharp
   public class Example : MonoBehaviour
   {
       public NetTouchInput netTouchInput;

       private void Start()
       {
           netTouchInput.OnSingleTap.AddListener(HandleSingleTap);
           netTouchInput.OnDoubleTap.AddListener(HandleDoubleTap);
           netTouchInput.OnTripleTap.AddListener(HandleTripleTap);
           netTouchInput.OnLongPress.AddListener(HandleLongPress);
       }

        void Update()
        {
            if (NetTouchInput.SingleTap())
            {
                Debug.Log("Single tap detected.");
            }
        }

       private void HandleSingleTap()
       {
           Debug.Log("Single tap detected.");
       }

       private void HandleDoubleTap()
       {
           Debug.Log("Double tap detected.");
       }

       private void HandleTripleTap()
       {
           Debug.Log("Triple tap detected.");
       }

       private void HandleLongPress()
       {
           Debug.Log("Long press detected.");
       }
   }

'''
Notes
Ensure that NetTouchInput is attached to an active GameObject in your scene for proper functionality.
Adjust event listeners and settings as needed to fit your application's requirements.
License
This project is licensed under the MIT License - see the LICENSE file for details.

Contributing
Feel free to submit issues, feature requests, or pull requests. Contributions are welcome!

Contact
For questions or feedback, please contact numankaankaratas@gmail.com
