namespace Goresc.Scrappers.BusinessInformation;

using System.Globalization;
using System.Text.RegularExpressions;
using PuppeteerSharp;

public class DefaultTotalReviewsScrapper : ITotalReviewsScrapper
{
    private const string TotalReviewsDivXPath = "//div[@class = 'F7nice ']//span[starts-with(text(), '(')]"; // The total reviews, inside a span tag, are in the format: (1 234)

    /// <inheritdoc />
    public async Task<int> ScrapAsync(IPage page)
    {
        var totalReviews = 0;
        var spanWithTotalReviews = await page.FindAsync(TotalReviewsDivXPath);

        if (spanWithTotalReviews?.Any() == true)
        {
            var totalReviewsText = await spanWithTotalReviews[0].EvaluateFunctionAsync<string>("e => e.innerText");
            
            totalReviewsText = Regex.Replace(totalReviewsText, @"\D", "");

            if (int.TryParse(totalReviewsText, CultureInfo.InvariantCulture, out var totalReviewsValue))
            {
                totalReviews = totalReviewsValue;
            }
        }

        return totalReviews;
    }
}