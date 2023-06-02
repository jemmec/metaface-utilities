# Metaface Utilities

A small Unity packaged for _more-easily_ utilizing the face tracking and eye gaze systems within the Meta Quest Pro headset. 

This package assumses you have the __Meta Movement__ package installed: https://github.com/oculus-samples/Unity-Movement.git

# Package Contents

## EmotionHelper

> Under development

Helper class for easily implementing controls using user emotions.

## EyeGazeHelper

> Under development

## BlinkHelper

> Requirements:
> - Facial Tracking Support

Helper class for blinking events.

### Blink Parameters

`BlinkParameters` are list of face expressions (_`OVRFaceExpression.FaceExpressions`_) and threshold objects that define what is considered to be a __blink__. By default the `BlinkParameters` are set to _Left eye closed_ and _Right eye closed_ both with a default threshold of `0.5`. A combination of different `FaceExpressions` and threshold values can be used to complicate what the system deems to be a __blink__. 

### Events

#### __`OnBlink` Event__

Invoked when the uses successfully performs a 'blink' under the constraints of the `maxEyesClosedTime` and `blinkParameters`. Passes a `BlinkEventArgs` object that currently only contains a `EyesClosedTime` property.

#### __`OnEyeClosed` Event__

Invoked when the user closes their eyes under the constraints of the `blinkParameters`.

#### __`OnEyeOpen` Event__

Invoked when the user opens their eyes under the constraints of the `blinkParameters`.

## Blink Calibration

> Not implemented

## Debugging Interfaces

This pacakge comes with a handful of debugging interfaces that can be used to quickly debug or test the aforementioned helper utilities.

#### `@/Prefabs/BlinkDebugger`

![BlinkLogs](https://user-images.githubusercontent.com/41222625/223655158-baa22201-0b63-41c0-bb61-4716fe079981.jpg)

#### `@/Prefabs/FaceExpressionDebugger`

![com DefaultCompany opartmr-20230308-173221](https://user-images.githubusercontent.com/41222625/223655195-1d1a3267-2684-4823-af19-924bfc80a06d.jpg)

#### `@/Prefabs/EyeGazeDebugger`

![com DefaultCompany opartmr-20230308-173159](https://user-images.githubusercontent.com/41222625/223655284-8cad71ea-a52f-4042-b74e-3c379fb5ab51.jpg)

