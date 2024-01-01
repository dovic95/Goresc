namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public interface IGlobalRatingScrapper
{
    Task<double> ScrapAsync(IPage page);
}