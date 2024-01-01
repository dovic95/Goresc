namespace Goresc;

using PuppeteerSharp;

/// <summary>
/// Puppeteer implementation of <see cref="IBusinessInformationProvider"/> which uses XPath to find elements.
/// </summary>
public class PuppeteerBusinessInformationProvider : IBusinessInformationProvider
{
    private readonly IPage businessPage;
    private readonly ScrappingSettings scrappingSettings;
    

    public PuppeteerBusinessInformationProvider(IPage businessPage, ScrappingSettings scrappingSettings)
    {
        this.businessPage = businessPage;
        this.scrappingSettings = scrappingSettings;
    }

    /// <inheritdoc />
    public async Task<BusinessInformation> GetBusinessInformationAsync()
    {
        var businessName = await this.scrappingSettings.BusinessNameScrapper.ScrapAsync(this.businessPage);
        var businessCategory = await this.scrappingSettings.BusinessCategoryScrapper.ScrapAsync(this.businessPage);
        var globalRating = await this.scrappingSettings.GlobalRatingScrapper.ScrapAsync(this.businessPage);
        var totalReviews = await this.scrappingSettings.TotalReviewsScrapper.ScrapAsync(this.businessPage);

        return new BusinessInformation(businessName, businessCategory, globalRating, totalReviews);
    }
}