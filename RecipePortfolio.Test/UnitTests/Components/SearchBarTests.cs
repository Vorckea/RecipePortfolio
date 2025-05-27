using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RecipePortfolio.Shared.Components;

namespace RecipePortfolio.Test.UnitTests.Components
{
    public class SearchBarTests
    {
        private const int DebounceDelay = 400;

        private IRenderedComponent<SearchBar> RenderSearchBar(
            TestContext ctx, 
            string placeholder = "Search for recipes...", 
            string searchTerm = "", 
            EventCallback<string>? searchTermChanged = null,
            Func<string, Task<List<string>>>? suggestionProvider = null,
            int minimumSearchLength = 2)
        {
            return ctx.RenderComponent<SearchBar>(parameters => parameters
                .Add(p => p.Placeholder, placeholder)
                .Add(p => p.SearchTerm, searchTerm)
                .Add(p => p.SearchTermChanged, searchTermChanged ?? EventCallback.Factory.Create<string>(this, _ => { }))
                .Add(p => p.SuggestionProvider, suggestionProvider)
                .Add(p => p.MinimumSearchLength, minimumSearchLength)
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
            var inputElement = cut.Find("input");
            Assert.Equal("Search for recipes...", inputElement.GetAttribute("placeholder"));
            Assert.Equal("", inputElement.GetAttribute("value"));
            Assert.Equal("form-control search-bar", inputElement.GetAttribute("class"));
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

        [Fact]
        public async Task SearchBar_ShouldShowSuggestions_WhenInputMeetsMinimumLength()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Assert
            var dropdownMenu = cut.Find(".dropdown-menu");
            var suggestionItems = cut.FindAll(".dropdown-item");

            Assert.Equal(3, suggestionItems.Count);
            Assert.Contains("Apple", suggestionItems[0].TextContent);
            Assert.Contains("Apricot", suggestionItems[1].TextContent);
            Assert.Contains("Avocado", suggestionItems[2].TextContent);
        }

        [Fact]
        public async Task SearchBar_ShouldNotShowSuggestions_WhenInputBelowMinimumLength()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 2
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Assert
            var dropdownMenu = cut.FindAll(".dropdown-menu");
            Assert.Empty(dropdownMenu);
        }

        [Fact]
        public async Task SearchBar_ShouldFilterSuggestions_BasedOnInput()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Banana", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("ap");
            await Task.Delay(50);

            // Assert
            var suggestionItems = cut.FindAll(".dropdown-item");

            Assert.Equal(2, suggestionItems.Count);
            Assert.Contains("Apple", suggestionItems[0].TextContent);
            Assert.Contains("Apricot", suggestionItems[1].TextContent);
        }

        [Fact]
        public async Task SearchBar_ShouldSelectSuggestion_OnClick()
        {
            // Arrange
            using var ctx = new TestContext();
            var searchTerm = "";
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                searchTermChanged: EventCallback.Factory.Create<string>(this, term => searchTerm = term),
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Click the first suggestion
            cut.FindAll(".dropdown-item")[0].Click();
            await Task.Delay(DebounceDelay);

            // Assert
            Assert.Equal("Apple", searchTerm);

            // Dropdown should be closed after selection
            var dropdownMenu = cut.FindAll(".dropdown-menu");
            Assert.Empty(dropdownMenu);
        }

        [Fact]
        public async Task SearchBar_ShouldNavigateSuggestions_WithKeyboard()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Press down arrow twice
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
            await Task.Delay(10);
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
            await Task.Delay(10);

            // Assert - second item should be selected (has active class)
            var suggestionItems = cut.FindAll(".dropdown-item");
            Assert.DoesNotContain("active", suggestionItems[0].GetAttribute("class") ?? "");
            Assert.Contains("active", suggestionItems[1].GetAttribute("class") ?? "");
            Assert.DoesNotContain("active", suggestionItems[2].GetAttribute("class") ?? "");
        }

        [Fact]
        public async Task SearchBar_ShouldSelectSuggestion_WithKeyboardEnter()
        {
            // Arrange
            using var ctx = new TestContext();
            var searchTerm = "";
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                searchTermChanged: EventCallback.Factory.Create<string>(this, term => searchTerm = term),
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Press down arrow once and then Enter
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
            await Task.Delay(10);
            input.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            await Task.Delay(DebounceDelay);

            // Assert
            Assert.Equal("Apple", searchTerm);

            // Dropdown should be closed after selection
            var dropdownMenu = cut.FindAll(".dropdown-menu");
            Assert.Empty(dropdownMenu);
        }

        [Fact]
        public async Task SearchBar_ShouldCloseSuggestions_WithEscapeKey()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");
            await Task.Delay(50);

            // Verify dropdown is open
            var dropdownMenuBefore = cut.FindAll(".dropdown-menu");
            Assert.NotEmpty(dropdownMenuBefore);

            // Press Escape key
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "Escape" });
            await Task.Delay(10);

            // Assert - dropdown should be closed
            var dropdownMenuAfter = cut.FindAll(".dropdown-menu");
            Assert.Empty(dropdownMenuAfter);
        }
    }
}
