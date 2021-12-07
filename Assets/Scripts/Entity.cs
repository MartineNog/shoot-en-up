using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int m_Max_PV;
    private int m_Current_PV;


    private void Awake()
    {
        WriteCurrentPV(5);
    }

    public int ReadCurrentPV()
    {
        return m_Current_PV;
    }
    protected void WriteCurrentPV(int curren_PV)
    {
        m_Current_PV = curren_PV;
    }

    public int ReducePV(int reduce)
    {
        return m_Current_PV - reduce;
    }

}
