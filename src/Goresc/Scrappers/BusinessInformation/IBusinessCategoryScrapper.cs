namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public interface IBusinessCategoryScrapper
{
    Task<string> ScrapAsync(IPage page);
}