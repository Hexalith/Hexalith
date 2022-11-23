# Getting started with Hexalith

In this article, we are going to see how easy it is to create micro-services applications using the NuGet packages provided by Hexalith.

## Create an Hexalith service application

In Visual Studio, create a new empty .NET Core web application. Ex: `MyBridge.Server`. 

!!! note
    If you want to use the `preview` packages, [configure the Hexalith github preview package source](preview-package-source.md)

To add a reference to the package, right-click on the project and click on `Manage NuGet packages...`, check `Include prerelease` if required. If you added the preview source above, select this from the `Package Source` selection in the top right.  In the `Browse` tab, search for `Hexalith` packages and `Install` them.

### Getting Started with `Program.cs` 

Open `Program.cs` file. 

Add the following line 

```csharp
builder.Services.AddHexalith()
```

When you are done, the `Program.cs` file will something like this

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHexalith()

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.Run();
```
