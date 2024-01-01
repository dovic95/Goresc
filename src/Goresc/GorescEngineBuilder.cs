namespace Goresc;

public class GorescEngineBuilder
{
    private string? businessUrl;
    private string language = "en-GB";
    
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
        
        var scrappingSettings = new ScrappingSettings(new Uri(this.businessUrl), this.language);
        var engine = new GorescEngine(scrappingSettings);
        
        return engine;
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
    /// Determines the language to use for scrapping. By default, it is set to "en-GB".
    /// </summary>
    public GorescEngineBuilder UseLanguage(string scrappingLanguage)
    {
        this.language = scrappingLanguage;
        
        return this;
    }
}