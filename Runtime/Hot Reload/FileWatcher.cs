using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RishUI.Deprecated
{
    internal delegate void ChangedFile(string path);
    internal delegate void CreatedFile(string path);
    internal delegate void DeletedFile(string path);
    internal delegate void Disposed(FileWatcher watcher);
    
    internal class FileWatcher
    {
        private const string Filter = "*.cs";
        
        public event ChangedFile Changed;
        public event CreatedFile Created;
        public event DeletedFile Deleted;
        private event Disposed Disposed;
        
        private string FolderPath { get; }
        private FileSystemWatcher NativeWatcher { get; set; }
        private FileSystemWatcher ChildrenWatcher { get; set; }

        private List<string> Files { get; } = new();
        private Dictionary<string, FileWatcher> Children { get; } = new();

        public FileWatcher(string folder)
        {
            FolderPath = folder;

            ChildrenWatcher = new FileSystemWatcher(folder);
            ChildrenWatcher.EnableRaisingEvents = true;
            ChildrenWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName;
            ChildrenWatcher.Created += ChildrenCreated;
            ChildrenWatcher.Deleted += ChildrenDeleted;
            ChildrenWatcher.Disposed += ChildrenWatcherDisposed;
            
            NativeWatcher = new FileSystemWatcher(folder, Filter);
            NativeWatcher.EnableRaisingEvents = true;
            NativeWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName
                                                                    | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                                    | NotifyFilters.Size;
            NativeWatcher.Changed += NativeChangedFile;
            NativeWatcher.Created += NativeCreatedFile;
            NativeWatcher.Deleted += NativeDeletedFile;
            NativeWatcher.Error += NativeError;
            NativeWatcher.Disposed += NativeWatcherDisposed;
        }

        public void Initialize()
        {
            var directories = Directory.GetDirectories(FolderPath);
            foreach (var path in directories)
            {
                AddChild(path);
            }
            
            var files = Directory.GetFiles(FolderPath, Filter);
            foreach (var path in files)
            {
                Files.Add(path);
                CreatedFile(path);
            }
        }

        public void Dispose()
        {
            foreach (var child in Children.Values)
            {
                child.Dispose();
            }
            Children.Clear();
            
            foreach (var path in Files)
            {
                DeletedFile(path);
            }
            Files.Clear();
            
            NativeWatcher.Dispose();
            ChildrenWatcher.Dispose();
        }

        private void NativeWatcherDisposed(object sender, EventArgs e)
        {
            NativeWatcher.Changed -= NativeChangedFile;
            NativeWatcher.Created -= NativeCreatedFile;
            NativeWatcher.Deleted -= NativeDeletedFile;
            NativeWatcher.Error -= NativeError;
            NativeWatcher.Disposed -= NativeWatcherDisposed;
            
            NativeWatcher = null;

            NotifyDispose();
        }
        
        private void ChildrenWatcherDisposed(object sender, EventArgs e)
        {
            ChildrenWatcher.Created -= ChildrenCreated;
            ChildrenWatcher.Deleted -= ChildrenDeleted;
            ChildrenWatcher.Disposed -= ChildrenWatcherDisposed;
            
            ChildrenWatcher = null;

            NotifyDispose();
        }

        private void NotifyDispose()
        {
            if (NativeWatcher != null || ChildrenWatcher != null)
            {
                return;
            }

            UnityThread.ExecuteInUpdate(RaiseEvent);
            
            void RaiseEvent() => Disposed?.Invoke(this);
        }

        private void NativeChangedFile(object sender, FileSystemEventArgs args)
        {
            var path = args.FullPath;

            UnityThread.ExecuteInUpdate(RaiseEvent);
            
            void RaiseEvent() => ChangedFile(path);
        }
        private void ChangedFile(string path) => Changed?.Invoke(path);

        private void NativeCreatedFile(object sender, FileSystemEventArgs args)
        {
            var path = args.FullPath;
            
            Files.Add(path);
            
            UnityThread.ExecuteInUpdate(RaiseEvent);
            
            void RaiseEvent() => CreatedFile(path);
        }
        private void CreatedFile(string path) => Created?.Invoke(path);

        private void NativeDeletedFile(object sender, FileSystemEventArgs args)
        {
            var path = args.FullPath;

            Files.Remove(path);
            
            UnityThread.ExecuteInUpdate(RaiseEvent);
            
            void RaiseEvent() => DeletedFile(path);
        }
        private void DeletedFile(string path) => Deleted?.Invoke(path);

        private static void NativeError(object sender, ErrorEventArgs args) 
        {
            var exception = args.GetException();
            
            UnityThread.ExecuteInUpdate(DebugError);

            void DebugError() => Debug.LogException(exception);
        }
        
        private void ChildrenCreated(object sender, FileSystemEventArgs args)
        {
            var path = args.FullPath;
            AddChild(path);
        }
        
        private void ChildrenDeleted(object sender, FileSystemEventArgs args)
        {
            var path = args.FullPath;
            RemoveChild(path);
        }

        private void ChildDisposed(FileWatcher child)
        {
            child.Changed -= ChangedFile;
            child.Created -= CreatedFile;
            child.Deleted -= DeletedFile;
            child.Disposed -= ChildDisposed;
        }

        private bool AddChild(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }
            
            var child = new FileWatcher(path);
            child.Changed += ChangedFile;
            child.Created += CreatedFile;
            child.Deleted += DeletedFile;
            child.Disposed += ChildDisposed;
            
            Children[path] = child;
            
            child.Initialize();

            return true;
        }
        
        private bool RemoveChild(string path)
        {
            if (!Children.TryGetValue(path, out var child))
            {
                return false;
            }
            
            child.Dispose();
            Children.Remove(path);

            return true;
        }
    }
}