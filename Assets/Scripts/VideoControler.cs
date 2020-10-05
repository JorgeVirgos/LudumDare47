using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControler : MonoBehaviour
{

        public VideoPlayer vid;

        void Start() { vid.loopPointReached += CheckOver; }

        void CheckOver(UnityEngine.Video.VideoPlayer vp)
        {
            print("Video Is Over");
             Application.Quit();
    }

   
}
