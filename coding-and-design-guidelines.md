# C# Coding Guidelines

## Introduction
In this document we provide some coding and design guidelines that we would like you to use in this code base.  

We believe being consistent is a good thing in a lot of different situations, adhering to the same coding guidelines being one of them. Of course,
these are only guidelines, and therefore are in no way enforced upon anyone. 

This document is by no means a finished product, and changes and/or additions to this text are likely to occur.

### **Why use a coding standard**
As long as each developer works in complete isolation, coding guidelines are, in principle, not strictly necessary. That is not to say they can be benificial (on the contrary). However, from the moment on that 
multiple developers touch the same code, using the same set of guidelines can improve productivity a lot. Keep in mind that code is typically read a lot more than it written/changed, so keeping your code clear 
and easy to read for other developers will save us a lot of time in the future.

### **Current state of the code base**
Not all code in this code base is guaranteed to follow these guidelines. Of course, there is nothing that stops you from refactoring bad code, so that it does follow the guidelines in this text, when you encounter 
such code. Leaving code cleaner/better than you found it, is also known as the *Boy Scout Rule*. We encourage you to apply this rule as much as possible.

## Naming Conventions
<br>

| **Element**       | **Casing** | **Example**                         | **Remarks**                        |
|-------------------|------------|-------------------------------------|------------------------------------|
| Namespace         | Pascal     | `Domain.Exceptions`                 |                                    |
| Class             | Pascal     | `BookAuthorDto`                     | Even when you use known abbreviations in your class name, stick with Pascal case. E.g. Use `BookAuthorDto` instead of `BookAuthorDTO`. |
| Struct            | Pascal     | `Coordinate`                        |                                    |
| Record            | Pascal     | `BookAddedToReadingListEvent`       |                                    |
| Enum              | Pascal     | `BookType`                          | Use singular. E.g. `BookType` instead of `BookTypes`. |
| Interface         | Pascal     | `IBookRepository`                   | Interface names should always start with the letter `I`. |
| Method            | Pascal     | `CreateBookAuthor`                  |                                    |
| Property          | Pascal     | `PublicationDate`                   |                                    |
| Constant field    | Pascal     | `BaseUrl`                           |
| Non private field | Pascal     | `HasAdministratorRole`              |                                    |
| Private field     | Camel      | `_bookRepository`                   | Private instance fields should start with an *underscore*. For one, this avoids unexpected behaviour when you inject a dependency into a constructor, using the same parameter name and instance field name, and forget to prefix the instance variable with *this* when you assign the dependency to the instance field.
| Private static<br>readonly field | Pascal     | `AadInstance`                       |                                    |
| Method parameter  | Camel      | `bookAuthorId`                      |                                    | 
| Enum member       | Pascal     | `NonFiction`                        |                                    |
| Local variable    | Camel      | `activeBookAuthors`                 |                                    |
| CSS class name    | Kebap      | `btn-fluent-primary`                |                                    |
| HTML element ID   | Kebap      | `book-details`                      |                                    |
|                   |            |                                     |                                    |

<br>


## Naming Guidelines

In this section some heuristics are layed out you can use when naming things.

### **Be consistent**
Follow the naming conventions proposed above and you're all set ;)  

For example, don't use Camel case for a local variable in one place and Pascal case in another place. When looking at a variable name, it should be immediately obvious whether you're dealing with a local variable or a more global member variable. Our brain should not have to waste effort on figuring out things that can be made clear immediately. The consistent use of casings for local variables and member variables can help a great deal in accomplishing this goal.
<br><br>


### **Don't use fully qualified type names, unless it's necessary**
Extensive use of fully qualified names adds a lot of unnecessary clutter and makes your code more verbose. Also, using fully qualified names, for example in one of your lower-level helper methods, can make dependencies a lot less obvious. Indeed, one of quickest ways to determine the dependencies of a class is to look at the *using directives* of that class. When you use a fully qualified name for a type, that dependency is not visible when you scan the using directives of the class.

So instead of using the fully qualified name:
```csharp
var dto = FictionalBookBorrow.Infrastructure.DTO.BookQueryResult(); 
```

Import the namespace and use only the class name in the code.
```csharp
using FictionalBookBorrow.Infrastructure.DTO;

var dto = BookQueryResult(); 
```
Of course, there are going to be cases where you have no choice other than to use the fully qualified type name to avoid name collisions.
<br><br>


