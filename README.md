# MAUI API Interaction

A .NET MAUI app (MVVM) and an ASP.NET Core Web API that work together for user authentication and item storage.

## ItemsWebAPI (HTTP, port 5082)

| Endpoint | Purpose |
|---|---|
| `GET /api/Auth` | Protected by the `BasicAuthentication` filter. Returns 200 for `Gonzalez01` / `Password1` (user name case-insensitive), otherwise 401 |
| `GET /api/Items` | All stored items |
| `POST /api/Items` | Adds an item. Returns 400 if a field is missing, 409 if the Item ID already exists |

Items are stored server-side in SQLite (`ItemsSQLite.db3`). Swagger UI: `http://localhost:5082/swagger`.

## MauiItemsApp (MVVM)

- **Views**: `LoginPage` and `ItemsPage` (XAML with data binding only; code-behind just sets the BindingContext)
- **ViewModels**: `BaseViewModel` (INotifyPropertyChanged, busy/message state), `LoginViewModel` (Login/Cancel commands), `ItemsViewModel` (Save command, ObservableCollection of items)
- **Services** (data access to the web service): `AuthService` (Basic Authentication login), `ItemService` (get/add items)
- **Models**: `Item`
- Views, View Models, and services are registered in `MauiProgram` and injected through dependency injection.

## Running

1. Start the API: `dotnet run --launch-profile http` in `ItemsWebAPI`.
2. Run `MauiItemsApp` on Windows Machine, log in as `Gonzalez01` / `Password1`, and add items.
