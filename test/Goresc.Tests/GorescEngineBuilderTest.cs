namespace Goresc.Tests;

using System.Globalization;
using Navigators;
using PuppeteerSharp;
using Scrappers.BusinessInformation;

public class GorescEngineBuilderTest
{
    [Fact]
    public async Task Business_information_are_retrieved_from_url()
    {
        // Arrange
        var engine = await GorescEngineBuilder.Create
            .WithBusinessUrl(
                "https://www.google.com/maps/place/The+Ivy/@51.5133696,-0.1338576,16z/data=!4m16!1m9!3m8!1s0x47e666476eef1ec5:0xe2c157f47bc2f686!2sLidl!8m2!3d48.96528!4d2.26157!9m1!1b1!16s%2Fg%2F1tmpjty2!3m5!1s0x487604cd73cacb55:0xf70e3382d1ea5dc9!8m2!3d51.5128603!4d-0.128045!16zL20vMDhrbWp2?entry=ttu")
            .BuildAsync();

        // Act
        await using var session = await engine.StartAsync();

        var businessInformation = await session.Browser.GetBusinessInformationAsync();

        // Assert
        businessInformation.Name.Should().Be("The Ivy");
        businessInformation.Category.Should().Be("British restaurant");
        businessInformation.GlobalRating.Should().BeGreaterThan(0).And.BeLessOrEqualTo(5);
        businessInformation.GlobalRating.ToString(CultureInfo.InvariantCulture).Should().HaveLength(3);
        businessInformation.TotalReviews.Should().BeGreaterThan(0);

        await session.DisposeAsync();
    }

    [Fact]
    public async Task Business_information_are_retrieved_from_url_using_custom_navigators_and_scrappers()
    {
        // Arrange
        var customCookiesConsentNavigator = new CustomCookiesConsentNavigator(new DefaultCookiesConsentNavigator());
        var testableScrapper = new TestableScrapper(new DefaultTotalReviewsScrapper(), new DefaultGlobalRatingScrapper(), new DefaultBusinessNameScrapper(), new DefaultBusinessCategoryScrapper());

        var engine = await GorescEngineBuilder.Create
            .WithBusinessUrl(
                "https://www.google.com/maps/place/The+Ivy/@51.5133696,-0.1338576,16z/data=!4m16!1m9!3m8!1s0x47e666476eef1ec5:0xe2c157f47bc2f686!2sLidl!8m2!3d48.96528!4d2.26157!9m1!1b1!16s%2Fg%2F1tmpjty2!3m5!1s0x487604cd73cacb55:0xf70e3382d1ea5dc9!8m2!3d51.5128603!4d-0.128045!16zL20vMDhrbWp2?entry=ttu")
            .ConfigureNavigators(options =>
            {
                options.UseCookiesConsentNavigator(customCookiesConsentNavigator);
            })
            .ConfigureScrappers(options =>
            {
                options.UseTotalReviewsScrapper(testableScrapper);
                options.UseGlobalRatingScrapper(testableScrapper);
                options.UseBusinessNameScrapper(testableScrapper);
                options.UseBusinessCategoryScrapper(testableScrapper);
            })
            .BuildAsync();

        // Act
        await using var session = await engine.StartAsync();

        var businessInformation = await session.Browser.GetBusinessInformationAsync();

        // Assert
        businessInformation.Name.Should().Be("The Ivy");
        businessInformation.Category.Should().Be("British restaurant");
        businessInformation.GlobalRating.Should().BeGreaterThan(0).And.BeLessOrEqualTo(5);
        businessInformation.GlobalRating.ToString(CultureInfo.InvariantCulture).Should().HaveLength(3);
        businessInformation.TotalReviews.Should().BeGreaterThan(0);
        
        customCookiesConsentNavigator.Called.Should().BeTrue();
        testableScrapper.TotalReviewsScraped.Should().BeTrue();
        testableScrapper.GlobalRatingScraped.Should().BeTrue();
        testableScrapper.BusinessNameScraped.Should().BeTrue();
        testableScrapper.BusinessCategoryScraped.Should().BeTrue();
    }

