using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

  public AudioClip MusicClip;
  public bool bAutoPlay = true;
  public bool bLoopMusic = true;
  
  private AudioSource MusicComp;

  // Start is called before the first frame update
  void Start() {
    MusicComp = GetComponent<AudioSource>();

    if(!MusicComp)
      MusicComp = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

    if(MusicClip)
      MusicComp.clip = MusicClip;

    if(bAutoPlay)
      Play();

    MusicComp.loop = bLoopMusic;
  }

  public void Play() {
    MusicComp.Play();
  }

  public void Stop() {
    MusicComp.Stop();
  }
}
