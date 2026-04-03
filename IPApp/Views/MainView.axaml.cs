using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using IPApp.ViewModels;

namespace IPApp.Views;

public partial class MainView : UserControl {
    public MainView( ) {
        InitializeComponent( );
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged( object? sender, EventArgs e ) {
        if (DataContext is not MainViewModel vm) {
            return;
        }

        _ = vm.CopyToClipboard.RegisterHandler( async interaction => {
            Avalonia.Input.Platform.IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard is not null) {
                await clipboard.SetTextAsync( interaction.Input );
            }
            interaction.SetOutput( Unit.Default );
        } );

        _ = vm.OpenUrl.RegisterHandler( async interaction => {
            ILauncher? launcher = TopLevel.GetTopLevel(this)?.Launcher;
            if (launcher is not null) {
                _ = await launcher.LaunchUriAsync( new Uri( interaction.Input ) );
            }
            interaction.SetOutput( Unit.Default );
        } );
    }
}