### **Favor long descriptive names over short unreadable names.**
Ideally a short descriptive name makes clear the intent you want to convey. Many times, however, a short name just isn't going to cut it, and you need to turn to longer names. In such cases is it almost always better to go for a longer descriptive name then to use a short name that doesn't clearly state your intent. This is true for variable names, method names as well as for names of other constructs such as classes and interfaces.

**Example**

```csharp
// Don't do this.
string studProgDescription;
var res;

// Do this instead.
string studyProgrammeDescription;
var result;
```
<br>


### **Don't use abbreviations in names, unless they are well-known**
Generally speaking abbreviations should be avoided when naming things, with the exception if the exeption is a well-known acronym.  

It is, for example, perfectly fine to use names like `SapBookDto` and `httpClient`. In fact, it obviously is preferred to use the class name `SapBookDto` instead of `SystemeAnwendungenUndProdukteBookDataTransferObject`. This last name also violates the use of *US English* as the preferred language to write our code in. 
<br><br>


### **Don't use prefixes**
Avoid prefixes when naming things. In any case don't use (Systems) Hungarian notation in a strongly typed language like C#.  

Also, in UI code it might be tempting to prefix the name of a UI control with the type of that control. However, when you're not bounded by a character limit it probably is more readable to use the full description of the control as a postfix.

**Examples**
```javascript
  // Don't do this.
  let lblTitle = $("#lblTitle").text();
  let txtTitle $("#txtTitle").text();
  let ddlType $("#ddlType").val();

  // Instead, do this.
  let titleLable = $("#title-label").text();
  let titleTextBox $("#title-textbox").text();
  let typeDropDownList = $("#type-dropdownlist").val();
```

**Exceptions**  
The use of the `_` prefix for private fields. One of the main reasons we chose to keep using this prefix for private fields is that it avoids errors like the one in the following code snippet.

```csharp
private readonly IBookRepository bookRepository;

public BookController(IBookRepository bookRepository)
{
  // Forgetting to prefix the instance field with "this" causes you to assign the parameter to itself. 
  bookRepository = bookRepository;
}
```
<br>


### **Use consistent naming for similar operations**
Be consistent throughout your code base when you choose names for operations that do certain things. As an example, don't mix the verbs *get*, *list* and *retrieve* when defining various methods for retrieving things.

```csharp
// Don't do this.
IEnumerable<int> GetPublicationYears();
BookAuthor RetrieveBookAuthor(int id);
IReadOnlyCollection<Book> ListBooksFromAuthor(int authorId);

// Do this instead.
IEnumerable<int> GetPublicationYears();
BookAuthor GetBookAuthor(int id);
IReadOnlyCollection<Student> GetBooksFromAuthor(int authorId);
```

Which verb you choose here is totally up to you, but make sure that you consistently use the same verb throughout your code base.
<br><br>


### **Don't repeat the name of a type in it's methods**
Repeating the name of a type in some of it's methods is redundant information and only clutters your code. 

```csharp
public interface IBookAuthorRepository
{
  // Don't repeat the noun used in the repository name in the methods. 
  BookAuthor GetBookAuthor(int id);
  void CreateBookAuthor(BookAuthor author);
  void UpdateBookAuthor(BookAuthor author);
  void DeleteBookAuthor(int id);

  // Use method names without repeating the noun instead. 
  BookAuthor Get(int authorId);
  void Create(BookAuthor author);
  void Update(BookAuthor author);
  void Delete(int authorId);
}
```

**Exceptions**  
When using factories is the name of the object that will be created typically is used in the factory method's name, even if the object type is already used in the name of the factory (that is, the class) itself.

```csharp
public sealed class BookFactory
{
  public static Book CreateBook(int typeId);
}
```

---

## Class Design Guidelines

### **Make classes sealed by default**
Make your classes `sealed` by default.   

Just like you should use the most strict access modifier for your types and members, you should also start with the most strict configuration when it comes to inheritance. Marking a class as `sealed` prevents (accidental) misuse of the class. Only when you have explicitly determined your class needs to act as the base class of another class you can remove the `sealed` modifier, allowing other classes to inherit from it. 
<br><br>

### **Avoid using static classes**
With the exception of classes that expose extension methods, in many cases static classes are best avoided. When you use static classes you can not (easily) swap the dependency on that static class for a fake version in your tests.

