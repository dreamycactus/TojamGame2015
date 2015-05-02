/*
 *
 *
 */
using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {

    public static int State = Animator.StringToHash("State");

    public static int Idle = Animator.StringToHash("Base Layer.Idle");
    public static int FrontBite = Animator.StringToHash("Base Layer.FrontBite");
    public static int Run = Animator.StringToHash("Base Layer.Run");
    public static int Jump = Animator.StringToHash("Base Layer.Jump");
    public static int ShootStraight = Animator.StringToHash("Base Layer.ShootStraight");
    public static int Crouch = Animator.StringToHash("Base Layer.Crouch");
    public static int DownBite = Animator.StringToHash("Base Layer.DownBite");
    public static int Smear = Animator.StringToHash("Base Layer.Smear");


}
