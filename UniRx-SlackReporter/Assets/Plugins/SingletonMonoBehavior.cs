using UnityEngine;

public class SingletonMonoBehaviour<Type> : MonoBehaviour where Type : SingletonMonoBehaviour<Type>
{
    protected static Type instance;
    public static Type Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Type)FindObjectOfType(typeof(Type));

                if (instance == null)
                    Debug.LogWarning(typeof(Type) + "is nothing");
            }

            return instance;
        }
    }

    protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (Type)this;
            return true;
        }

        if (Instance == this)
            return true;

        Destroy(this);
        return false;
    }
}