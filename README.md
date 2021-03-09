# CalculatorApp
Functionally, this is nothing more than your basic calculator. However, beneath the surface, this project serves as a sample implementation of the MVVM design pattern. MVVM stands for Model-View-ViewModel, and its primary use is separation of concerns and distribution of responsibilities.

# The Problem
So many instructional videos (paid or free) demonstrate implementing MVVM using a framework, e.g. Caliburn Micro, MVVMLight, Prism, ReactiveUI, etc. While I don't doubt these frameworks are beneficial, the problem for me is they seem to work like *magic*. And when magic breaks or doesn't work properly, now I'm up the creek. 

Thus I set out on a path to scrounge together, how do you implement MVVM, without a framework? Perhaps through a manual implementation, without relying on someone else's code, I could not only better understand MVVM, but understand why so many others go straight for a framework implementation. Who knows, by the end of this venture I could be embracing the usage of a framework, or maybe even I will discover I don't need one at all? *shrug*

After all, if the MVVM design patter is so popular and so important, why has Microsoft not incorprated an MVVM framework implementation directly into the language? I need to find all this out for myself...

# MVVM in WPF
When working with WPF, it literally means the code-behind is nearly empty. That UI code is purely contained in the XAML, and the code-behind is virtually empty, less of course the InitializeComponent(). Yes, this doesn't mean you can't have anything in the code behind. Just keep reading...

Rather than binding your UI elements to events in the code-behind (which is a tight coupling), the UI elements are bound (loosely) to the ViewModel. This is done by utilizing BINDINGS to the View-Model. These bindings could be one-way or two-way as you need them. Within your View-Model class, you typically have full-properties which back your interactive UI elements. Further, within your View-Model class, you implement something called INotifyPropertyChanged, which allows for subscriptions to property change events.  E.g. you have a button that gets enabled or disabled based on something the user is typing in another UI element. Essentially, your UI has a reactive nature through a system of property change notifications.

In replacement of event handling in the code-behind, such as a button click event that would normally be implemented in the code-behind, you can build a library of Commands that implement ICommand. In this method, you'll have reactive UI eventing, nicely separated in a library of Commands, coupled to the View-Model. Each ViewModel you implement would theortically get its own 'Commands' library, as needed. The ICommand implementation provides you evaluation to determine if the functionality can be executed (e.g. should the button be disabled or enabled), as well as an Execution method which can pass any necessary parameters.

# A Design Pattern, not a LAW
I should note that MVVM is a design-pattern, and not a LAW, in case I'm making it sound more strict that it really is. For example, if you had a button with pure UI-only interaction, there would be nothing wrong with utilizing the XAML code-behind. Violation of MVVM would occur, say if you incorporated business-logic into your View.

# The Point of MVVM
So, why all this work to achieve separation of concerns? Why make something so simple, incredibly difficult and different? In my view, it's the ability to separate developer from designer. The idea that, with a UI (the View) separated from the business logic, now we can swap in or out any UI we want. If we were in a classroom where everyone implemented their own calcualtor interface, all we would need to do to swap in the UI, is update the bindings to the ViewModel. That's it. It's about flexibility. Perhaps your app supports more than one type of user-interface.

I'm glossing over several more benefits, such as the ability to do Unit Testing on the Model and ViewModel, completely separate from the View. This can be essential for Test-Driven-Development or really any app heading for Production in a proper organization.

# Additonal Reading
This is an EXCELLENT post on WHAT/WHY/WHEN, all about MVVM...
https://stackoverflow.com/questions/2653096/why-use-mvvm

# In which layer of MVVM do I perform my Data Access?
None of them! Data Access should be performed in a separate Class Library project. Each component of MVVM is about your UI. By performing your Data Access in a Class Library, you maintain the ability to replace your entire UI with another. For example, you could have a Blazor Server project with a Web-based UI, utilizing the same Data Access Library as your WPF UI. The 'Model' part of Model-View-ViewModel is for Models related to displaying data (obtained from Data Access) in your UI.

Unfortunately this Calculator app is not the shining example of this concept. In the future I will look to pull-together a multi-UI app that uses a common Data Access library. For now, the topic is MVVM :-)
