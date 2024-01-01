namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public class DefaultBusinessNameScrapper : IBusinessNameScrapper
{
    private const string BusinessNameDivXPath = "//div[@aria-label and @jslog and @role='main']";

    /// <inheritdoc />
    public async Task<string> ScrapAsync(IPage page)
    {
        var businessName = string.Empty;
        var divWithBusinessName = await page.FindAsync(BusinessNameDivXPath);

        if (divWithBusinessName.Any() == true)
        {
            businessName = await divWithBusinessName[0].EvaluateFunctionAsync<string>("e => e.getAttribute('aria-label')");
        }

        return businessName;
    }
}