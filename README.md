# GymSystem

A .NET 10 Razor Pages application with ASP.NET Core Identity and SQL Server.

## Prerequisites

- **.NET 10 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server** (LocalDB or full version)
- **Visual Studio 2026** or **Visual Studio Code** with C# extension

## Getting Started

### 1. Clone the Repository
Determine where you want to store the project and
right-click in the folder > Open in Terminal (or Git Bash Here), then run:

```bash
git clone https://github.com/jkaykay/delta-gym-system.git
cd GymSystem
```
or download the ZIP and extract it.

### 2. Restore Dependencies
```bash
dotnet restore
```
or in Visual Studio, right-click the solution and select "Restore NuGet Packages".

### 3. Configure User Secrets (Connection String)

Store your local database connection string in user secrets instead of `appsettings.json`:

Right-click on the project in Visual Studio > Manage User Secrets
Add the following JSON, replacing with your actual connection string:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "YOUR_DB_CONNECTION_STRING_HERE"
  }
}
```

### 4. Apply Migrations and Seed Database
```bash
dotnet ef database update
```

or in the Package Manager Console:
```bash
Update-Database
```

### 5. Run the Application

```bash
dotnet run
```
or press F5 in Visual Studio.

The application will be available at `https://localhost:5001` (or the port shown in console output).
---
## Development Notes

- **Connection strings** are managed via [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development
- Never commit `appsettings.Development.json` or user secrets files (already in `.gitignore`)
- Database migrations are stored in the project; apply them with `dotnet ef database update`

## Troubleshooting

**Connection string not found?**
- Ensure user secrets are initialized: `dotnet user-secrets list`
- Verify the connection string key matches `"DefaultConnection"` in user secrets

**Database migrations not applied?**
- Ensure SQL Server is running
- Run `dotnet ef database update` from the project root

**Port already in use?**
- Modify the port in `Properties/launchSettings.json` or specify: `dotnet run --urls "https://localhost:5002"`

---
## Git Workflow

### Branch Structure

This project follows a branching strategy:
- **main** - Production-ready code
- **development** - Integration branch for features
- **feature/*** - Individual feature branches

### Working on a Feature

#### 1. Checkout to Development Branch

Start by pulling the latest changes from the development branch:
```bash
git checkout development
git pull origin development
```

#### 2. Create or Checkout to a Feature Branch

**Create a new feature branch:**
```bash
git checkout -b feature/your-feature-name
```
**Or switch to an existing feature branch:**
```bash
git checkout feature/your-feature-name
```
Use descriptive branch names, e.g.: `feature/user-authentication`, `feature/gym-class-scheduling`

#### 3. Commit Your Changes
Make your changes, then stage and commit them:
```bash
git add .
git commit -m "Brief description of changes"
```

#### 4. Push Your Feature Branch
```bash
git push origin feature/your-feature-name
```
If pushing for the first time, you may need to set the upstream:
```bash
git push -u origin feature/your-feature-name
```

### 5. Initiating a Pull Request

1. **Push your feature branch** to the remote repository (see Step 4 above)

2. **Open a Pull Request on GitHub:**
   - Go to [https://github.com/jkaykay/delta-gym-system](https://github.com/jkaykay/delta-gym-system)
   - Click the **"Pull requests"** tab
   - Click **"New pull request"**
   - Set the base branch to **development** and compare branch to **feature/your-feature-name**
   - Add a descriptive title and description of your changes
   - Click **"Create pull request"**

3. **Request code review** from team members and address any feedback

4. **Merge the PR** once approved

### 6. Syncing with Development (Merging)

If development branch has been updated and you want to pull those changes into your feature branch:
```bash
git fetch origin
git merge origin/development
```

**Merge conflicts?**
- Use `git status` to see conflicting files
- Resolve conflicts in your editor, then `git add` and `git commit`