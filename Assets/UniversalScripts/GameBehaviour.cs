using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : JMC
{
    protected static NetworkPlayerCardSpot _NPCS { get { return NetworkPlayerCardSpot.INSTANCE; } }
    protected static PlayerCardSpot _PCS { get { return PlayerCardSpot.INSTANCE; } }
    protected static EnemyCardSpot _ECS { get { return EnemyCardSpot.INSTANCE; } }
    protected static GameManager _GM { get { return GameManager.INSTANCE; } }

}

public class GameBehaviour<T> : GameBehaviour where T : GameBehaviour
{
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>();
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
