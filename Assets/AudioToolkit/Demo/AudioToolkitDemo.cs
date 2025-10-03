#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1
#define UNITY_AUDIO_FEATURES_4_0
#else
#define UNITY_AUDIO_FEATURES_4_1
#endif

using UnityEngine;
using System.Collections.Generic;
using CS.AudioToolkit;
using UnityEngine.SceneManagement;
using CS.Essentials;

#pragma warning disable 1591 // undocumented XML code warning

namespace CS.AudioToolkit.Demo
{
    public class AudioToolkitDemo : MonoBehaviour
    {
        public AudioClip customAudioClip;

        float musicVolume = 1;
        float ambienceVolume = 1;
        bool musicPaused = false;

        Vector2 playlistScrollPos = Vector2.zero;

        AudioObject introLoopOutroAudio;

        void OnGUI()
        {
            DrawGuiLeftSide();
            DrawGuiRightSide();
            DrawGuiBottom();
        }

        void DrawGuiLeftSide()
        {
            var headerStyle = new GUIStyle( GUI.skin.label );
            headerStyle.normal.textColor = new UnityEngine.Color( 1, 1, 0.5f );
            string txt = string.Format( "ClockStone Audio Toolkit Free Version - Demo" );
            GUI.Label( new Rect( 22, 10, 500, 20 ), txt, headerStyle );

            int ypos = 10;
            int yposOff = 35;
            int buttonWidth = 200;
            int scrollbarWidth = 130;

            ypos += 30;

            if( !AudioController.DoesInstanceExist() )
            {
                GUI.Label( new Rect( 250, ypos, buttonWidth, 30 ), "No Audio Controller found!" );
                return;
            }

            GUI.Label( new Rect( 250, ypos, buttonWidth, 30 ), "Global Volume" );

            AudioController.SetGlobalVolume( GUI.HorizontalSlider( new Rect( 250, ypos + 20, scrollbarWidth, 30 ), AudioController.GetGlobalVolume(), 0, 1 ) );


            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Cross-fade to music track 1" ) )
            {
                AudioController.PlayMusic( "MusicTrack1" );
            }

            ypos += yposOff;

            GUI.Label( new Rect( 250, ypos + 10, buttonWidth, 30 ), "Music Volume" );

            float musicVolumeNew = GUI.HorizontalSlider( new Rect( 250, ypos + 30, scrollbarWidth, 30 ), musicVolume, 0, 1 );

            if( musicVolumeNew != musicVolume )
            {
                musicVolume = musicVolumeNew;
                AudioController.SetCategoryVolume( "Music", musicVolume );
            }

            GUI.Label( new Rect( 250 + scrollbarWidth + 30, ypos + 10, buttonWidth, 30 ), "Ambience Volume" );

            float ambienceVolumeNew = GUI.HorizontalSlider( new Rect( 250 + scrollbarWidth + 30, ypos + 30, scrollbarWidth, 30 ), ambienceVolume, 0, 1 );

            if( ambienceVolumeNew != ambienceVolume )
            {
                ambienceVolume = ambienceVolumeNew;
                AudioController.SetCategoryVolume( "Ambience", ambienceVolume );
            }

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Cross-fade to music track 2" ) )
            {
                AudioController.PlayMusic( "MusicTrack2" );
            }

            ypos += yposOff;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Fade out music category" ) )
            {
                AudioController.FadeOutCategory( "Music", 2 );
            }

            ypos += yposOff;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Fade in music category" ) )
            {
                AudioController.FadeInCategory( "Music", 2 );
            }

            ypos += yposOff;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Stop Music" ) )
            {
                AudioController.StopChannel( AudioChannelType.Music, 0.3f );
            }

            //if ( GUI.Button( new Rect( 250, ypos, buttonWidth, 30 ), "Stop Ambience" ) )
            //{
            //    AudioController.StopAmbienceSound( 0.5f );
            //}

            ypos += yposOff;

            bool musicPausedNew = GUI.Toggle( new Rect( 20, ypos, buttonWidth, 30 ), musicPaused, "Pause All Audio" );

            if( musicPausedNew != musicPaused )
            {
                musicPaused = musicPausedNew;

                if( musicPaused )
                {
                    AudioController.PauseAll( 0.1f );
                }
                else
                    AudioController.UnpauseAll( 0.1f );
            }

            ypos += 25;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Play Sound with random pitch" ) )
            {
                AudioController.Play( "RandomPitchSound" );
            }
            ypos += yposOff;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Play Sound with alternatives" ) )
            {
                AudioObject soundObj = AudioController.Play( "AlternativeSound" );
                if( soundObj != null ) soundObj.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
            }
            ypos += yposOff;

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Play Both" ) )
            {
                AudioObject soundObj = AudioController.Play( "RandomAndAlternativeSound" );
                if( soundObj != null ) soundObj.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
            }
            ypos += yposOff;

            GUI.Label( new Rect( 20, ypos, 100, 20 ), "Playlists: " );

            ypos += 20;

            playlistScrollPos = GUI.BeginScrollView( new Rect( 20, ypos, buttonWidth, 100 ), playlistScrollPos,
                new Rect( 0, 0, buttonWidth, 33f * AudioController.Instance.playlists.Length ) );

            for( int i = 0; i < AudioController.Instance.playlists.Length; ++i )
            {
                if( GUI.Button( new Rect( 20, i * 33f, buttonWidth - 20, 30f ), AudioController.Instance.playlists[i].name ) )
                {
                    AudioController.SetCurrentPlaylist( AudioController.Instance.playlists[i].name, AudioChannelType.Music );
                }
            }

            ypos += 105;

            GUI.EndScrollView();

            if( GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Play Music Playlist" ) )
            {
                AudioController.PlayPlaylist();
            }

            ypos += yposOff;

            if( AudioController.IsPlaylistPlaying( AudioChannelType.Music ) && GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Next Track on Playlist" ) )
            {
                AudioController.JumpToNextOnPlaylist( AudioChannelType.Music );
            }

            ypos += 32;

            if( AudioController.IsPlaylistPlaying() && GUI.Button( new Rect( 20, ypos, buttonWidth, 30 ), "Previous Track on Playlist" ) )
            {
                AudioController.JumpToPreviousOnPlaylist( AudioChannelType.Music );
            }

            var musicSettings = AudioController.GetAudioChannel( AudioChannelType.Music ).settings;

            ypos += yposOff;
            musicSettings.loopPlaylist = GUI.Toggle( new Rect( 20, ypos, buttonWidth, 30 ), musicSettings.loopPlaylist, "Loop Playlist" );
            ypos += 20;
            musicSettings.shufflePlaylist = GUI.Toggle( new Rect( 20, ypos, buttonWidth, 30 ), musicSettings.shufflePlaylist, "Shuffle Playlist " );
            ypos += 20;
            AudioController.Instance.soundMuted = GUI.Toggle( new Rect( 20, ypos, buttonWidth, 30 ), AudioController.Instance.soundMuted, "Sound Muted" );
        }

        bool wasClipAdded = false;
        bool wasCategoryAdded = false;

        void DrawGuiRightSide()
        {
            int ypos = 50;
            int yposOff = 35;
            int buttonWidth = 300;

            if( !wasCategoryAdded )
            {
                if( customAudioClip != null && GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Create new category with custom AudioClip" ) )
                {
                    var category = AudioController.GetCategory( "Music" );
                    //AudioController.AddToCategory( category, customAudioClip, "CustomAudioItem" );

                    var audioItemTension = new AudioItem
                    {
                        Name = "CustomAudioItem",
                        SubItemPickMode = AudioPickSubItemMode.RandomNotSameTwice
                    };
                    var asub0 = new AudioSubItem
                    {
                        Clip = customAudioClip,
                        Volume = 1f
                    };
                    var asub1 = new AudioSubItem
                    {
                        Clip = customAudioClip,
                        Volume = 1f
                    };

                    audioItemTension.AddAudioSubItem( asub0 );
                    audioItemTension.AddAudioSubItem( asub1 );

                    category.AddAudioItem( audioItemTension );

                    wasClipAdded = true;
                    wasCategoryAdded = true;
                }
            }
            else
            {
                if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Play custom AudioClip" ) )
                {
                    AudioController.Play( "CustomAudioItem" );
                }

                if( wasClipAdded )
                {

                    ypos += yposOff;

                    if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Remove custom AudioClip" ) )
                    {
                        if( AudioController.RemoveAudioItem( "CustomAudioItem" ) )
                        {
                            wasClipAdded = false;
                        }
                    }
                }
            }

            ypos = 130;

#if !UNITY_AUDIO_FEATURES_4_1
        BeginDisabledGroup( true );
#endif

            if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Play gapless audio loop" ) )
            {
                var ao = AudioController.Play( "GaplessLoopTest" );
                if( ao )
                {
                    ao.Stop( 1, 4 );
                }
                else
                {
                    // this can happen if you press the button twice really quickly so the audio is skipped because of the "Min Time Between Play" setting
                }
            }
            ypos += yposOff;

            if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Play random loop sequence" ) )
            {
                var go = new GameObject( "DummyParent" );
                go.transform.position = new Vector3( -20, 0, 0 );
                AudioController.Play( "RandomLoopSequence", go.transform );
            }
            ypos += yposOff;

            if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 50 ), "Play intro-loop-outro sequence\ngatling gun" ) )
            {
                introLoopOutroAudio = AudioController.Play( "IntroLoopOutro_Gun" );
            }

            ypos += 20;
            ypos += yposOff;

            BeginDisabledGroup( !( introLoopOutroAudio != null && introLoopOutroAudio != null ) );

            if( GUI.Button( new Rect( Screen.width - ( buttonWidth + 20 ), ypos, buttonWidth, 30 ), "Finish gatling gun sequence" ) )
            {
                introLoopOutroAudio.FinishSequence();
            }

            EndDisabledGroup();

#if !UNITY_AUDIO_FEATURES_4_1
        EndDisabledGroup();
#endif

        ypos += 60;

        const float textWidth = 500;

        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        GUI.skin.box.wordWrap = true;
        GUI.skin.box.richText = true;

        const string infoText =
            "<size=18><color=orange>Welcome to Audio Toolkit!\n</color></size>" +
            "<size=14>The number one audio management solution for Unity used in AAA titles!\n\n" +
            "What does the toolkit do? In a nutshell:\n" + 
            "1) It separates scripting from managing audio:\n" +
            " Let your audio artist define complex behaviours of what 'MySoundID' will sound like. All within the Unity inspector.\n" +
            "2) Trigger audio without any scripting knowledge using the example behaviours like <color=lightblue>PlayAudio</color> or by script with\n" +
            " a simple function call, e.g. <color=lightblue>AudioController.Play( \"MySoundID\" );</color>\n"+ 
            "3) It makes life much easier in many ways: control volume by categories, play random effects, chain sequences of sound files, define sound alternatives, manage playlists, ...\n" +
            "\n<color=cyan>Select the AudioController game object to see how to configure audio in the inspector!</color>" + 
            "</size>";

        GUI.Box( new Rect( Screen.width - textWidth, ypos, textWidth - 10 , Screen.height - ypos - 60 ), infoText );

        }

