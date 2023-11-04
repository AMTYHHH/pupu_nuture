using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudioSystem{
    [CreateAssetMenu(fileName = "AudioInfo_SO", menuName = "DevelopBasic/AudioSystem/AudioInfo_SO")]
    public class AudioInfo_SO : ScriptableObject
    {
        public List<AudioInfo> bgm_info_list;
        public List<AudioInfo> amb_info_list;
        public List<AudioInfo> sfx_info_list;
        public AudioClip GetBGMClipByName(string audio_name){
            return bgm_info_list.Find(x=>x.audio_name == audio_name).audio_clip;
        }
        public AudioClip GetAMBClipByName(string audio_name){
            return amb_info_list.Find(x=>x.audio_name == audio_name).audio_clip;
        }
        public AudioClip GetSFXClipByName(string audio_name){
            return sfx_info_list.Find(x=>x.audio_name == audio_name).audio_clip;
        }
    }
    [System.Serializable]
    public struct AudioInfo{
        public string audio_name;
        public AudioClip audio_clip;
    }
}
