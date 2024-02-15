using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using static DeckLogic;
using Vector3 = UnityEngine.Vector3;

public class CardObject : MonoBehaviour
{
    public char Suit;
    public char Number;
    public float LerpAmount = 20.0f;
    public bool Movable = false;
    private bool FollowingMouse;
    public BlackJackHand ParentHand;
    public Vector3? FinalRestingPlace = null;
    // Start is called before the first frame update
    void Start()
    {
        FollowingMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowingMouse){
            Vector3 screenPoint = MouseToClampedWorldPoint(Input.mousePosition);
            Vector3 startingPoint = this.transform.position;
            Vector3 midPoint = Vector3.Lerp(startingPoint, screenPoint, LerpAmount * Time.deltaTime);
            midPoint.z = -6.0f;
            this.transform.position = midPoint;
            //Debug.Log(ParentHand.GetClosestSlot(midPoint.x));
            //Debug.Log($"{Input.mousePosition} {MouseToClampedWorldPoint(Input.mousePosition)}");
        }
        else if (FinalRestingPlace != null){
            Vector3 startingPoint = this.transform.position;
            Vector3 midPoint = Vector3.Lerp(startingPoint, (Vector3)FinalRestingPlace, LerpAmount * Time.deltaTime);
            this.transform.position = midPoint;
            if (midPoint == FinalRestingPlace)
                FinalRestingPlace = null;
        }
    }
    void OnMouseDown(){
        if (Movable){
            FollowingMouse = true;
        }
    }
    void OnMouseUp(){
        if (Movable) {
            FollowingMouse = false;
            ParentHand.MoveCardToSlot(this.gameObject,MouseToClampedWorldPoint(Input.mousePosition).x);
        }
    }

    private Vector3 MouseToClampedWorldPoint(Vector3 mousePos){
        return Camera.main.ScreenToWorldPoint(
            new Vector3(
                Mathf.Clamp(mousePos.x,0,Screen.width),
                Mathf.Clamp(mousePos.y,0,Screen.height),
                10.0f
            )
        );
    }
}