        void DrawGuiBottom()
        {
            if( GUI.Button( new Rect( Screen.width / 2 - 150, Screen.height - 40, 300, 30 ), "Video tutorial & more info..." ) )
            {
                Application.OpenURL( "http://unity.clockstone.com" );
            }
        }
        void OnAudioCompleteleyPlayed( AudioObject audioObj )
        {
            Debug.Log( "Finished playing " + audioObj.audioID + " with clip " + audioObj.primaryAudioSource.clip.name );
        }

        List<bool> disableGUILevels = new List<bool>();

        void BeginDisabledGroup( bool condition )
        {
            disableGUILevels.Add( condition );
            GUI.enabled = !IsGUIDisabled();
        }

        void EndDisabledGroup()
        {
            var count = disableGUILevels.Count;
            if( count > 0 )
            {
                disableGUILevels.RemoveAt( count - 1 );
                GUI.enabled = !IsGUIDisabled();
            }
            else
                Debug.LogWarning( "misplaced EndDisabledGroup" );
        }

        bool IsGUIDisabled()
        {
            foreach( var b in disableGUILevels )
            {
                if( b ) return true;
            }
            return false;
        }

        private void Update()
        {
            if( Input.GetKey( KeyCode.R ) )
            {
                SceneManager.LoadScene( SceneManager.GetActiveScene().name );
            }
        }
    }
}