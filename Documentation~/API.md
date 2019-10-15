
# Oculus XR Plugin APIs


### Unity.XR.Oculus.Utils.SetColorScaleAndOffset
public static void SetColorScaleAndOffset(Vector4 colorScale, Vector4 colorOffset);

**Parameters**
_colorScale_ - Scales the eye layer texture color by this Vector. Vector components are expected to be a value from 0.0 to 1.0.

_colorOffset_- Offsets the eye layer texture color by this Vector. Vector components are expected to be a value from 0.0 to 1.0.
```csharp
public class LerpColorScale : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    	// animate the color scale value 
        float value =  0.5f * (1.0f + Mathf.Sin(2.0f*Mathf.PI * 0.1f* Time.time));

        // sets the color scale and offset on the Oculus XR Plugin.
        // with the animation defined above this causes a constant "fade in" -> "fade out" effect.
        Unity.XR.Oculus.Utils.SetColorScaleAndOffset(new Vector4(value,value,value,value), Vector4.zero);
    }
}
```
