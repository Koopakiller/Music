﻿using GalaSoft.MvvmLight.CommandWpf;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class SettingsViewModel : HeaderViewModelBase
    {
        private string _musicPath;
        private string _playlistsPath;

        public SettingsViewModel() : base("Settings")
        {
            this.SaveCommand = new RelayCommand(this.OnSave, () => this.IsSaved);
            this.ResetCommand = new RelayCommand(this.OnReset, () => this.IsSaved);

            this.OnReset();
        }

        private void RaiseIsSavedChanged()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged(nameof(this.IsSaved));
            this.SaveCommand.RaiseCanExecuteChanged();
            this.ResetCommand.RaiseCanExecuteChanged();
        }

        public bool IsSaved => this.MusicPath == Properties.Settings.Default.MusicPath
                            && this.PlaylistsPath == Properties.Settings.Default.PlaylistsPath;

        private void OnReset()
        {
            this.MusicPath = Properties.Settings.Default.MusicPath;
            this.PlaylistsPath = Properties.Settings.Default.PlaylistsPath;
        }

        private void OnSave()
        {
            Properties.Settings.Default.MusicPath = this.MusicPath;
            Properties.Settings.Default.PlaylistsPath = this.PlaylistsPath;
            Properties.Settings.Default.Save();
        }

        public string MusicPath
        {
            get { return this._musicPath; }
            set
            {
                this._musicPath = value;
                this.RaisePropertyChanged();
                this.RaiseIsSavedChanged();
            }
        }

        public string PlaylistsPath
        {
            get { return this._playlistsPath; }
            set
            {
                this._playlistsPath = value;
                this.RaisePropertyChanged();
                this.RaiseIsSavedChanged();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand ResetCommand { get; }
    }
}