namespace Goresc;

public class GorescEngineBuilder
{
    private string? businessUrl;
    private string language = "en-GB";

    private Action<NavigatorsOptions> navigatorOptionsAction = _ => { };
    private Action<ScrappersOptions> scrapperOptionsAction = _ => { };

    private GorescEngineBuilder()
    {
    }

    /// <summary>
    /// Entry point to create a new instance of <see cref="GorescEngineBuilder"/>.
    /// </summary>
    public static GorescEngineBuilder Create => new();

    /// <summary>
    /// Builds a new instance of <see cref="GorescEngine"/>.
    /// </summary>
    /// <returns>The Goresc engine</returns>
    public async Task<GorescEngine> BuildAsync()
    {
        ArgumentNullException.ThrowIfNull(this.businessUrl);

        await Task.Yield();

        var navigatorOptions = new NavigatorsOptions();
        this.navigatorOptionsAction(navigatorOptions);
        
        var scrapperOptions = new ScrappersOptions();
        this.scrapperOptionsAction(scrapperOptions);

        var scrappingSettings = new ScrappingSettings(new Uri(this.businessUrl), this.language)
        {
            // Navigators
            CookiesConsentNavigator = navigatorOptions.CookiesConsentNavigator,
            
            // Scrappers
            TotalReviewsScrapper = scrapperOptions.TotalReviewsScrapper,
            BusinessNameScrapper = scrapperOptions.BusinessNameScrapper,
            BusinessCategoryScrapper = scrapperOptions.BusinessCategoryScrapper,
            GlobalRatingScrapper = scrapperOptions.GlobalRatingScrapper
        };

        var engine = new GorescEngine(scrappingSettings);

        return engine;
    }

    /// <summary>
    /// Configures the navigators to be used for scrapping.
    /// </summary>
    public GorescEngineBuilder ConfigureNavigators(Action<NavigatorsOptions> options)
    {
        this.navigatorOptionsAction = options;

        return this;
    }

    /// <summary>
    /// Determines the language to use for scrapping. By default, it is set to "en-GB".
    /// </summary>
    public GorescEngineBuilder UseLanguage(string scrappingLanguage)
    {
        this.language = scrappingLanguage;

        return this;
    }

    /// <summary>
    /// Sets the Google Maps business URL to scrap. 
    /// </summary>
    public GorescEngineBuilder WithBusinessUrl(string url)
    {
        this.businessUrl = url;

        return this;
    }

    /// <summary>
    /// Configures the scrappers to be used for scrapping.
    /// </summary>
    public GorescEngineBuilder ConfigureScrappers(Action<ScrappersOptions> options)
    {
        this.scrapperOptionsAction = options;

        return this;
    }
}