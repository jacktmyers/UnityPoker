using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static DeckLogic;
using Vector3 = UnityEngine.Vector3;

public class JacksCardObject : MonoBehaviour
{
    public bool Modifiable;
    public char Suit;
    public char Number;
    public float LerpAmount = 20.0f;
    public JacksHand ParentHand;
    public bool DestroyOnArrival;
    public bool Selected;
    public Vector3? FinalRestingPlace = null;
    // Start is called before the first frame update
    public int PointValue;
    void Start()
    {
        DestroyOnArrival = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Find("Checked").gameObject.SetActive(Selected);
        if (FinalRestingPlace != null){
            Vector3 startingPoint = this.transform.position;
            Vector3 midPoint = Vector3.Lerp(startingPoint, (Vector3)FinalRestingPlace, LerpAmount * Time.deltaTime);
            this.transform.position = midPoint;
            if (midPoint == FinalRestingPlace){
                if (DestroyOnArrival){
                    Destroy(this.gameObject);
                }
                FinalRestingPlace = null;
            }
        }
    }
    void OnMouseDown(){
        if (Modifiable){
            Selected = !Selected;
        }
    }
}

