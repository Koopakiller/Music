﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ImportViewModel : HeaderViewModelBase
    {
        public ImportViewModel() : base("Import")
        {
            this.DragOverCommand = new RelayCommand<DragEventArgs>(this.OnDragOver);
            this.DropCommand = new RelayCommand<DragEventArgs>(this.OnDrop);
        }

        private readonly string[] _supportedFileExtensions = new[] { ".mp3", ".wav", ".m4a", ".flac" };

        private void OnDragOver(DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Contains(DataFormats.FileDrop))
            {
                var fis = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new FileInfo(x))
                    .Where(x => this._supportedFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

                if (fis.Any())
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDrop(DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Contains(DataFormats.FileDrop))
            {
                var fis = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new FileInfo(x))
                    .Where(x => this._supportedFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

                foreach (var fi in fis)
                {
                    this.ProcessFile(fi);
                }
            }
        }

        public RelayCommand<DragEventArgs> DragOverCommand { get; }
        public RelayCommand<DragEventArgs> DropCommand { get; }

        private void ProcessFile(FileInfo fi)
        {
            var tag = TagLib.File.Create(fi.FullName).Tag;
            var pb = new PathBuilder(Properties.Settings.Default.MusicPath, Properties.Settings.Default.ImportPathPattern)
            {
                Replacements =
                {
                    ["Extension"]=fi.Extension,
                    ["Title"]=tag.Title,
                    ["JoinedAlbumArtists"]=tag.JoinedAlbumArtists,
                    ["JoinedPerformers"]=tag.JoinedPerformers,
                    ["Album"]=tag.Album,
                    ["JoinedGenres"]=tag.JoinedGenres,
                }
            };

            var p = pb.Build();

            File.Copy(fi.FullName, p);
        }
    }
}
