namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public interface IBusinessNameScrapper
{
    Task<string> ScrapAsync(IPage page);
}