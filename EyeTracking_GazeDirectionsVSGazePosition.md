Gaze direction and gaze position are two distinct concepts in the context of eye tracking and mixed reality:

## Gaze Direction
Gaze direction refers to the orientation or the vector indicating where the user is looking. It is usually represented as a normalized 3D vector (a unit vector) that points from the user's eye position in the direction they are looking. This vector doesn't have a specific endpoint; it merely indicates the direction of the gaze.
Example: If the user is looking straight ahead, the gaze direction might be (0, 0, 1).

## Gaze Position
Gaze position refers to the specific point or location in the user's field of view where their gaze intersects or focuses on. In the context of eye tracking within a 3D environment, it is often the point in space where the gaze direction intersects with objects or surfaces in the scene.
Example: If the user is looking at a specific spot on a wall, the gaze position would be the coordinates of that spot.

### In Eye Tracking and Mixed Reality:
Gaze Direction: Can be used to draw a line or ray from the user's eye into the environment. This is useful for determining what the user is looking at directionally, which can be used for aiming, selecting objects, or pointing.
Gaze Position: Is determined by finding the intersection of the gaze direction with objects in the scene. This tells you the exact point the user is focusing on, which is useful for interactions that require precise targeting, such as clicking on an interface element or focusing on a specific point on an object.

#### Example:
Imagine a user is wearing a mixed reality headset and looking at a virtual button on a wall:

Gaze Direction: The vector indicating the line from the user's eyes to the button, like (0.5, -0.2, 1.0).
Gaze Position: The coordinates of the button on the wall, like (2.0, 1.5, 5.0).
Implementation in Unity with Eye Tracking:
In the EyeTrackerOutlet example provided earlier:

Gaze Direction is obtained using gazeInteractor.rayOriginTransform.forward, which gives the forward direction vector from the eye tracker (normalized).
Gaze Position is obtained using gazeInteractor.rayOriginTransform.position, which gives the current position of the gaze origin in the world space.
