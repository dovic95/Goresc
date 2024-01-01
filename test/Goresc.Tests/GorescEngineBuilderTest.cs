namespace Goresc.Tests;

public class GorescEngineBuilderTest
{
    [Fact]
    public async Task Business_information_are_retrieved_from_url()
    {
        // Arrange
        var engine = await GorescEngineBuilder.Create
            .WithBusinessUrl("https://www.google.com/maps/place/The+Ivy/@51.5133696,-0.1338576,16z/data=!4m16!1m9!3m8!1s0x47e666476eef1ec5:0xe2c157f47bc2f686!2sLidl!8m2!3d48.96528!4d2.26157!9m1!1b1!16s%2Fg%2F1tmpjty2!3m5!1s0x487604cd73cacb55:0xf70e3382d1ea5dc9!8m2!3d51.5128603!4d-0.128045!16zL20vMDhrbWp2?entry=ttu")
            .BuildAsync();

        // Act
        await using var session = await engine.StartAsync();

        var businessInformation = await session.Browser.GetBusinessInformationAsync();

        // Assert
        businessInformation.Name.Should().Be("The Ivy");
        businessInformation.Category.Should().Be("British restaurant");
        businessInformation.GlobalRating.Should().BeGreaterThan(0).And.BeLessOrEqualTo(5);
        businessInformation.TotalReviews.Should().BeGreaterThan(0);

        await session.DisposeAsync();
    }

    [Fact]
    public async Task Language_can_be_specified()
    {
        // Arrange
        var engine = await GorescEngineBuilder.Create
            .UseLanguage("fr")
            .WithBusinessUrl("https://www.google.com/maps/place/The+Ivy/@51.5133696,-0.1338576,16z/data=!4m16!1m9!3m8!1s0x47e666476eef1ec5:0xe2c157f47bc2f686!2sLidl!8m2!3d48.96528!4d2.26157!9m1!1b1!16s%2Fg%2F1tmpjty2!3m5!1s0x487604cd73cacb55:0xf70e3382d1ea5dc9!8m2!3d51.5128603!4d-0.128045!16zL20vMDhrbWp2?entry=ttu")
            .BuildAsync();

        // Act
        await using var session = await engine.StartAsync();

        var businessInformation = await session.Browser.GetBusinessInformationAsync();

        // Assert
        businessInformation.Name.Should().Be("The Ivy");
        businessInformation.Category.Should().Be("Restaurant britannique");
        
        await session.DisposeAsync();
    }
}