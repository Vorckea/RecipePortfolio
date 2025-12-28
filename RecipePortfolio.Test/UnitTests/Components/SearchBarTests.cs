using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RecipePortfolio.Shared.Components;

namespace RecipePortfolio.Test.UnitTests.Components
{
    public class SearchBarTests
    {
        private const int _debounceDelay = 400;

        private IRenderedComponent<SearchBar> RenderSearchBar(
            TestContext ctx,
            string placeholder = "Search for recipes...",
            string searchTerm = "",
            EventCallback<string>? searchTermChanged = null,
            Func<string, CancellationToken, Task<List<string>>>? suggestionProvider = null,
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

            // Wait for debounced invocation
            await Task.Delay(_debounceDelay);

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

            // Wait for debounced invocation
            await Task.Delay(_debounceDelay);

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
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");

            // Wait until suggestions appear (robust against debounce)
            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.Equal(3, suggestionItems.Count);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            var suggestionItems = cut.FindAll(".dropdown-item");
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
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 2
            );

            // Act
            cut.Find("input").Input("a");

            // The component immediately clears suggestions when below minimum length.
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
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("ap");

            // Wait until filtered suggestions appear
            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.Equal(2, suggestionItems.Count);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            var suggestionItems = cut.FindAll(".dropdown-item");

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
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act
            cut.Find("input").Input("a");

            // Wait for suggestions to appear
            cut.WaitForAssertion(() =>
            {
                var suggestionItem = cut.FindAll(".dropdown-item");
                Assert.NotEmpty(suggestionItem);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            // Click the first suggestion
            cut.FindAll(".dropdown-item")[0].Click();

            // Wait until the selection callback has been invoked and the dropdown closed
            cut.WaitForAssertion(() =>
            {
                Assert.Equal("Apple", searchTerm);
                Assert.Empty(cut.FindAll(".dropdown-menu"));
            }, timeout: TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task SearchBar_ShouldNavigateSuggestions_WithKeyboard()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act - type and wait for suggestions to appear (avoid fragile fixed delays)
            cut.Find("input").Input("a");

            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.Equal(3, suggestionItems.Count);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            // Press down arrow once - should select first item
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.Contains("active", suggestionItems[0].GetAttribute("class") ?? "");
            }, timeout: TimeSpan.FromMilliseconds(500));

            // Press down arrow a second time - should move to second item
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

            // Assert - second item should be selected (has active class)
            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.DoesNotContain("active", suggestionItems[0].GetAttribute("class") ?? "");
                Assert.Contains("active", suggestionItems[1].GetAttribute("class") ?? "");
                Assert.DoesNotContain("active", suggestionItems[2].GetAttribute("class") ?? "");
            }, timeout: TimeSpan.FromMilliseconds(500));
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
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act - type and wait for suggestions
            cut.Find("input").Input("a");

            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.NotEmpty(suggestionItems);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            // Press down arrow once and then Enter
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

            cut.WaitForAssertion(() =>
            {
                var suggestionItems = cut.FindAll(".dropdown-item");
                Assert.Contains("active", suggestionItems[0].GetAttribute("class") ?? "");
            }, timeout: TimeSpan.FromMilliseconds(500));

            input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

            // Wait until selection propagated
            cut.WaitForAssertion(() =>
            {
                Assert.Equal("Apple", searchTerm);
                Assert.Empty(cut.FindAll(".dropdown-menu"));
            }, timeout: TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task SearchBar_ShouldCloseSuggestions_WithEscapeKey()
        {
            // Arrange
            using var ctx = new TestContext();
            var testSuggestions = new List<string> { "Apple", "Apricot", "Avocado" };
            var cut = RenderSearchBar(ctx,
                suggestionProvider: (term, ct) => Task.FromResult(testSuggestions.Where(s =>
                    s.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList()),
                minimumSearchLength: 1
            );

            // Act - type and wait for suggestions to appear
            cut.Find("input").Input("a");

            cut.WaitForAssertion(() =>
            {
                var dropdownMenuBefore = cut.FindAll(".dropdown-menu");
                Assert.NotEmpty(dropdownMenuBefore);
            }, timeout: TimeSpan.FromMilliseconds(1000));

            // Press Escape key
            var input = cut.Find("input");
            input.KeyDown(new KeyboardEventArgs { Key = "Escape" });

            // Assert - dropdown should be closed
            cut.WaitForAssertion(() =>
            {
                var dropdownMenuAfter = cut.FindAll(".dropdown-menu");
                Assert.Empty(dropdownMenuAfter);
            }, timeout: TimeSpan.FromMilliseconds(500));
        }
    }
}
