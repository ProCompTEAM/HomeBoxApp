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
using HomeBoxLauncher.Droid.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using static Android.Widget.AdapterView;

namespace HomeBoxLauncher.Droid
{
    [Activity(Label = "HomeBox Selector", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class PlayerListActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private M3UReader reader;

        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LoadAll();

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        private void LoadAll()
        {
            string playlistPath = PlaylistInfo.GetPlaylistPath();

            InitializeReader(playlistPath);
            LoadUI();
        }

        private void LoadUI()
        {
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            layout.SetBackgroundColor(Color.Black);

            List<string> channelLabels = reader.Channels.Select((channel, index) => $"{index + 1}. {channel.Label}").ToList();
            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, channelLabels);
            listView = new ListView(this);
            listView.Adapter = adapter;
            listView.SetBackgroundColor(Color.SlateBlue);
            listView.ItemClick += OnChannelSelected;

            layout.AddView(listView);
            SetContentView(layout);
        }

        private void OnChannelSelected(object sender, ItemClickEventArgs eventArgs)
        {
            SelectChannel(eventArgs.Position);

            if (AppSettings.Mode == Mode.TV)
            {
                StartActivity(typeof(TVPlayActivity));
            }

            if (AppSettings.Mode == Mode.Radio)
            {
                StartActivity(typeof(RadioPlayActivity));
            }
        }

        private void InitializeReader(string playlistPath)
        {
            reader = new M3UReader(playlistPath);
            reader.ReadAll();
        }

        private void SelectChannel(int index)
        {
            string label = reader.Channels[index].Label;
            string url = reader.Channels[index].Url;
            
            AppSettings.PublicLabel = label;
            AppSettings.StreamUrl = url;
        }
    }
}