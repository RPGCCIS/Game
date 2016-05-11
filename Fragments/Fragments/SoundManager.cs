using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Fragments
{
    /// <summary>
    /// Music player!
    /// 
    /// ---- Songs ----
    /// All Final Fantasy IV music (for testing/consistency)
    /// 
    /// Main menu - Prelude
    /// Town      - Welcome to our town
    /// Overworld - Main theme
    /// Battle    - Fight 2
    /// 
    /// </summary>
    class SoundManager
    {
        #region Singleton
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion

        #region Variables
        //Content
        private ContentManager content;

        //Sound holders
        private Dictionary<string, Song> songs;
        private Dictionary<string, SoundEffect> soundEffects;

        private bool fadingIn;
        private const float fadeConstant = 0.01f;
        #endregion

        #region Properties

        public ContentManager Content
        {
            set { content = value; }
        }

        #endregion

        #region Constructor
        public SoundManager()
        {
            MediaPlayer.IsRepeating = true;

            songs = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
        }
        #endregion

        //Methods
        public void PlaySong(string file)
        {
            //If the song wasn't used yet, load it in
            if (!songs.ContainsKey(file))
            {
                songs.Add(file, content.Load<Song>(file));
            }

            MediaPlayer.Play(songs[file]);
            MediaPlayer.IsRepeating = true;
            //for some reason this breaks with a nullpointerexception
            try
            {
                MediaPlayer.Volume = 0.1f;
            }
            catch (Exception) { }
            fadingIn = true;
        }

        public void PlaySoundEffect(string file)
        {
            //If the song wasn't used yet, load it in
            if (!soundEffects.ContainsKey(file))
            {
                soundEffects.Add(file, content.Load<SoundEffect>(file));
            }
            SoundEffect.MasterVolume = 1.0f;
            soundEffects[file].Play();
        }

        public void Update()
        {
            if (fadingIn)
            {
                try
                {
                    MediaPlayer.Volume += fadeConstant;
                }
                catch (Exception) { }

                if (MediaPlayer.Volume >= 1.0f)
                {
                    MediaPlayer.Volume = 1;
                    fadingIn = false;
                }
            }
        }

    }
}