// https://youtu.be/_nRzoTzeyxU

using System.Collections.Generic;
using UnityEngine;

/// This is a class script which we can use as an object to pass into the dialogue manager whenever we want to start a new dialogue.
/** This class will host all the information we need to start a new dialogue. */
[System.Serializable]
public class Dialogue
{
    public string NameOfSpeaker;
    public Sprite ImageOfSpeaker;

    [TextArea(3, 10)]
    public List<string> SentencesToSpeak;
}