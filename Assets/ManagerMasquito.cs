using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMasquito : MonoBehaviour
{
    const int MaxMasquito = 50;
    private int curMasquito = 0;
    private GameObject[] Masquitos = new GameObject[MaxMasquito];
    public GameObject MasquitoExample;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(curMasquito < MaxMasquito)
        {
            Masquitos[curMasquito] =Instantiate( MasquitoExample, new Vector3(3, 3, 0),MasquitoExample.transform.rotation);
            Masquitos[curMasquito].SetActive( true);
            curMasquito++;
        }
        
    }
}
