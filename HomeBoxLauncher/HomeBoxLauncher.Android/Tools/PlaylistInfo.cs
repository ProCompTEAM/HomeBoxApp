using HomeBoxLauncher.Droid.Enums;

namespace HomeBoxLauncher.Droid.Tools
{
    public static class PlaylistInfo
    {
        public static string GetPlaylistPath()
        {
            return System.IO.Path.Combine(
                Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                GetPlaylistFilename());
        }

        public static string GetPlaylistFilename()
        {
            switch (AppSettings.Mode)
            {
                case Mode.TV:
                    return AppConstants.PlaylistTVFileName;
                case Mode.Radio:
                    return AppConstants.PlaylistRadioFileName;
            }

            return string.Empty;
        }
    }
}