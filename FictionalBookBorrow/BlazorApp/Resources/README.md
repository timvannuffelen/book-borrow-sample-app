# A note on the use of resource files in this app
========================================================

In this app we chose to store all resources in a single resource file per language. Typically you would split various 
resources into different resource files. One strategy you could use is explained in the section below.

In this sample app, however, all resources are stored in **\Resources\App.[culture].resx** files.

The remainder of this README file explains how you can organize various resources in different resource files.


# How to organize resource files
========================================================

## Shared resources
Add resources that need to be available in multiple components to the **\Resources\App.[culture].resx** resource 
files. Access to shared resources is made available by injecting a *IStringLocalizer<App> SharedResourcesLocalizer* 
into your components.

## Localize components in the *\Features* folder and sub folders of the *\Features* folder.
Add the resource file for a given Razor component in the *Features* folder, or a sub folder of the *\Features* 
folder in the *\Resources* folder, so that the folder structure in the *\Resources* folder matches exactly the 
folder structure in the *\Features* folder. The name of the resource files needs to match the name of the Razor 
compoment: **[Name of Razor component].[culture].resx**  
Access to these resources is made available by injecting a *IStringLocalizer<[Component name]> ComponentLocalizer* 
into the component.

### Example
Resources for the component *\Features\BookAuthors\Create.razor* need to be defined in resource files 
**\Resources\Features\BookAuthors\Create.[culture].resx**.  

## Localize components in the *\Shared* folder
To make things more concrete we're going to use the *CookieConsentBanner* component as an example.

### Option 1
Inject a *IStringLocalizer<App> SharedResourcesLocalizer* into your component and specifiy your resources in the 
**\Resources\App.[culture].resx** resource files.

### Option 2:
Inject a *IStringLocalizer<CookieConsentBanner> ComponentLocalizer* and specifiy resources in the 
**\Resources\Shared\CookieConsentBanner.[culture].resx** resource files.

## Localize data annotions
Data annotations are localized via the **\Resources\DisplayNameResources.[culture].resx** resource files.

## Localize error messages
Error messages can be localized via the **\Resources\ErrorMessageResources.[culture].resx** resource files. 

To be able to make the localization work: 

1. Add two files to the *\Resources* folder: 

 - ErrorMessageResources.nl.resx
 - ErrorMessageResources.en.resx

2. Make sure the access modifier of the resource files is set to **No code generation**.

3. Add a class with the same name as the resource files (in this case "ErrorMessageResources") in the root of 
the *BlazorApp* project, and make sure the class is defined in the namespace *BlazorApp*.

4. Inject the localizer in the classes that need it. E.g.

```csharp
public ManageBookHandler(IStringLocalizer errorMessageResourcesLocalizer)
{
    _errorMessageResourcesLocalizer = errorMessageResourcesLocalizer;
}
```

The dummy class defined in step 3 is needed so the localizer can find the resource files. Before we added the 
class we saw in the debugger that the localizer was looking for the resources in a folder 
*BlazorApp/Resources/Resources*.

Notice the two occurences of the string "Resources" in the searched location. The result of this is that the 
error message resources can't be found.

The reason that the resource files initially couldn't be found is that the framework will check for the resource 
files relative to the class that is specified as the generic type parameter to the 
"Microsoft.Extensions.Localization.IStringLocalizer":

   Searched location = [Location class used in IStringLocalizer]/[Value of ResourcePath property]

Because the "ErrorMessageResources" resource files are located inside the Resources folder of the BlazorApp 
project, and the "ResourcePath" property is set to the value "Resources" when we add support for localization 
for the project, the framework will search for the resources in the following location:

   [Location class used in IStringLocalizer]/[Value of ResourcePath property] = BlazorApp.Resources/Resources

So that explains why the string "Resources" occured twice in the searched location. By placing the ErrorMessageResources 
class one level higher than the Resources folder, the searched location will be:

   [Location class used in IStringLocalizer]/[Value of ResourcePath property] = BlazorApp/Resources.

Boom! Problem solved.

So, to summarise, we fixed the problem by manually adding a class with the same name as the resource file in the 
root of the BlazorApp project. The namespace used in that file needs to be "BlazorApp". The class doesn't need 
any implementation as it is only used to provide the correct type to the IStringLocalizer.
