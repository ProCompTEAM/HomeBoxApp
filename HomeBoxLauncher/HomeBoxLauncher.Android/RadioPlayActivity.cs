using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using HomeBoxLauncher.Droid.Enums;
using HomeBoxLauncher.Droid.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Orientation = Android.Widget.Orientation;

namespace HomeBoxLauncher.Droid
{
    [Activity(Label = "HomeBox Radio", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class RadioPlayActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private MediaPlayer player = new MediaPlayer();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LoadUI();

            player.SetAudioStreamType(Android.Media.Stream.Music);
            player.SetDataSource(AppSettings.StreamUrl);

            Play();

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        public override void OnBackPressed()
        {
            Stop();
            Finish();
        }

        private void LoadUI()
        {
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            layout.SetBackgroundColor(Color.Black);

            TextView label = new TextView(this);
            label.Text = AppSettings.PublicLabel;
            label.SetTextColor(Color.Yellow);
            label.SetTextSize(ComplexUnitType.Px, 72);
            label.SetPadding(0, 300, 0, 0);
            label.Gravity = GravityFlags.Center;

            layout.AddView(label);
            SetContentView(layout);
        }

        private void Play()
        {
            player.Prepare();
            player.Start();
        }

        private void Stop()
        {
            player.Stop();
        }
    }
}