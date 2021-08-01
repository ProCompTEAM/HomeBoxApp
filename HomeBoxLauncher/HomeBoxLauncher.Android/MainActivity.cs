using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using HomeBoxLauncher.Droid.Enums;
using System;
using Xamarin.Essentials;

namespace HomeBoxLauncher.Droid
{
    [Activity(Label = "HomeBox Launcher", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryHome, Intent.CategoryDefault })]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetDefaultSettings();
            LoadUI();

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void LoadUI()
        {
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Horizontal;
            layout.SetBackgroundColor(Color.Black);

            Button buttonTV = new Button(this);
            buttonTV.Click += OnTVButtonClicked;
            buttonTV.Text = "📺 Телевидение";
            SetDefaultButtonStyle(buttonTV);

            Button buttonRadio = new Button(this);
            buttonRadio.Click += OnRadioButtonClicked;
            buttonRadio.Text = "🎶 Радио";
            SetDefaultButtonStyle(buttonRadio);

            layout.AddView(buttonTV);
            layout.AddView(buttonRadio);

            SetContentView(layout);
        }

        private void OnTVButtonClicked(object sender, EventArgs eventArgs)
        {
            AppSettings.Mode = Mode.TV;
            StartActivity(typeof(PlayerSwitchActivity));
        }

        private void OnRadioButtonClicked(object sender, EventArgs eventArgs)
        {
            AppSettings.Mode = Mode.Radio;
            StartActivity(typeof(PlayerSwitchActivity));
        }

        private void SetDefaultSettings()
        {
            RequestedOrientation = ScreenOrientation.Landscape;
        }

        private void SetDefaultButtonStyle(Button button)
        {
            DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            button.Top = 0;
            button.Left = 0;

            button.SetWidth((int)mainDisplayInfo.Width / 2);
            button.SetHeight((int)mainDisplayInfo.Height);

            button.SetBackgroundColor(Color.Black);
            button.SetTextColor(Color.White);

            button.SetTextSize(ComplexUnitType.Px, 76);
        }
    }
}