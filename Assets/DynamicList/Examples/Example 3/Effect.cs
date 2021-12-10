using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    public abstract void Trigger();
}

public class Fireball: Effect
{
    public int Damage;

    [SerializeField]
    private float m_range;
    [SerializeField]
    private GameObject m_fireballPrefab;

    public override void Trigger()
    {
        Debug.Log("you shot a fireball " + m_range + " metters in front of you, dealing " + Damage + " to all enemies");
    }
}

public class Teleport: Effect
{
    [SerializeField]
    private int m_energyCost;

    public override void Trigger()
    {
        Debug.Log("you teleported aways, costing " + m_energyCost + " energy");
    }
}

public class IceWall: Effect
{
    [SerializeField]
    private int m_size;

    public override void Trigger()
    {
        Debug.Log("you created a " + m_size + " meters long wall of ice in front of you");
    }
}
