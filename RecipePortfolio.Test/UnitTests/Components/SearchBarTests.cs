using Bunit;
using Microsoft.AspNetCore.Components;
using RecipePortfolio.Shared.Components;

namespace RecipePortfolio.Test.UnitTests.Components
{
    public class SearchBarTests
    {
        private const int DebounceDelay = 400;

        private IRenderedComponent<SearchBar> RenderSearchBar(TestContext ctx, string placeholder = "Search for recipes...", string searchTerm = "", EventCallback<string>? searchTermChanged = null)
        {
            return ctx.RenderComponent<SearchBar>(parameters => parameters
                .Add(p => p.Placeholder, placeholder)
                .Add(p => p.SearchTerm, searchTerm)
                .Add(p => p.SearchTermChanged, searchTermChanged ?? EventCallback.Factory.Create<string>(this, _ => { }))
            );
        }

        [Fact]
        public void SearchBar_ShouldRenderCorrectly()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = RenderSearchBar(ctx);

            // Assert
            cut.MarkupMatches(@"<input type=""text"" class=""form-control search-bar"" placeholder=""Search for recipes..."" value=""""/>");
        }

        [Fact]
        public async Task SearchBar_ShouldUpdateSearchTerm_OnInput()
        {
            // Arrange
            using var ctx = new TestContext();
            var searchTerm = "initial";
            var cut = RenderSearchBar(ctx,
                searchTerm: searchTerm,
                searchTermChanged: EventCallback.Factory.Create<string>(this, term => searchTerm = term)
            );

            // Act
            cut.Find("input").Input("new search term");

            // Simulate debounce delay
            await Task.Delay(DebounceDelay);

            // Assert
            Assert.Equal("new search term", searchTerm);
        }

        [Fact]
        public async Task SearchBar_ShouldInvokeSearchTermChanged_OnInput()
        {
            // Arrange
            using var ctx = new TestContext();
            var searchTermChangedInvoked = false;
            var cut = RenderSearchBar(ctx,
                searchTermChanged: EventCallback.Factory.Create<string>(this, _ => searchTermChangedInvoked = true)
            );

            // Act
            cut.Find("input").Input("test");

            // Simulate debounce delay
            await Task.Delay(DebounceDelay);

            // Assert
            Assert.True(searchTermChangedInvoked);
        }
    }
}
