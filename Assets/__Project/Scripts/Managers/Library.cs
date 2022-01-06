using System.Reflection;
using UnityEngine;

/// <summary>
/// General function library for common use
/// </summary>
public class Library : MonoBehaviour
{
    /// <summary>
    /// Custom PlayClipAtPoint function has an optional parenting attribute
    /// </summary>
    public void PlayClipAtPoint(AudioClip clip, Vector3 pos, GameObject parentObject = null)
    {
        GameObject tempObject = new GameObject("TempAudio"); // Create the temp object

        // Create new audio source object as a parent of the _DynamicObjects
        if (parentObject != null)
        {
            tempObject.transform.SetParent(parentObject.transform);
        }
        
        tempObject.transform.position = pos; // Set its position

        AudioSource audioSource = tempObject.AddComponent<AudioSource>(); // Add an audio source
        audioSource.clip = clip; // Define the clip
        audioSource.Play(); // Play the Sound
        Destroy(tempObject, clip.length); // Destroy temmp object after clip duration
    }

    /// <summary>
    /// Convert string to int array and return
    /// </summary>   
    public int[] StringToIntArray(string data)
    {
        int[] intData = new int[data.Length];
        int count = 0;

        foreach (char c in data)
        {
            intData[count] = c;

            // The -48 is because 0 is 48 (0x0030) in Unicode value
            // and you need to subtract that value to get the integer representation.
            intData[count] -= 48;

            count++;
        }
        
        return intData;
    }

    /// <summary>
    /// (WIP) Field Debug Method
    /// </summary>
    /// <param name="scriptComponent"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public bool FieldDebugger(Component scriptComponent, string fieldName)
    {
        System.Type fieldObjectType = scriptComponent.GetType();
        FieldInfo fieldInfo = fieldObjectType.GetField(fieldName);

        if (fieldInfo.GetValue(scriptComponent) == null || fieldInfo.GetValue(scriptComponent).ToString() == "null")
        {
            Debug.LogWarning("Field missing in " + scriptComponent + " -> " + fieldInfo);
            return false;
        }
        return true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
