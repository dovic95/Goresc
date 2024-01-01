namespace Goresc;

using Scrappers.BusinessInformation;

public class ScrappersOptions
{
    public ITotalReviewsScrapper TotalReviewsScrapper { get; private set; } = new DefaultTotalReviewsScrapper();
    public IBusinessNameScrapper BusinessNameScrapper { get; private set; } = new DefaultBusinessNameScrapper();
    public IBusinessCategoryScrapper BusinessCategoryScrapper { get; private set; } = new DefaultBusinessCategoryScrapper();
    public IGlobalRatingScrapper GlobalRatingScrapper { get; private set; } = new DefaultGlobalRatingScrapper();
    
    /// <summary>
    /// Defines a new total reviews scrapper to be used instead of the default one.
    /// </summary>
    public ScrappersOptions UseTotalReviewsScrapper<T>(T scrapper) where T : ITotalReviewsScrapper
    {
        this.TotalReviewsScrapper = scrapper;

        return this;
    }
    
    /// <summary>
    /// Defines a new business name scrapper to be used instead of the default one.
    /// </summary>
    public ScrappersOptions UseBusinessNameScrapper<T>(T scrapper) where T : IBusinessNameScrapper
    {
        this.BusinessNameScrapper = scrapper;

        return this;
    }
    
    /// <summary>
    /// Defines a new business category scrapper to be used instead of the default one.
    /// </summary>
    public ScrappersOptions UseBusinessCategoryScrapper<T>(T scrapper) where T : IBusinessCategoryScrapper
    {
        this.BusinessCategoryScrapper = scrapper;

        return this;
    }
    
    /// <summary>
    /// Defines a new global rating scrapper to be used instead of the default one.
    /// </summary>
    public ScrappersOptions UseGlobalRatingScrapper<T>(T scrapper) where T : IGlobalRatingScrapper
    {
        this.GlobalRatingScrapper = scrapper;

        return this;
    }
}