```csharp
// Don't take a dependency on a static class in your constructor.
public class BookController
{
    private readonly IBookRepository _bookRepository;

    // The class does not communicate clearly to it's clients what dependencies it needs through it's constructor.
    public BookController()
    {
        // There is no way to easily swap the dependency to the static BookFactory for a test double in your tests.
        _bookRepository = BookRepositoryFactory.CreateBookRepository(); // Here we use a reference to a static factory.
    }
}

// Instead inject the repository into your constructor.
public class BookController
{
    private readonly IBookRepository _bookRepository;

    // The constructor clearly states what dependencies the class expects to be able to do it's work.
    public BookController(IBookRepository bookRepository)
    {
        // Instead of a static factory (or service locator), we use dependency injection to inject the repository in our controller. This allows us to easily pass in 
        // a fake repository in our tests.
        _bookRepository = bookRepository;
    }
}

```

### **Remove not required using directives**
Don't keep using directives that are not required by the code in your source code files. They serve no other purpose than to clutter your code.  
<br>
In Visual Studio you can use the very convenient shortcut **Ctrl + R, Ctrl + G** to 1) remove not required usings and 2) sort your using directives alphabetically.
<br><br>

---

## Language Guidelines

### **Use C# type keywords instead of the .NET types**
Don't use the .NET types for primitive types. All they do is make your code more verbose.

```csharp
// Don't do this.
Object response;
var typeId = Int32.Parse(id);
Int16 index;
Int64 nextFreeNumber;
String title;

// Do this instead.
object response;
var typeId = int.Parse(id);
short index;
long nextFreeNumber;
string title;
```

A complete list of the built-in types can be found [here](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types).
<br><br>


### **Use language shortcuts for nullable types and value tuples**
The C# language provides useful shortcuts when you need to work with nullable types or value tuples. Use them. 

```csharp
// Don't use the verbose System.Nullable<T> structure to declare nullable variables.
Nullable<int> amount;

// Use the more concise T? shorthand syntax instead.
int? amount
```

```csharp
// Don't use the verbose System.ValueTuple<T1,T2,...> structure to declare (value) tuples.
ValueTuple<string, string> tuple;

// Use the more concise (T1,T2,...) shorthand syntax instead.
(string, string) = tuple;

// On a side note, when you use the shorthand notation you can specify the names of the tuple fields in the initialization expression to make your code more readable. 
(string bookId, string publicationDate) tuple;

```
<br>


### **Use var only when the type is very obvious**
Only use *implicitly typed* local variables when the type of the variable is immediately obvious.

**Examples**
The first example is an example where the use of the `var` keyword makes perfect sense. 
```csharp
// The type of model is immediately clear. Use of the var keyword is perfectly fine here. 
var model = new CreateBookAuthorViewModel();

// Note that in C# language version 9.0 or greater you can also use target-typed new expressions. This way you don't have to use the var keyword and you don't 
// have to repeat the type. Winner winner chicken dinner.
CreateBookAuthorViewModel model = new();
```

However, there are other cases where the use of `var` should be avoided. For one, the type is not immediately obvious from the code. Secondly, this type of code makes refactoring harder because you can't navigate to the class definition of the returned type as easily.
```csharp
// Don't do this.
var response = GetResponse();
var book = BookFactory.Create(bookTypeId);

// Do this instead.
CreateBookResponse response = GetResponse();
Book book = BookFactory.Create(bookTypeId);
```
<br>


### **Use the ternairy conditional operator for simple assignments**
Simple assignments that depend on a condition can be implemented more concisely with the use of the *ternary conditional operator*.

```csharp
// Assignment of a CSS class without the use the ternary conditional operator.
string iconClass;

if (isLeafNode) 
{
  iconClass = "fas fa-caret-right";
} 
else 
{
  iconClass = "fas fa-chevron-right";
}

// Assignment of a CSS class with the use the ternary conditional operator. Much better, right?
string iconClass = isLeafNode ? "fas fa-caret-right" : "fas fa-chevron-right";
```
<br>


### **Favore the use of expression-bodied members for single line bodies**
Expression bodied-members provide a clean way to express certain member bodies, in particular when the member's body fits on a single line. For example, read-only properties can be expressed a lot more concise using an expression-bodied property. Note that when you use an expression body, you do *not* need to use a `return` statement to return the value. When an expression body evaluates to `void`, no value (or `void`) is returned. 

