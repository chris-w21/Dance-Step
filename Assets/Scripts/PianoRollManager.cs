using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoRollManager : MonoBehaviour
{
    public PianoRoll roll;

    public AudioSource source;

    public bool Play = false;

    private void OnValidate()
    {
        if (Play)
        {
            Play = false;
            source.pitch = roll.key.i * 0.0289856f - 1f;
            source.PlayOneShot(roll.clip);
        }
    }
}

[System.Serializable]
public class PianoRoll
{
    public static readonly string[] KeyNotes = new string[96] { "C0", "", "", "", "", "", "", "", "", "", "", "", "C1", "", "", "", "", "", "", "", "", "", "", "", "C2", "", "", "", "", "", "", "", "", "", "", "", "C3", "", "", "", "", "", "", "", "", "", "", "", "C4", "", "", "", "", "", "", "", "", "", "", "", "C5", "", "", "", "", "", "", "", "", "", "", "", "C6", "", "", "", "", "", "", "", "", "", "", "", "C7", "", "", "", "", "", "", "", "", "", "", "" };
    public AudioClip clip;
    public Key key = new Key();
}

[System.Serializable]
public class Key
{
    [SerializeField]public int i = 69;
    public int length = 4;
    //public int i
    //{
      //  get
        //{
          //  return I;
        //}
        //set
        //{
          //  I = value * 2;
        //}
    //}
}