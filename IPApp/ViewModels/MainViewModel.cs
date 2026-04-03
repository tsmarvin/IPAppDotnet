using System;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IPApp.Models;
using ReactiveUI;

namespace IPApp.ViewModels;

public class MainViewModel : ViewModelBase {
    public string IpAddress {
        get;
        set => this.RaiseAndSetIfChanged( ref field, value );
    } = "";

    public string IpProtocol {
        get;
        set => this.RaiseAndSetIfChanged( ref field, value );
    } = "";

    public bool IsLoading {
        get;
        set => this.RaiseAndSetIfChanged( ref field, value );
    } = true;

    public bool IsCopied {
        get;
        set => this.RaiseAndSetIfChanged( ref field, value );
    }

    public bool HasError {
        get;
        set => this.RaiseAndSetIfChanged( ref field, value );
    }

    private readonly ObservableAsPropertyHelper<string> _copyButtonText;
    public string CopyButtonText => _copyButtonText.Value;

    public ReactiveCommand<Unit, Unit> CopyCommand { get; }
    public ReactiveCommand<string, Unit> OpenUrlCommand { get; }

    public Interaction<string, Unit> CopyToClipboard { get; } = new( );
    public Interaction<string, Unit> OpenUrl { get; } = new( );

    public MainViewModel( ) {
        _copyButtonText = this.WhenAnyValue( x => x.IsCopied )
            .Select( copied => copied ? "Copied!" : "Copy to Clipboard" )
            .ToProperty( this, x => x.CopyButtonText );

        IObservable<bool> canCopy = this.WhenAnyValue(
            x => x.IsLoading,
            x => x.HasError,
            (loading, error) => !loading && !error);

        CopyCommand = ReactiveCommand.CreateFromTask( DoCopyAsync, canCopy );
        OpenUrlCommand = ReactiveCommand.CreateFromTask<string>( DoOpenUrlAsync );

        _ = LoadIpAddressAsync( );
    }

    private async Task LoadIpAddressAsync( ) {
        try {
            using HttpClient client = new();
            string baseUrl = App.BaseUrl;

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? baseUri) ||
                !Uri.TryCreate(baseUri, "/api?format=json", out Uri? requestUri))
            {
                IpAddress = "Unable to detect IP";
                HasError = true;
                return;
            }

            string response = await client.GetStringAsync(requestUri);
            IpResponse? result = JsonSerializer.Deserialize<IpResponse>(response);
            if (result is not null && !string.IsNullOrEmpty( result.Ip )) {
                IpAddress = result.Ip;
                IpProtocol = result.Protocol;
            } else {
                IpAddress = "Unable to detect IP";
                HasError = true;
            }
        } catch {
            IpAddress = "Unable to detect IP";
            HasError = true;
        } finally {
            IsLoading = false;
        }
    }

    private async Task DoCopyAsync( ) {
        _ = await CopyToClipboard.Handle( IpAddress );
        IsCopied = true;
        await Task.Delay( 2000 );
        IsCopied = false;
    }

    private async Task DoOpenUrlAsync( string url ) {
        _ = await OpenUrl.Handle( url );
    }
}