```csharp
// Implementation that does not use expresion-bodied syntax.
public bool TotalAmount
{
    get { return AmountInactive + AmountActive; }
}

// By using an expression body we can make the implementation more concise. 
public bool TotalAmount
{
    get => AmountInactive + AmountActive;
}

// We can even further refactor the above code to this very clean, readable and concise implementation. I would argue this looks a lot more clean that the previous 
// implementations to most developers.
public bool TotalAmount => AmountInactive + AmountActive;
```
You can use expression body definitions for 
- properties
- methods
- constructors
- finalizers
- indexers
<br><br>


### **Use auto-implemented properties when no logic is necessary**

Use auto-implemented properties whenever you only need simple *get-and-set* functionality for one of your class members. A typical example of this functionality can be found in your DTOs.

```csharp
// Don't use the old verbose implementation that uses an explicit (private) backing field. 
private bool _isActive;
public bool IsActive
{
  get => _isActive; 
  set => _isActive = value;
}

// Auto-implemented properties for the win!
public bool IsActive { get; set; }
```

Of course, there are going to be situations where you need additional logic for one of your properties. If that's the case, don't hesitate to use a property with a backing field.
<br><br>

### **Initialize collection types to an empty collection**
Client code might not expect a `null` value which can result in a `NullReferenceException` when the calling code does not check for `null`. A typical example is calling `Count` or `Any()` on a collection that is `null`.  

Initializing collections to an empty collection solves this problem. Clients also do not need to check for `null` any longer, which results in less clutter in the client code.
<br><br>

### **Expose collections as much as possible as *read-only* collections**
Not exposing collection properties as read-only collections allows client code to add items to the collection or remove items from the collection. Even when you expose your collection as an `IEnumerable`, smart developers can determine the underlying collection type, for example by using a decompiler or reflection, and use it to change the contents of the collection.

Therefore, expose your collection objects as much as possible as read-only collections to clients. The standard way of wrapping a collection, and make it available as a read-only collection, is by using `IReadOnlyCollection`, `IReadOnlyList` or `IReadOnlySet`.  

```csharp
private readonly List<BookAuthor> _authors = new List<BookAuthor>();

public IReadOnlyCollection<BookAuthor> Authors => _authors;

// Add extra methods that operate on the list, needed by your clients, to the public interface of the class. These methods can, of course, work with the mutable 
// _authors list.
```

### **Use file scoped namespace when your language version allows it**
When using C# language version 10 or greater use *file scoped namespaces* to decrease indentation.  

With the old block scoped namespaces you define the type *inside* a namespace block, which indents the type definition.

```csharp
// Using directives

namespace BookManagement.Application.Features.BookOrders.Commands.ProcessOrderStatus
{
    public class ProcessOrderStatusCommand : IRequest<Result>
    {
        // Implementation here.
    }
}
```

With file scoped namespaces the type is defined on the same level as the namespace, resulting in no extra level of indentation. Note that the namespace needs to be followed by a semicolon when you use a file scoped namespace.
```csharp
// Using directives

namespace BookManagement.Application.Features.BookOrders.Commands.ProcessOrderStatus;

public class ProcessOrderStatusCommand : IRequest<Result>
{
    // Implementation here.
}
```

### **Use global using directives when your language version allows it**
When using C# language version 10 or greater use *global using directives*. This allows you to declare heavily used namespaces in one place and have those namespaces available in all source code files. The use of global using directives helps to keep the list of using directives in your classes shorter.

However, *don't spread global using directives over multiple files in the same project*. Instead create a file *GlobalUsings.cs* per project and add all global using directives needed in the project to that file.

*GlobalSettings.cs* in the *Domain* project could look like this:
```csharp
global using FictionalBookBorrow.Domain.Common;
```

Or, when you use a more infrastucture-driven packaging approach:
```csharp
global using FictionalBookBorrow.Domain.Common;
global using FictionalBookBorrow.Domain.Entities;
global using FictionalBookBorrow.Domain.Enums;
global using FictionalBookBorrow.Domain.Events;
global using FictionalBookBorrow.Domain.Interfaces;
global using FictionalBookBorrow.Domain.Services;
global using FictionalBookBorrow.Domain.ValueObjects;
```
Note, however, that you should try to avoid this type of packaging structure when you adopt a Domain-Driven Design approach in your project. You should group conceptually cohesive objects together instead. 

