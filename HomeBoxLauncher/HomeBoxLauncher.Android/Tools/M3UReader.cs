using HomeBoxLauncher.Droid.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace HomeBoxLauncher.Droid.Tools
{
    public class M3UReader
    {
        public List<string> Labels = new List<string>();

        public List<string> Channels = new List<string>();

        private string Path;

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
                        string label = TakeLabel(rawData);
                        Labels.Add(label);
                    break;

                    case M3UView.Url:
                        Channels.Add(rawData);
                    break;
                }
            }
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