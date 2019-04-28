# TfL Road Status

This project is created with Visual Studio 2019 for Mac using .NET Core 2.1.

## Build
* Terminal: Switch to solution folder and run `dotnet build`. 
* Visual Studio: Open the solution with Visual Studio and click build.

## Running
* Terminal: Switch to solution folder and run `dotnet run --project TfL.RoadStatus` or `dotnet run --project TfL.RoadStatus [ROAD_NAME]`.
* Visual Studio: Hit Option + Command + Enter on Mac, Control + F5 on Windows

If you do not provide [ROAD_NAME] when running the application, it will ask for it.

If the road name is valid you'll see this result:

```
$ dotnet run --project TfL.RoadStatus A10
The status of the A10 is as follows
    Road Status is Good
    Road Status Description is No Exceptional Delays
```

If the road name is invalid you'll see this result:

```
$ dotnet run --project TfL.RoadStatus A10f
A10f is not a valid road.
```


## Tests
Tests were written using NUnit. They should simply run when `Run All Tests` hit. It might need NUnit3TestAdapter to be installed from NuGet.

## Explanations
To keep the code simpler I've made some assumptions and chosen some shortcuts.

* RoadStatusPolicy: Base url is kept in this file instead of appsettings.json. I didn't use seperate validator and returned null instead of throwing any exception.

I first started a bit complex and then simplified the code. Also to make (and keep) it simple I ignored exception response object and returned null.

Thank you.
