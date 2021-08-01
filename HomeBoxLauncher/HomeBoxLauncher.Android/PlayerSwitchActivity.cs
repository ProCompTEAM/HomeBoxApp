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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Mode = HomeBoxLauncher.Droid.Enums.Mode;
using Orientation = Android.Widget.Orientation;

namespace HomeBoxLauncher.Droid
{
    [Activity(Label = "HomeBox Selector", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class PlayerSwitchActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private M3UReader reader;

        private Button channelLabel;

        private int ChannelIndex = 0;

        private MediaPlayer backgroundPlayer = new MediaPlayer();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LoadAll();

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        public override void OnBackPressed()
        {
            StopBackgroundPlay();
            Finish();
        }

        protected override void OnPause()
        {
            StopBackgroundPlay();
            base.OnPause();
        }

        private void LoadAll()
        {
            string playlistPath = PlaylistInfo.GetPlaylistPath();
            bool m3uPresentState = CheckIsM3UPresent(playlistPath);

            if (!m3uPresentState)
            {
                Finish();
                return;
            }

            backgroundPlayer.SetAudioStreamType(Android.Media.Stream.Music);

            InitializeReader(playlistPath);
            LoadUI();
            SelectChannel(0);
        }

        private void LoadUI()
        {
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            layout.SetBackgroundColor(Color.Black);

            Button previousChannelButton = new Button(this);
            previousChannelButton.Click += OnPreviousChannelButtonClicked;
            previousChannelButton.Text = "↩ Предыдущий канал";
            SetDefaultButtonStyle(previousChannelButton);

            channelLabel = new Button(this);
            channelLabel.Click += OnPlayChannelButtonClicked;
            SetDefaultButtonLabelStyle(channelLabel);

            Button nextChannelButton = new Button(this);
            nextChannelButton.Click += OnNextChannelButtonClicked;
            nextChannelButton.Text = "↪ Следующий канал";
            SetDefaultButtonStyle(nextChannelButton);

            Button channelListButton = new Button(this);
            channelListButton.Click += OnChannelListButtonClicked;
            channelListButton.Text = "≡ Список каналов";
            SetDefaultButtonStyle(channelListButton);

            layout.AddView(previousChannelButton);
            layout.AddView(channelLabel);
            layout.AddView(nextChannelButton);
            layout.AddView(channelListButton);

            SetContentView(layout);
        }

        private void OnPreviousChannelButtonClicked(object sender, EventArgs eventArgs)
        {
            PreviousChannel();
        }

        private void OnNextChannelButtonClicked(object sender, EventArgs eventArgs)
        {
            NextChannel();
        }

        private void OnPlayChannelButtonClicked(object sender, EventArgs eventArgs)
        {
            StopBackgroundPlay();

            if (AppSettings.Mode == Mode.TV)
            {
                StartActivity(typeof(TVPlayActivity));
            }

            if (AppSettings.Mode == Mode.Radio)
            {
                StartActivity(typeof(RadioPlayActivity));
            }
        }

        private void OnChannelListButtonClicked(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(PlayerListActivity));
        }

        private void SetDefaultButtonStyle(Button button)
        {
            button.SetBackgroundColor(Color.Black);
            button.SetTextColor(Color.White);
            button.SetTextSize(ComplexUnitType.Px, 64);
            button.SetPadding(0, 20, 0, 20);
            button.TextAlignment = TextAlignment.Center;
        }

        private void SetDefaultButtonLabelStyle(Button button)
        {
            button.SetBackgroundColor(Color.Black);
            button.SetTextColor(Color.Yellow);
            button.SetTextSize(ComplexUnitType.Px, 76);
            button.SetPadding(0, 70, 0, 70);
            button.TextAlignment = TextAlignment.Center;
        }

        private void InitializeReader(string playlistPath)
        {
            reader = new M3UReader(playlistPath);
            reader.ReadAll();
        }

        private bool CheckIsM3UPresent(string playlistPath)
        {
            if (!File.Exists(playlistPath))
            {
                Toast.MakeText(this, $"Плейлист '{playlistPath}' не найден!", ToastLength.Long).Show();

                return false;
            }

            return true;
        }

        private void SelectChannel(int index)
        {
            string label = reader.Channels[index].Label;
            string url = reader.Channels[index].Url;

            ChannelIndex = index;
            
            AppSettings.PublicLabel = label;
            AppSettings.StreamUrl = url;

            channelLabel.Text = $"▶️ {(ChannelIndex + 1)}. {label}";

            StartBackgroundPlay();
        }

        private void PreviousChannel()
        {
            int index = ChannelIndex - 1;

            if (index < 0)
            {
                index = reader.Channels.Count - 1;
            }

            SelectChannel(index);
        }

        private void NextChannel()
        {
            int index = ChannelIndex + 1;

            if (index == reader.Channels.Count)
            {
                index = 0;
            }

            SelectChannel(index);
        }

        private void StartBackgroundPlay()
        {
            Task.Run(() =>
            {
                backgroundPlayer.Reset();
                backgroundPlayer.SetDataSource(AppSettings.StreamUrl);
                backgroundPlayer.Prepare();
                backgroundPlayer.Start();
            });
        }

        private void StopBackgroundPlay()
        {
            backgroundPlayer.Stop();
        }
    }
}