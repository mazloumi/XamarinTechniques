# Welcome to Xamarin Techniques!

This repository is a playground for exploring various ideas and learning more about Xamarin.

## When to Use What Technique in Xamarin.Forms?

One question relates to better understand when to use what technique. This section compares various techniques applied to a simple use case where the background color of a view changes each when a control gains focus.

### Evaluation Criteria
Possible evaluation criteria could be:
 1. **Availability of technique**. How many controls the technique can be applied to.
 2. **XAML-only**. The extent the solution requires C# code or can be purely implemented in XAML
 3. **Lines of Code**. The number of lines of code needed to solve the problem
 4. **Shared project only**. The extent the solution can be implemented in the Forms project or platform-specific projects
 5. **Brevity of XAML**.  How readable and brief the XAML code is
 6. **Reusability of approach**.  Can a functionality can be written once and used across all similar controls

### Techniques
The table shows the techniques examined:
|Technique|What is it?|Location|Mainly|XAML Syntax|Thoughts|Lines of Code in C#|Available
|-|-|-|-|-|-|-|-
|**Attached Property**|Public Class with public static readonly`BindableProperty`, Getter and Setter and possibly`PropertyChanged` delegate |Shared project|C#|`<Entry local:MyClass.MyProperty="Some Value"/>`|Useful for extending View including wrapping other techniques listed here. Brevity of XAML.|39|Any XAML element
|**Behavior**|Sub class of `Behavior<T>` with overrides for `OnAttachedTo` and `OnDetachingFrom`|Shared project|C#|`<Entry><Entry.Behaviors><local:MyBehavior /></Entry.Behaviors></Entry>`|Useful for extending the View|23|`VisualElement.Behaviors`
|**Effect**|Sub class of `RoutingEffect` and platform-specific sub classes of `PlatformEffect` with overrides for `OnAttached`, `OnDetached` and possibly `OnElementPropertyChanged`|Shared project and platform-specific projects|C#|`<Entry><Entry.Effects><local:MyEffect /></Entry.Effects></Entry>`|Access to native control, events and properties|67|`Element.Effects`
|**Event handler**|View with a delegate for an event|Shared project Code Behind|C#|`<Entry Focused="Entry_Focused" />`|Usage limited to specific view used in the XAML|5|All Events available for a view
|**Property Trigger**|`Trigger` for specific `Property` and `Value` of a `TargetType` and `Setter` elements to change specific properties of the target type|Shared project|XAML|`<Entry><Entry.Triggers><Trigger Property="IsFocused" Value="True"><Setter Property="BackgroundColor" Value="Yellow"/></Trigger></Entry.Triggers></Entry>`|Scope limited but effective.|0|`VisualElement.Triggers` and `Style.Triggers`
|**Renderer**|Optional sub class of a visual element and platform-specific sub classes of the element's renderer class with override of `OnElementChanged` method evaluating `OldElement` and `NewElement` member of `ElementChangedEventArgs<T>`|Platform-specific projects|C#|`<local:MyControl/>`|Full access to the native control used even allowing it to be replaced|72|40 renderer base classes
|**Tap Gesture Recognizer**|`TapGestureRecognizer` with `Tapped` delegate triggered after `NumberOfTapsRequired`|Shared project|XAML|`<Entry><Entry.GestureRecognizers><TapGestureRecognizer Tapped="Entry_Focused" NumberOfTapsRequired="1" /></Entry.GestureRecognizers></Entry>`|Listen to user interaction and extend behavior. Event handler can be in the Code Behind or a command in the view model.|5|`View.GestureRecognizers` 

### Comparison
The table evaluates the extent each technique contributes to a particular evaluation criteria. The perceived winner is highlighted bold.
|Technique|Availability|XAML Only|Minimum LoC|Shared Project Only|Brevity|Reuse
|-|-|-|-|-|-|-
|Attached Property|**100%+**|No|Medium|Yes|**1 Line**|**Highest**
|Behavior|86%|No|Medium|**Yes**|3 Lines|Medium
|Effect|100%|No|High|No|3 Lines|High|High
|Event|100%|No|Low|**Yes**|**1 Line**|Lowest
|Property Trigger|86%|**Yes**|**0**|**Yes**|4 Lines|High
|Renderer|100%|No|High|No|**1 Line**|Low
|Tap Gesture Recognizer|72%|No|Low|**Yes**|3 Lines|Medium-Low

### Conclusion
For the specific use case property trigger seems to be the most suitable approach. With 4 lines of XAML the problem is solved. The approach is not available though for all controls. What makes property triggers also attractive is that they can be defined in styles making them highly reusable. Next in line could be Attached Properties and Behaviors, which essentially are wrappers for Event handlers or even Tap Gesture Recognizers making them independent of the Code Behind. Effects and Renderer seem to be an overkill for this particular use case. The Tap Gesture Recognizer is not suitable in this particular case as tapping on the Entry does not trigger an a tap event.

What do you think?

*Credit*: This page was written using [StackEdit](https://stackedit.io/app)