using UnityEngine;

public class CenaData : MonoBehaviour
{
    public CenaDao Data;
    private void Awake()
    {
        GameObject.Instantiate(Data);
    }
}
