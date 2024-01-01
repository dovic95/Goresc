namespace Goresc.Scrappers.BusinessInformation;

using PuppeteerSharp;

public class DefaultBusinessCategoryScrapper : IBusinessCategoryScrapper
{
    private const string BusinessCategoryButtonXPath = "//button[@jsaction='pane.rating.category']";

    /// <inheritdoc />
    public async Task<string> ScrapAsync(IPage page)
    {
        var category = string.Empty;
        var buttonWithCategory = await page.FindAsync(BusinessCategoryButtonXPath);

        if (buttonWithCategory.Any() == true)
        {
            category = await buttonWithCategory[0].EvaluateFunctionAsync<string>("e => e.innerText");
        }

        return category;return null;
    }
}