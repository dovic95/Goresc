namespace Goresc;

using System.Globalization;
using PuppeteerSharp;

/// <summary>
/// Puppeteer implementation of <see cref="IBusinessInformationProvider"/> which uses XPath to find elements.
/// </summary>
public class PuppeteerBusinessInformationProvider : IBusinessInformationProvider
{
    private readonly IPage businessPage;

    public PuppeteerBusinessInformationProvider(IPage businessPage)
    {
        this.businessPage = businessPage;
    }

    /// <inheritdoc />
    public async Task<BusinessInformation> GetBusinessInformationAsync()
    {
        var businessName = await GetBusinessNameAsync();
        var businessCategory = await GetBusinessCategoryAsync();
        var globalRating = await GetGlobalRatingAsync();
        var totalReviews = await GetTotalReviewsAsync();

        return new(businessName, businessCategory, globalRating, totalReviews);
    }

    private async Task<string> GetBusinessCategoryAsync()
    {
        var category = string.Empty;
        var buttonWithCategory = await this.businessPage.FindAsync("//button[@jsaction='pane.rating.category']");

        if (buttonWithCategory?.Any() == true)
        {
            category = await buttonWithCategory[0].EvaluateFunctionAsync<string>("e => e.innerText");
        }

        return category;
    }

    private async Task<string> GetBusinessNameAsync()
    {
        var businessName = string.Empty;
        var divWithBusinessName = await this.businessPage.FindAsync("//div[@aria-label and @jslog and @role='main']");

        if (divWithBusinessName?.Any() == true)
        {
            businessName = await divWithBusinessName[0].EvaluateFunctionAsync<string>("e => e.getAttribute('aria-label')");
        }

        return businessName;
    }

    private async Task<double> GetGlobalRatingAsync()
    {
        var globalRating = 0.0;
        var spanWithGlobalRating = await this.businessPage.FindAsync("//div[@class = 'F7nice ']//span[@aria-hidden]");

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

    private async Task<int> GetTotalReviewsAsync()
    {
        var totalReviews = 0;
        var spanWithTotalReviews = await this.businessPage.FindAsync("//div[@class = 'F7nice ']//span[contains(@aria-label, ' reviews')]");

        if (spanWithTotalReviews?.Any() == true)
        {
            var totalReviewsText = await spanWithTotalReviews[0].EvaluateFunctionAsync<string>("e => e.innerText");
            totalReviewsText = totalReviewsText.Replace("(", string.Empty);
            totalReviewsText = totalReviewsText.Replace(")", string.Empty);
            totalReviewsText = totalReviewsText.Replace(".", string.Empty);
            totalReviewsText = totalReviewsText.Replace(",", string.Empty);

            if (int.TryParse(totalReviewsText, CultureInfo.InvariantCulture, out var totalReviewsValue))
            {
                totalReviews = totalReviewsValue;
            }
        }

        return totalReviews;
    }
}