    [Fact]
    public async Task Language_can_be_specified()
    {
        // Arrange
        var engine = await GorescEngineBuilder.Create
            .UseLanguage("fr")
            .WithBusinessUrl(
                "https://www.google.com/maps/place/The+Ivy/@51.5133696,-0.1338576,16z/data=!4m16!1m9!3m8!1s0x47e666476eef1ec5:0xe2c157f47bc2f686!2sLidl!8m2!3d48.96528!4d2.26157!9m1!1b1!16s%2Fg%2F1tmpjty2!3m5!1s0x487604cd73cacb55:0xf70e3382d1ea5dc9!8m2!3d51.5128603!4d-0.128045!16zL20vMDhrbWp2?entry=ttu")
            .BuildAsync();

        // Act
        await using var session = await engine.StartAsync();

        var businessInformation = await session.Browser.GetBusinessInformationAsync();

        // Assert
        businessInformation.Name.Should().Be("The Ivy");
        businessInformation.Category.Should().Be("Restaurant britannique");
        businessInformation.GlobalRating.Should().BeGreaterThan(0).And.BeLessOrEqualTo(5);
        businessInformation.GlobalRating.ToString(CultureInfo.InvariantCulture).Should().HaveLength(3);
        businessInformation.TotalReviews.Should().BeGreaterThan(0);

        await session.DisposeAsync();
    }

    private class CustomCookiesConsentNavigator : ICookiesConsentNavigator
    {
        private readonly DefaultCookiesConsentNavigator defaultCookiesConsentNavigator;

        public CustomCookiesConsentNavigator(DefaultCookiesConsentNavigator defaultCookiesConsentNavigator)
        {
            this.defaultCookiesConsentNavigator = defaultCookiesConsentNavigator;
        }

        public bool Called { get; set; }

        /// <inheritdoc />
        public Task AcceptCookiesAsync(IPage page)
        {
            this.Called = true;
            return this.defaultCookiesConsentNavigator.AcceptCookiesAsync(page);
        }
    }
    
    private class TestableScrapper : ITotalReviewsScrapper, IGlobalRatingScrapper, IBusinessNameScrapper, IBusinessCategoryScrapper
    {
        private readonly DefaultTotalReviewsScrapper defaultTotalReviewsScrapper;
        private readonly DefaultGlobalRatingScrapper defaultGlobalRatingScrapper;
        private readonly DefaultBusinessNameScrapper defaultBusinessNameScrapper;
        private readonly DefaultBusinessCategoryScrapper defaultBusinessCategoryScrapper;

        public TestableScrapper(DefaultTotalReviewsScrapper defaultTotalReviewsScrapper, DefaultGlobalRatingScrapper defaultGlobalRatingScrapper, DefaultBusinessNameScrapper defaultBusinessNameScrapper, DefaultBusinessCategoryScrapper defaultBusinessCategoryScrapper)
        {
            this.defaultTotalReviewsScrapper = defaultTotalReviewsScrapper;
            this.defaultGlobalRatingScrapper = defaultGlobalRatingScrapper;
            this.defaultBusinessNameScrapper = defaultBusinessNameScrapper;
            this.defaultBusinessCategoryScrapper = defaultBusinessCategoryScrapper;
        }

        public bool TotalReviewsScraped { get; private set; }
        public bool GlobalRatingScraped { get; private set; }
        public bool BusinessNameScraped { get; private set; }
        public bool BusinessCategoryScraped { get; private set; }

        /// <inheritdoc />
        Task<int> ITotalReviewsScrapper.ScrapAsync(IPage page)
        {
            this.TotalReviewsScraped = true;

            return this.defaultTotalReviewsScrapper.ScrapAsync(page);
        }

        /// <inheritdoc />
        Task<double> IGlobalRatingScrapper.ScrapAsync(IPage page)
        {
            this.GlobalRatingScraped = true;
            return this.defaultGlobalRatingScrapper.ScrapAsync(page);
        }

        /// <inheritdoc />
        Task<string> IBusinessNameScrapper.ScrapAsync(IPage page)
        {
            this.BusinessNameScraped = true;
            return this.defaultBusinessNameScrapper.ScrapAsync(page);
        }

        /// <inheritdoc />
        Task<string> IBusinessCategoryScrapper.ScrapAsync(IPage page)
        {
            this.BusinessCategoryScraped = true;
            return this.defaultBusinessCategoryScrapper.ScrapAsync(page);
        }
    }
}