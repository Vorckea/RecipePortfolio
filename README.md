# RecipePortfolio

RecipePortfolio is a web application built with Blazor WebAssembly that allows users to browse, filter, and view detailed recipes. The application is designed to be responsive and user-friendly.

## Features

- **Recipe Filtering**: Filter recipes based on search terms and selected tags.
- **Recipe Details**: View detailed information about each recipe, including ingredients, instructions, preparation time, and tags.
- **Responsive Design**: The application is designed to work on various devices, including desktops, tablets, and mobile phones.

## Technologies Used

- **Blazor WebAssembly**: For building the client-side web application.
- **.NET 9**: The target framework for the application.
- **C# 13.0**: The programming language used for the application logic.
- **Bootstrap**: For responsive and modern UI design.
- **bUnit**: For unit testing Blazor components.
- **xUnit**: For unit testing the application logic.

## Project Structure

- **RecipePortfolio**: The main project containing the Blazor WebAssembly application.
- **RecipePortfolio.Models**: Contains the data models used in the application.
- **RecipePortfolio.Services**: Contains the services for filtering recipes and updating tag counts.
- **RecipePortfolio.Shared**: Contains shared components used across the application.
- **RecipePortfolio.Test**: Contains unit tests for the application.

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Vorckea/RecipePortfolio.git
    ```
2. Navigate to the project directory:
    ```sh
    cd RecipePortfolio
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Running the Application

1. Build the application:
    ```sh
    dotnet build
    ```
2. Run the application:
    ```sh
    dotnet run --project RecipePortfolio
    ```
3. Open your browser and navigate to `https://localhost:5001` to view the application.

### Running Tests

1. Navigate to the test project directory:
    ```sh
    cd RecipePortfolio.Test
    ```
2. Run the tests:
    ```sh
    dotnet test
    ```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the Apache-2.0 License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [Bootstrap](https://getbootstrap.com/)
- [bUnit](https://bunit.dev/)
- [xUnit](https://xunit.net/)
