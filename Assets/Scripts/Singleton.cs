using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class makes a certain object a singleton, to assure that there is always exactly one instance of it
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    protected static T instance; // The instance of the object

    // Makes returns the instance if there is one stored in the instance field. If not, it tries to find one reference of the type the instance should have. 
    // If there was not any instance found, it logs an error.
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    Debug.LogErrorFormat("An instance of {0} is missing in this scene", typeof(T));
                }
            }

            return instance;
        }
    }
}
