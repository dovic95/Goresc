namespace Goresc;

public class GorescSession : IAsyncDisposable
{
    private readonly IGorescBrowser gorescBrowser;

    public GorescSession(IGorescBrowser gorescBrowser)
    {
        this.gorescBrowser = gorescBrowser;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await this.gorescBrowser.DisconnectAsync();
    }

    /// <summary>
    /// Returns the browser instance.
    /// </summary>
    public IBusinessDataProvider Browser => this.gorescBrowser;
}