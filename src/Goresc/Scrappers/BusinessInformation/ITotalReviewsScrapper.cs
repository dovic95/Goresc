namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public interface ITotalReviewsScrapper
{
    Task<int> ScrapAsync(IPage page);
}