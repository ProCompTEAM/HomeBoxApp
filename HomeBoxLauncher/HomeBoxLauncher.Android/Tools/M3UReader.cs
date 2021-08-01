using HomeBoxLauncher.Droid.Enums;
using System;
using System.Collections.Generic;
using System.IO;

using HomeBoxLauncher.Droid.Models;

namespace HomeBoxLauncher.Droid.Tools
{
    public class M3UReader
    {
        public List<Channel> Channels = new List<Channel>();

        public string Path { get; }

        private string lastScannedLabel;

        private string lastScannedUrl;

        public M3UReader(string playlistFullPath)
        {
            Path = playlistFullPath;
        }

        public void ReadAll()
        {
            string[] content = File.ReadAllLines(Path);
            
            foreach (string rawData in content)
            {
                M3UView view = DetectView(rawData);

                switch (view)
                {
                    case M3UView.Label:
                        lastScannedLabel = TakeLabel(rawData);
                    break;

                    case M3UView.Url:
                        lastScannedUrl = rawData;
                        RegisterChannel();
                    break;
                }
            }
        }

        private void RegisterChannel()
        {
            Channel channel = new Channel
            {
                Label = lastScannedLabel,
                Url = lastScannedUrl
            };

            Channels.Add(channel);
        }

        private string TakeLabel(string rawData)
        {
            return rawData.Substring(8).Split(',')[1];
        }

        private M3UView DetectView(string rawData)
        {
            if (rawData.StartsWith("#EXTM3U"))
            {
                return M3UView.Header;
            }

            if (rawData.StartsWith("#EXTINF"))
            {
                return M3UView.Label;
            }

            if (Uri.IsWellFormedUriString(rawData, UriKind.RelativeOrAbsolute))
            {
                return M3UView.Url;
            }

            return M3UView.Unknown;
        }
    }
}