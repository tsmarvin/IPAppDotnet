using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using IPApp;
using ReactiveUI.Avalonia;

internal sealed partial class Program {
    private static async Task Main( string[] args ) {
        if (args.Length > 0 && Uri.TryCreate( args[0], UriKind.Absolute, out Uri? uri )) {
            App.BaseUrl = uri.GetLeftPart( UriPartial.Authority );
        }

        await BuildAvaloniaApp( )
            .WithInterFont( )
            .UseReactiveUI( _ => { } )
            .StartBrowserAppAsync( "out" );
    }

    public static AppBuilder BuildAvaloniaApp( ) {
        return AppBuilder.Configure<App>( );
    }
}