*GlobalSettings.cs* in the *Infrastructure* project could import the following namespaces:
```csharp
global using FictionalBookBorrow.Infrastructure.DTO;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
```

A disadvantage of global using directives is that they make it less obvious which dependencies a particular class (or interface, struct or record) has. Indeed, just like with the use of fully qualified type names, you cannot quickly determine all dependencies of a type by looking at the using directives at the beginning of the source code file in which the type is defined. Some of the dependencies will be imported via global using directives, which are defined in the *GlobalUsing.cs* file. Moreover, looking at the imported namespaces in the *GlobalUsings.cs* file, does not tell you which dependencies are used by a certain class. Indeed, a particular class might only use a subset of all globally imported namespaces.

---

## Layout Guidelines

### **Use guard clauses to reduce the nesting of if statements**
The use of multiple nested `if`-statements makes the happy path of your code a lot less obvious. One way to make your code more clear is to use guard clauses that return or throw an exception as soon as they detect a situation that causes the flow to deviate from the happy path.  

Also, when using deep nesting in combination with a long method body (which, by the way, is also something you should avoid as much as possible) it can be hard to determine which `if` condition belongs to a certain `else` branch, making it a lot harder to quickly understand what the code is doing or, even worse, reading the code incorrect when you match an `else` branch with the wrong `if` condition. 

```csharp
// Deep nesting hides the happy path somewhere in a nested if-statement. It's also not very clear which else branch belongs to which if-condition.
if (conditionA) {

  if (conditionB) {

    if (conditionC) {
      // Do the actual work the method needs to do.
    }
    else {
      // throw or return    
    }
  }
  else {
    // throw or return
  }    
}
else {
  // throw or return
}
  
// The use of gaurd clauses removes the nesting, making the actual work to method is supposed to do more clear.

if (!conditionA) {
  // throw or return
}

if (!conditionB) {
  // throw or return
}

if (!conditionC) {
  // throw or return
}
 
// Do the actual work the method needs to do.

```
<br>


### **Don't add multiple empty lines between different code blocks**
Be consistent with the use of empty lines. Empty lines should be used to seperate unrelated code blocks but don't add 1 empty line between some code blocks and 5 empty lines between other code blocks.
<br><br>

### **Break long expression bodies *after* the arrow**
Ideally expression-bodied members are implemented in a single line. Sadly, this is not always possible without the need to scroll horizontally. Break long expression bodies *after* the arrow sign.

```csharp
// Don't break long lines before the arrow sign.
private bool CanLendBooksToReaders() 
    => HasAdministratorRole || User.IsInRole(AppRoles.BookLender);


// Instead, break long lines after the arrow sign.
private bool CanLendBooksToReaders() => 
    HasAdministratorRole || User.IsInRole(AppRoles.BookLender);
```

---

## Documentation Guidelines

<br>

> <br>  
>
> *Documentation is a love letter that you write to your future self.*    
> 
> -- Damian Conway --
>   
> <br>

<br>

### **Use XML docs**
Make use of XML elements when documenting a public API. Private helper method, in principle, do not need explicit documentation but instead should have names that make the intent of the method very clear.  

An overview of the [recommmende tags](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags) and some [examples](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/examples) can be found in the Microsoft documentation. 
<br><br>


### **Document each class, interface, struct, record**
It doesn't always have to be an elaborate description but even adding a short description to explain the function of, or intent for having, a custom type can help other developers a whole lot when they need to use your type.  

---

## General Design Guidelines

