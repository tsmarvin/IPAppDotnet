using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using IPApp.ViewModels;

namespace IPApp.Views;

public partial class MainView : UserControl {
    private IDisposable? _copyHandlerDisposable;
    private IDisposable? _openUrlHandlerDisposable;

    public MainView( ) {
        InitializeComponent( );
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged( object? sender, EventArgs e ) {
        _copyHandlerDisposable?.Dispose();
        _openUrlHandlerDisposable?.Dispose();

        if (DataContext is not MainViewModel vm) {
            return;
        }

        _copyHandlerDisposable = vm.CopyToClipboard.RegisterHandler(async interaction =>
        {
            Avalonia.Input.Platform.IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard is not null) {
                await clipboard.SetTextAsync( interaction.Input );
            }
            interaction.SetOutput( Unit.Default );
        } );

        _openUrlHandlerDisposable = vm.OpenUrl.RegisterHandler(async interaction =>
        {
            ILauncher? launcher = TopLevel.GetTopLevel(this)?.Launcher;
            if (launcher is not null && Uri.TryCreate(interaction.Input, UriKind.Absolute, out Uri? uri))
            {
                _ = await launcher.LaunchUriAsync(uri);
            }
            interaction.SetOutput( Unit.Default );
        } );
    }
}
