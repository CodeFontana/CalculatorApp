# CalculatorApp
Functionally, this is nothing more than your basic calculator. However, beneath the surface, this project serves as a sample implementation of the MVVM design pattern. MVVM stands for Model-View-ViewModel, and its primary use is separation of concerns and distribution of responsibilities.

# The Problem
So many instructional videos (paid or free) demonstrate implementing MVVM using a framework, e.g. Caliburn Micro, MVVMLight, Prism, ReactiveUI, etc. While I don't doubt these frameworks are beneficial, the problem for me is they seem to work like *magic*. And when magic breaks or doesn't work properly, now I'm up some kind of creek.  

Thus I set out on a path to scrounge together, how do you implement MVVM, without a framework? Perhaps through a manual implementation, without relying on someone else's code, I could not only better understand MVVM, but understand why so many others go straight for a framework implementation. Who knows, by the end of this venture I could be embracing the usage of a framework, or maybe even I will discover I don't need one at all?  

After all, if the MVVM design pattern is so popular and so important, why has Microsoft not incorprated an MVVM framework implementation directly into the language? Dare I make a comparison to the Generic Host Builder as a container for Dependency Injection, Logging and Configuration? It's good to have lots of 3rd party and open-source options, but I need to discover MVVM for myself, and see where it takes me...

# MVVM in WPF
When working with WPF, it literally means the code-behind is nearly empty. That UI code is purely contained in the XAML, and the code-behind is virtually empty, less of course the InitializeComponent() and posisbly setting the DataContext to a particular ViewModel. Note that this doesn't mean your code-behind is always empty, just that beyond initializing and setting a DataContext, the code should be UI-only code, and not contain any part of the business logic or data modeling.

Rather than binding your UI elements to events in the code-behind (which is a tight coupling), the UI elements are bound (loosely) to the ViewModel. This is done by utilizing BINDINGS to the View-Model. These bindings could be one-way or two-way as you need them. Within your View-Model class, you typically have full-properties which back your interactive UI elements. Further, within your View-Model class, you implement something called INotifyPropertyChanged, which allows for subscriptions to property change events.  E.g. you have a button that gets enabled or disabled based on something the user is typing in another UI element. Essentially, your UI has a reactive nature through a system of property change notifications.  

In replacement of event handling in the code-behind, such as a button click event that would normally be implemented in the code-behind, you can build a library of Commands that implement ICommand. In this method, you'll have reactive UI eventing, nicely separated in a library of Commands, coupled to the View-Model. Each ViewModel you implement would theortically get its own 'Commands' library, as needed. The ICommand implementation provides you evaluation to determine if the functionality can be executed (e.g. should the button be disabled or enabled), as well as an Execution method which can pass any necessary parameters.

# A Design Pattern, not a LAW
I should note that MVVM is a design-pattern, and not a LAW, in case I'm making it sound more strict that it really is. For example, if you had a button with pure UI-only interaction, there would be nothing wrong with utilizing the XAML code-behind. Violation of MVVM would occur, say if you incorporated business-logic into your View.

# The Point of MVVM
So, why all this work to achieve separation of concerns? Why make something so simple, incredibly difficult and different? In my view, it's the ability to separate developer from designer. The idea that, with a UI (the View) separated from the business logic, now we can swap in or out any UI we want. If we were in a classroom where everyone implemented their own calcualtor interface, all we would need to do to swap in the UI, is update the bindings to the ViewModel. That's it. It's about flexibility. Or perhaps your app supports more than one type of user-interface.

I'm glossing over several more benefits, such as the ability to do Unit Testing on the Model and ViewModel, completely separate from the View. This can be essential for Test-Driven-Development or really any app heading for Production in a proper organization.

# Additonal Reading
This is an EXCELLENT post on WHAT/WHY/WHEN, all about MVVM...  
https://stackoverflow.com/questions/2653096/why-use-mvvm  

# In which layer of MVVM do I perform my Data Access?
None of them! Data Access should be performed in a separate Class Library project. Each component of MVVM is about your UI. By performing your Data Access in a Class Library, you maintain the ability to replace your entire UI with another. For example, you could have a Blazor Server project with a Web-based UI, utilizing the same Data Access Library as your WPF UI. The 'Model' part of Model-View-ViewModel is for Models related to displaying data (obtained from Data Access) in your UI.

Unfortunately this Calculator app is not the shining example of this concept. If you would like a shining example of this, I would encourage you to checkout this course:  
https://www.iamtimcorey.com/p/getting-started-with-azure

# Generic Host Builder
Speaking somewhere above about Microsoft's Generic Host Builder, as a Microsoft-provided container for Depencency Injection, Logging, & Configurtation-- I came across this wonderful article, with steps for wiring a WPF application to make excellent utilization of this:  
https://laurentkempe.com/2019/09/03/WPF-and-dotnet-Generic-Host-with-dotnet-Core-3-0/  

This is an excellent idea, so simple and so elegent, and I've incorporated in this example-- injecting the MainWindow (CalcWindow) and a simple class to use as a 'MathService'. This further allows us to decouple the core mathematical functions, from our user interface. With the MathService injected as a separate component, it can easily be Unit Tested :-) 

Note that this was not possible under the .NET Framework, and that the Generic Host Builder was introduced in .NET Core 2.1 and newer:  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0  
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=dotnet-plat-ext-5.0

Thus if you were developing within the .NET Framework and needed to create a project following the MVVM pattern, you were probably more apt to use an open-source framework, which may provide a Dependency Injection system, than go it alone, without any framework.

# Conclusion
At the time I'm writing this, the only logical conclusion I can draw, stems from the prior section above. If I was developing under the .NET Framework, I probably would be more apt to use an open-source framework, espcially if it provides me easier MVVM with some sort of Dependency Injection system. Thus far the jury is still out on .NET Core and newer, until I gain more experience. I need to practice more, and intentfully try building the same project, both with and without an added framework, to see what further conclusion(s) I'm able to draw.  

For now, Happy Coding.
