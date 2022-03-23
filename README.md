# CalculatorApp
This project serves as a introductory implementation of the MVVM design pattern. MVVM stands for Model-View-ViewModel, and its primary use is separation of concerns and distribution of responsibilities.

# The Problem
So many instructional videos (paid or free) demonstrate implementing MVVM using a framework, e.g. Caliburn Micro, MVVMLight, Prism, ReactiveUI, etc. While I don't doubt these frameworks are beneficial, the problem for me is they seem to work like *magic*. And when magic breaks or doesn't work properly, now I'm up some kind of creek. For me it's about understanding and appreciating the design pattern, before exploring any fancy frameworks to help automate things.

# Courses that don't use a fancy MVVM framework to demonstrate MVVM
SingletonSean on YouTube: https://www.youtube.com/playlist?list=PLA8ZIAm2I03hS41Fy4vFpRw8AdYNBXmNm  
Eduardo Rosas on Udemy: https://www.udemy.com/course/windows-presentation-foundation-masterclass/

# Understanding MVVM in WPF
MVVM is all about serparation of concerns, promoting abstraction, testability, re-usability and readability. Rather than managing your UI element properties and events with the code-behind (which is a tight coupling), the UI elements are bound (loosely) to the ViewModel. This is done by utilizing BINDINGS between the View and the ViewModel. These bindings could be one-way or two-way as you need them. ViewModels implement something called INotifyPropertyChanged, which allows for subscriptions to property change events, e.g. you have a button that gets enabled or disabled interactively, perhaps based on some user input. Essentially, your UI has a reactive nature through a system of property change notifications.  

For replacement of event handling in the code-behind, such as a button click event, you can build a library of Commands that implement ICommand. In this method, you'll have reactive UI eventing, loosely coupled to the ViewModel. The ICommand implementation provides you evaluation to determine if the functionality can be executed (e.g. is ther user control enabled or disabled), as well as an execution method which can pass any necessary parameters.

# The Point of MVVM
So, why all this work to achieve separation of concerns? Why make something so simple, somewhat more difficult and different? In my view, it's the ability to separate developer from designer. The idea that, with a UI (the View) separated from the business logic, now we can swap in or out any UI we want. If we were in a classroom where everyone implemented their own calcualtor interface, all we would need to do to swap in the UI, is update the bindings to the ViewModel. That's it. It's about flexibility, agility and futurability.

I'm glossing over several more benefits, such as the ability to do Unit Testing on the Model and ViewModel, completely separate from the View. This can be essential for Test-Driven-Development or really any app heading for Production in a proper organization.

# Additonal Reading
This is an EXCELLENT post on WHAT/WHY/WHEN, all about MVVM...  
https://stackoverflow.com/questions/2653096/why-use-mvvm  

# Sister Project
For a more complex example of MVVM with WPF, refer to my EverNote clone app: https://github.com/CodeFontana/EvernoteApp

This project includes integrating the Microsoft Generic Host, WPF Navigation, Service Injection, etc. It was based on the Udemy course by Eduardo Rosas, but built upon by subjects from SingletonSean's Youtube Playlist, and built upon the Microsoft Generic Host inspired by the Tim Corey course on Dependency Injection.

# In which layer of MVVM do I perform my Data Access?
None of them! Data Access should be performed in a separate Class Library project. Each component of MVVM is about your UI. By performing your Data Access in a Class Library, you maintain modularity and separation of concern.

# .NET Generic Host
An essential component for facilitating MVVM in any project. Perhaps one day Microsoft will refresh all their non-ASP.NET project templates to be based upon the Generic Host model. This means Console, Desktop and Background Service projects should all have introductory templates revolving around the Generic Host Builder.

Refer to my start templates at: https://github.com/CodeFontana/CSharpProjectTemplates

The Microsoft Generic Host provides your app with Startup Configuration, Logging and Dependency Injection container. I strongly recommend Tim Corey's course on this subject-- https://www.iamtimcorey.com/p/dotnet-core-dependency-injection-in-depth

Note that this was not possible under the .NET Framework, and that the Generic Host Builder was introduced in .NET Core 2.1 and newer:  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0

Happy Coding.
