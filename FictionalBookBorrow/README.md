# Static Code Analysis in the solution
To install the _SonarAnalyzer.CSharp_ static code analyzer to __all__ projects in the solution we add a _Directory.Build.props_ 
file in the root of the solution. After the file is created, add the following content to the file to add the static code analyzer 
to all projects in the solution.

```xml
<Project>
	<PropertyGroup>
		<AnalysisLevel>latest</AnalysisLevel>
		<AnalysisMode>all</AnalysisMode>
		<TreatWarningsAsErrors>false</TreatWarningsAsErrors>
		<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="SonarAnalyzer.CSharp" 
				  Version="9.6.0.74858" 
				  PrivateAssets="all" 
				  Condition="$(MSBuildProjectExtension) == '.csproj'" />
	</ItemGroup>
</Project>
````

This way we don't have to manually add the NuGet package to each project. The package reference to the static code analyzer in 
the _Directory.Build.props_ file adds the NuGet package to __all__ projects when you build the solution.

By setting the _TreatWarningsAsErrors_ property to __true__ the compiler will transform each compiler warning into an error.
