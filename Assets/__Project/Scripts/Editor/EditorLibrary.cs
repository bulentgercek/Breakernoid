using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor Method Library
/// </summary>
public class EditorLibrary : Editor
{
    /// <summary>
    /// Converting the MethodInfo Array to readable string. Example : MethodName (PType Param, ...)
    /// </summary>
    /// <param name="methodArray"></param>
    /// <returns>string</returns>
    public string MethodInfoArrayToString(MethodInfo[] methodArray)
    {
        string outputString = "";

        // Loop on methods array to build string
        foreach (var method in methodArray)
        {
            // Add method name
            outputString += method.Name;

            // Get parameters of method
            var parameterList = method.GetParameters();

            int counter = 0;

            // Loop on method's parameters
            foreach (var parameter in parameterList)
            {
                // Split the parameter name and namespace. Example : UnityEngine.Audioclip -> Audioclip
                string[] parameterType = parameter.ParameterType.ToString().Split('.');

                // Add "(" to start and add "," after every parameter and finish with ")"
                if (counter == 0) outputString += " (";
                outputString += parameterType[1] + " " + parameter.Name;
                if (counter < parameterList.Length - 1) outputString += ", ";
                if (counter == parameterList.Length - 1) outputString += ")";
                counter++;
            }

            // Add endline at the end of string
            outputString += "\n";
        }

        return outputString;
    }
}