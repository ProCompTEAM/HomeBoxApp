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
using Java.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Orientation = Android.Widget.Orientation;

namespace HomeBoxLauncher.Droid
{
    [Activity(Label = "HomeBox TV", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class TVPlayActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private VideoView video;

        private bool scalled = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;

            LoadUI();
            Play();

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        public override void OnBackPressed()
        {
            video.StopPlayback();
            Finish();
        }

        private void LoadUI()
        {
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            layout.SetBackgroundColor(Color.Black);

            video = new VideoView(this);
            video.Touch += OnVideoActivated;
            video.SetVideoPath(AppSettings.StreamUrl);

            layout.AddView(video);
            SetContentView(layout);
        }

        private void Play()
        {
            Toast.MakeText(this, AppSettings.PublicLabel, ToastLength.Long).Show();

            video.Start();
        }

        private void OnVideoActivated(object sender, EventArgs eventArgs)
        {
            if (!scalled)
            {
                video.ScaleX = 2;
                video.ScaleY = 2;

                Toast.MakeText(this, "Видео x2", ToastLength.Short).Show();
            }
            else
            {
                video.ScaleX = 1;
                video.ScaleY = 1;

                Toast.MakeText(this, "Видео x1", ToastLength.Short).Show();
            }

            scalled = !scalled;
        }
    }
}