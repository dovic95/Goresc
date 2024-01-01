namespace Goresc.Scrappers.BusinessInformation;

using System.Globalization;
using PuppeteerSharp;

public class DefaultGlobalRatingScrapper : IGlobalRatingScrapper
{
    private const string GlobalRatingSpanXPath = "//div[@class = 'F7nice ']//span[@aria-hidden]";

    /// <inheritdoc />
    public async Task<double> ScrapAsync(IPage page)
    {
        var globalRating = 0.0;
        var spanWithGlobalRating = await page.FindAsync(GlobalRatingSpanXPath);

        if (spanWithGlobalRating?.Any() == true)
        {
            var globalRatingValue = await spanWithGlobalRating[0].EvaluateFunctionAsync<string>("e => e.innerText");

            if (double.TryParse(globalRatingValue?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var globalRatingValueParsed))
            {
                globalRating = globalRatingValueParsed;
            }
        }

        return globalRating;
    }
}