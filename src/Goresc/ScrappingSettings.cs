namespace Goresc;

using Navigators;
using Scrappers.BusinessInformation;

public record ScrappingSettings(Uri BusinessUrl, string Language = "en-GB")
{
    // Navigators
    public ICookiesConsentNavigator CookiesConsentNavigator { get; init; } = new DefaultCookiesConsentNavigator();
    
    // Scrappers
    public ITotalReviewsScrapper TotalReviewsScrapper { get; init; } = new DefaultTotalReviewsScrapper();
    public IBusinessNameScrapper BusinessNameScrapper { get; init; } = new DefaultBusinessNameScrapper();
    public IBusinessCategoryScrapper BusinessCategoryScrapper { get; init; } = new DefaultBusinessCategoryScrapper();
    public IGlobalRatingScrapper GlobalRatingScrapper { get; init; } = new DefaultGlobalRatingScrapper();
}