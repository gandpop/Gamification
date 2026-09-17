using UnityEngine;

[CreateAssetMenu(fileName = "New AudioData", menuName = "AudioData")]
public class AudioData : ScriptableObject
{
    [Header("Sound Effects")]
    public AudioClip ButtonHover;
    public AudioClip ButtonClick;
    public AudioClip ErrorSound;
    public AudioClip Right;
    public AudioClip GameOver;
    public AudioClip GameWon;
}