### **A method should do only one thing**
Closely related to the _Single Responsibility Principle_, which you might recognize as being one of the [SOLID design principles](https://en.wikipedia.org/wiki/SOLID).  

One of the easiest ways to make a complicated mess of your methods is to make them do multiple things. By keeping your methods short and focused on doing one thing you keep them easier to understand.

One way to (partly) achieve this is to use the *Command-Query Separation* principle, which states that a routine should either retrieve data and return the result, or change the state of a system but do not return a result. When adhering to this principle you should **not**:
- use a method named `Get[Noun]` or `Create[Noun]` and have that method alter the state of your program, that is, produce a side-effect.
- use a method named `Set[Noun]` and have that method return a value.

**Exceptions**  
One way to implement error handling is to let your commands return a result, for example by returning a numeric or enum value that acts as the result of an operation that modified the state of the program. Of course, when you use this error-handling technique you have let your commands return some value.
<br><br>


### **Don't use magic numbers and magic strings**
Avoid the use of magic numbers. Introduce a well-named constant that makes clear what the number stands for.

This is one of the ways to raise the abstraction level in your code, allowing you to program in the problem domain, instead of in lower-level implementation details.

```csharp
// Don't do this.
List<Level> childLevels = _levelRepository.GetAllLevels.Where(level => level.Id != 1).ToList();

// Do this instead.
const int rootLevelId = 1;
List<Level> childLevels = _levelRepository.GetAllLevels.Where(level => level.Id != rootLevelId).ToList();


// Don't do this.
if (Type == 2)
{
}

// Do this instead.
const int NonFiction = 2;
if (Type == NonFiction)
{
}
```

Moreover, you also should not use magic strings in your code. Although magic strings don't suffer from the readability problem that magic numbers have, when you use a magic string in multiple places you need to change it in each of those places when the value of that string changes. If you save the magic string in a constant local variable or class member you only need to change it in that one location when the name of the connection changes. Remember, encapsulate what varies!

Don't pass the name of the BookBorrow connection as a string value each time you need it.
```csharp

// Method X in class A.
using (var dbContext = new DAC("BookBorrowDbContext"));

// Method Y in class A.
using (var dbContext = new DAC("BookBorrowDbContext"));

// Method Z in class B.
using (var dbContext = new DAC("BookBorrowDbContext"));
```

Instead save the connection name in a constant, and use that constant throughout your code.
```csharp
internal static class ConnectionNames
{
    internal const string BookBorrow = "BookBorrowDbContext";
}

// Method X in class A.
using (var dbContext = new DAC(ConnectionNames.BookBorrow));

// Method Y in class A.
using (var dbContext = new DAC(ConnectionNames.BookBorrow));

// Method Z in class B.
using (var dbContext = new DAC(ConnectionNames.BookBorrow));    
```
<br>


### **When using DDD principles don't put the data model at the center, but instead the domain model**
Not all projects need to use the principles explained in Domain-Driven Design (DDD), but when you do use a DDD approach, make your domain model the center of your design, not the database.

All the core business logic should be implemented in a *Core Domain* layer, using good object design and properly encapsulated in DDD building blocks like *Aggregates*, *Entities*, *Value Objects*, *Services*, *Repositories* and *Factories*. In any case do not put domain logic in SQL queries or stored procedures, and don't consider your datamodel (database scheme and tables) the foundation of your design. 

A lot more can be said about Domain-Driven Design but this is not the time, nor place, to elaborate on this topic.
<br><br>


### **Objects should contain both data and behaviour**
Good object design is about assigning the right responsibilities to the right objects. Each object should have both behaviour and data, where the data is properly encapsulated and abstracted behind a well-designed public interface. Don't make your domain objects nothing more than bags of data (DTOs if you will) that is set by calling code. This approach is characterized by a complete lack of encapsulation and requires calling code to check whether or not a certain value needs to be set on the domain object. This leads to an *anemic domain model* that lacks behaviour.  

Instead you should adhere to the [Tell-Don't-Ask](https://martinfowler.com/bliki/TellDontAsk.html) principle which states that you should tell a domain object what to do and let it do all the work to handle your request, instead of letting the client code ask the domain object for some data which is then operated on by the client code.

**Exception**  
Data Transfer Objects. DTOs should be no more than containers for data and therefore should never contain behaviour or logic.
<br><br>

### **Introduce abstractions that allow you to program in the problem domain space**
Don't code in lower level implementation details. Introduce Boolean variables and helper methods with well-chosen names. Introducing helper methods also helps to keep your methods short and focussed.

```csharp
// Don't do this.
if (TempData.ContainsKey(TempDataKey.IsRedirected) && (bool)TempData[TempDataKey.IsRedirected])
{
    // Handle redirected users here.
} 

// Do this instead.
bool userWasRedirected = TempData.ContainsKey(TempDataKey.IsRedirected) && (bool)TempData[TempDataKey.IsRedirected];

if (userWasRedirected)
{
    // Handle redirected users here.
} 
```

The example above uses a local variable with a clear name to raise the abstraction level of the code. Other ways of achieving this goal this is to avoid the use of magic numbers, and to introduce well-named helper methods and by introducing custom types. Indeed, the use of *Abstract Data Types (ADTs)* like classes are a great way to raise the abstraction level in your code base and to allow programmers to write code in terms of the problem space.
<br>


### **Organize code files by cohesive concept, not by type or the pattern they follow**
Organize your classes and interfaces by the domain concept they represent, not by the type of the class or the pattern they follow (entity, value object, event, repository, ...). Keep code that implements a cohesive concept in the same module<sup>1</sup>. Indeed, organizing your types by the pattern they follow results in the grouping of uncohesive objects in the same module (so much for _high cohesion_). Moreover, such packaging results in many dependencies between different modules (say bye to *low coupling*) because, say, an entity used to implement a certain concept needs access to value objects, events, factories, services, ... that are used to implement that same concept. When each of those classes lives in a different package, you obviously create many dependencies between those packages.

Grouping classes by the pattern they follow results in a package structure like the one below, with each class of a specific type contained in the folder that represents that type. The example shows classes associated with different concepts grouped together in the same module/folder.

- **MyApp.Core**
  - Aggregates
    - *Book.cs*
    - *Person.cs*
  - Entities
    - *UserAccount.cs*
    - *BookAuthor.cs*
    - *BookAuthorLink.cs*
  - Enums
    - *BookType.cs*
    - *Organization*
    - *AuthorType.cs*
  - Events
    - *BookAuthorCreatedEvent.cs*
    - *UserAccountDisabledEvent.cs*
  - Factories
    - *BookFactory.cs*
    - *PersonFactory.cs*
  - Services
    - *AzureService.cs*
    - *BatchService.cs*
  - ValueOjects
    - *Isbn.cs*
    - *DateTimeRange.cs*
    - *GroupMembership.cs*

A better way to organize your classes is via the package structure below. Each class that is used to implement the concept *BookManagement* is added to a BookManagement folder or a subfolder of the *BookManagement* folder, no matter what the type of the class is (entity, value object, event, ...). You can, however, use some subfolders to organize your classes inside the BookManagement folder. In this example subfolders are used to organize events, factories and services, used for implementing the *BookManagement* concept, in a separate folder. Other subfolder trees are possible, of course.

- **MyApp.Core**
  - BookManagement
    - *Isbn.cs*
    - *BookType.cs*
    - *Book.cs*
    - *BookAuthorLink.cs*
    - Events
      - *BookCreatedEvent.cs*
    - Factories
      - *BookFactory.cs*
    - Services
      - *AzureService.cs*

<br>

1  We're using *modules*, *packages* and *folders* interchangeably. Indeed, we typically use a folder structure to define our modules/packages.
<br><br>

### **First make it work, then refactor to a better design and more readable code**
In many cases implementing a feature so that it does what it's supposed to do is the easy part. Also in many cases, your work is not done there. Once your code does what was asked for it rarely is in a state that you want to leave behind for your team members or your future self. You need to refactor and improve it. "*First make it work. Then make it right*", Kent Beck said. Make sure you leave behind code that doesn't have to be deciphered when you come back to it some time later.  

Techniques such as TDD (Test-Driven Design) can help you with creating, in an incremental fashion, the right interfaces for a clean and decoupled design.
<br><br>

---

## Miscellaneous Guidelines


---

# Modules in Domain-Driven Design

## Don't: Use Infrastructure-Driven Packaging
You shouldn't use _infrastructure-driven_ packaging. This typically causes it to be difficult to look at various objects and mentally fitting them back together as a single conceptual entity. We also loose the 
connection between the model and the design, and violate the standard of _high cohesion/low coupling_. Therefore, keep all the code that implements a single concept in the same module.  

When you group objects according to the pattern that they follow the result is that objects that have conceptually little relationship (low cohesion) are crammed together, and associations run willy-nilly between 
all the __modules__. An example of this type of partitioning is grouping alle _entities_ together, grouping all _value objects_ together and grouping all _services_ together.  

## Do: Organize objects by domain concept
A better way to group would be to group objects conceptually, so that all objects that represents a single concept in the domain are grouped together in a single module. Inside a module you may or may not further 
group objects based on the pattern they follow. In the example below we group objects inside the __BookManagement__ module further based on the (technical) type of the class.  

- FictionalBookBorrow.Domain
  - BookManagement
    - Aggregates
    - Entities
    - ValueObjects
    - Interfaces

Objects that need to be shared between multiple modules can be housed in a __Common__ or __Shared__ folder.  

- FictionalBookBorrow.Core
  - Common
    - Aggregates
    - Entities
    - ValueObjects
    - Interfaces
