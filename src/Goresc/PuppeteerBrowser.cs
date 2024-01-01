namespace Goresc;

using PuppeteerSharp;

public class PuppeteerBrowser : IGorescBrowser
{
    private readonly ScrappingSettings scrappingSettings;
    private IPage page;
    private IBrowser browser;
    private readonly BrowserFetcher browserFetcher = new BrowserFetcher();
    private IBusinessInformationProvider businessInformationProvider;

    public PuppeteerBrowser(ScrappingSettings scrappingSettings)
    {
        ArgumentNullException.ThrowIfNull(scrappingSettings, nameof(scrappingSettings));
        
        this.scrappingSettings = scrappingSettings;
    }

    /// <inheritdoc />
    public async Task ConnectAsync()
    {
        this.page = await this.ConnectToBusinessPageAsync(this.scrappingSettings.BusinessUrl);
        
        await this.PassCookiesAcceptancePageAsync(page);
        
        await page.WaitForNavigationAsync();
        
        this.businessInformationProvider = new PuppeteerBusinessInformationProvider(page);
    }

    /// <inheritdoc />
    public async Task DisconnectAsync()
    {
        await this.page.CloseAsync();
        await this.browser.CloseAsync();
        this.browserFetcher.Dispose();
    }

    /// <inheritdoc />
    public Task<BusinessInformation> GetBusinessInformationAsync() => this.businessInformationProvider.GetBusinessInformationAsync();
    
    private async Task<IPage> ConnectToBusinessPageAsync(Uri businessUrl)
    {
        await browserFetcher.DownloadAsync();
        browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            DefaultViewport = new ViewPortOptions()
            {
                Height = 1200
            }
        });

        var newPage = await browser.NewPageAsync();
        await newPage.SetExtraHttpHeadersAsync(new Dictionary<string, string>() { { "Accept-Language", this.scrappingSettings.Language } });
            
        await newPage.GoToAsync(businessUrl.ToString(), WaitUntilNavigation.Load);
        
        return newPage;
    }
    
    private async Task PassCookiesAcceptancePageAsync(IPage? businessPage)
    {
        ArgumentNullException.ThrowIfNull(businessPage, nameof(businessPage));
        
        var rejectAllCookiesButton = await businessPage.FindAsync("//button");
        
        if (rejectAllCookiesButton?.Any() == true)
        {
            await rejectAllCookiesButton[0].ClickAsync();
        }

        await businessPage.WaitForNavigationAsync(new NavigationOptions());
    }
}