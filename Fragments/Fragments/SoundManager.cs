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
				if(instance == null)
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

		private bool playing;

		#endregion

		#region Properties

		public ContentManager Content
		{
			set
			{
				content = value;
			}
		}

		#endregion

		#region Constructor

		public SoundManager()
		{
			MediaPlayer.IsRepeating = true;
			playing = false;

			songs = new Dictionary<string, Song>();
			soundEffects = new Dictionary<string, SoundEffect>();
		}

		#endregion

		//Methods
		public void PlaySong(string file)
		{
			try
			{
				if(playing)
				{
					MediaPlayer.Stop();
				}

				//If the song wasn't used yet, load it in
				if(!songs.ContainsKey(file))
				{
					songs.Add(file, content.Load<Song>(file));
				}
				MediaPlayer.Play(songs[file]);
			}
			catch(Exception)
			{
			}
		}

		public void PlaySoundEffect(string file)
		{
			//If the song wasn't used yet, load it in
			if(!songs.ContainsKey(file))
			{
				soundEffects.Add(file, content.Load<SoundEffect>(file));
			}

			soundEffects[file].Play();
		}

	}
}
