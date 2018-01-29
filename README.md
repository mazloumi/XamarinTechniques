
# Welcome to Xamarin Techniques!

This repository is a playground for exploring various ideas and learning more about Xamarin. It is work in progress and your feedback is much appreciated. The purpose is to provide an overview of various techniques and to evaluate how to best use them.

## Techniques in Xamarin
When learning about Xamarin you will come at some point across techniques such as;

 - Attached Properties
 - Behaviors
 - Effects
 - Event Handlers
 - Property Triggers
 - Renderer
 - Tap Gesture Recognizers
 - Bindable Properties [Todo]
 - Derived Classes [Todo]

This repo demonstrates the basic structure for each technique and provides a comparison in order to determine when to use best what technique. Checkout the repo. It provides the same example across all of the techniques to make yourself familiar with the differences. An exploration on when to use what technique is provided below, after each technique is introduced. Refer to the excellent Xamarin documentation for details.

### Attached Property

Attached properties are static classes with a special type of bindable properties that can be added to any XAML element using the notation `class.member`, ex.
```sh
<Element MyClass.MyMember="MyValue"/>
```
The use of attached properties is not limited to only controls, layouts and views but can be used on any C# class that is used in XAML. The implementation of `MyClass.MyMember` is ex.
```sh
    public class MyClass {
        public static readonly BindableProperty MyMemberProperty =
                BindableProperty.CreateAttached("MyMember", typeof(bool), typeof(MyClass), false, propertyChanged: MyMemberPropertyChanged);

        public static bool GetMyMember(BindableObject view) {
            return (bool)view.GetValue(CheckFocusProperty);
        }

        public static void SetMyMember(BindableObject view, bool value) {
            view.SetValue(CheckFocusProperty, value);
        }
        
        private static void MyMemberPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
            ....
        }        
```
The `CreateAttached` method is key. Register your property by a name, its type, a default value and a `propertyChanged` delegate that is called when the attribute is used.
### Behavior
One or more behaviors can be added to class that derives from `VisualElement`, ex.

You override the methods `OnAttachedTo` and `OnDetachingFrom` to tell the behavior what specifically it should do. You need to register an event handler to do anything useful because the methods are only called when a behavior is added or removed from the control.
```sh
<Entry>
    <Entry.Behaviors>
        <local:MyBehavior />
    </Entry.Behaviors>
</Entry>
```
Subclass Behavior<T> and override the two methods to implement a behavior, ex.
```sh
    public class FocusBehavior : Behavior<Entry> {
        protected override void OnAttachedTo(Entry entry) {
            ....
        }

        protected override void OnDetachingFrom(Entry entry) {
            ....
        }
    }
```
Other properties i.e. regular bindable properties will be discussed in future.
### Effect
Effects are similar to behaviors but have a broader application as they can be added classes that derive from `Element`. You create an effect by sub classing the `RoutingEffect` class in the shared project and the `PlatformEffect` class in each platform-specific project overriding the methods  `OnAttached`, `OnDetached` and possibly `OnElementPropertyChanged`. Use the effect is XAML like this:
```sh
<Entry>
    <Entry.Effects>
        <local:MyEffect />
    </Entry.Effects>
</Entry>
```
The platform-specific `PlatformEffect`s gives access to the native control and container used as well as the Xamarin.Forms view, which makes an effect more powerful than a behavior but requires knowledge of each platform.
The implementation of the shared `RoutingEffect` is ex.
```sh
    public class MyEffect : RoutingEffect {
        public MyEffect() : base("com.yourcompany.MyEffect") {}
    }
```
The effect does nothing else but resolves to the platform-specific effect, the basic structure is almost identical on each platform, ex.

```sh
[assembly: ResolutionGroupName("com.yourcompany")]
[assembly: ExportEffect(typeof(MyEffect), "MyEffect")]
namespace com.yourcompany {
    public class FocusEffect : PlatformEffect {
        protected override void OnAttached() {
            ....
        }

        protected override void OnDetached() {
            ....
        }
    }
}
```
The attributes `ResolutionGroupName` and `ExportEffect` are intended uniquely register the effect so that the `RoutingEffect` can resolve the platform-specific implementation.
### Event Handler
All controls in Xamarin.Forms have the one or the other event. You can use the event handler syntax to register an event hander you provide in the Code Behind, ex.
```sh
<Entry Focused="Entry_Focused" />
```
The implementation of the handler `Entry_Focused` is in the Code Behind of the XAML page, ex.
```sh
    void Entry_Focused(object sender, Xamarin.Forms.FocusEventArgs e) {
    ....
    }
```
### Property Trigger
The property trigger is applied to a target type and observes whether one of the properties matches a specific value. Triggers are available on any class deriving from `VisualElement` and to the `Style` class. If the condition is true one or more setters are applied that can set values for other properties. Use the property trigger like this:
```sh
<Entry>
    <Entry.Triggers>
        <Trigger Property="IsFocused" Value="True">
            <Setter Property="BackgroundColor" Value="Yellow"/>
        </Trigger>
    </Entry.Triggers>
</Entry>
```
No C# code is needed. There are other type of triggers `DataTrigger`, `EventTrigger` and `MultiTrigger`, which we will discuss in future.
### Renderer
A renderer take effects to the next level. Instead of deriving from routing effect and platform effect, which are not control-specific you derive from the renderer class provided by Xamarin.Forms to get full access even replacing the native control used by Xamarin, if you wish. To create your own renderer first derive from a control, ex.
```sh
<Entry xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" x:Class="com.yourcompany.MyEntry"/>
```
Then provide for each platform a subclass of the respective renderer, ex.
```sh
[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace com.yourcompany {
    public class MyEntryRenderer : EntryRenderer {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) {
            base.OnElementChanged(e);
            ....
        }
    }
}
```
The renderer is registered using the attribute `ExportRenderer`.
### Tap Gesture Recognizer
Tap Gesture Recognizer are available for all classes that derive from `View`. You can use them to wire them to an event handler or a `Command` in your view model (TODO), ex.
```sh
<Entry>
    <Entry.GestureRecognizers>
        <TapGestureRecognizer Tapped="Entry_Focused" NumberOfTapsRequired="1" />
    </Entry.GestureRecognizers>
</Entry>
```
The implementation of the handler `Entry_Focused` is in the Code Behind of the XAML page, ex.
```sh
    void Entry_Focused(object sender, System.EventArgs e) {
        ....
    }
```
Gesture Recognizers can be considered for scenarios where a particular event is not available or to avoid using the Code Behind and instead to let the view model handle the event.
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

|Technique|What is it?|Location|Mainly|XAML Syntax|Thoughts|Lines of Code in C#|Available|
|-|-|-|-|-|-|-|-|
|**Attached Property**|Public Class with public static readonly`BindableProperty`, Getter and Setter and possibly`PropertyChanged` delegate |Shared project|C#|`<Entry local:MyClass.MyMember="Some Value"/>`|Useful for extending View including wrapping other techniques listed here. Brevity of XAML.|39|Any XAML element|
|**Behavior**|Sub class of `Behavior<T>` with overrides for `OnAttachedTo` and `OnDetachingFrom`|Shared project|C#|`<Entry><Entry.Behaviors><local:MyBehavior /></Entry.Behaviors></Entry>`|Useful for extending the View|23|`VisualElement.Behaviors`|
|**Effect**|Sub class of `RoutingEffect` and platform-specific sub classes of `PlatformEffect` with overrides for `OnAttached`, `OnDetached` and possibly `OnElementPropertyChanged`|Shared project and platform-specific projects|C#|`<Entry><Entry.Effects><local:MyEffect /></Entry.Effects></Entry>`|Access to native control, events and properties|67|`Element.Effects`|
|**Event Handler**|View with a delegate for an event|Shared project Code Behind|C#|`<Entry Focused="Entry_Focused" />`|Usage limited to specific view used in the XAML|5|All Events available for a view|
|**Property Trigger**|`Trigger` for specific `Property` and `Value` of a `TargetType` and `Setter` elements to change specific properties of the target type|Shared project|XAML|`<Entry><Entry.Triggers><Trigger Property="IsFocused" Value="True"><Setter Property="BackgroundColor" Value="Yellow"/></Trigger></Entry.Triggers></Entry>`|Scope limited but effective.|0|`VisualElement.Triggers` and `Style.Triggers`|
|**Renderer**|Optional sub class of a visual element and platform-specific sub classes of the element's renderer class with override of `OnElementChanged` method evaluating `OldElement` and `NewElement` member of `ElementChangedEventArgs<T>`|Platform-specific projects|C#|`<local:MyControl/>`|Full access to the native control used even allowing it to be replaced|72|40 renderer base classes|
|**Tap Gesture Recognizer**|`TapGestureRecognizer` with `Tapped` delegate triggered after `NumberOfTapsRequired`|Shared project|XAML|`<Entry><Entry.GestureRecognizers><TapGestureRecognizer Tapped="Entry_Focused" NumberOfTapsRequired="1" /></Entry.GestureRecognizers></Entry>`|Listen to user interaction and extend behavior. Event handler can be in the Code Behind or a command in the view model.|5|`View.GestureRecognizers`|

### Comparison
The table evaluates the extent each technique contributes to a particular evaluation criteria. The perceived winner is highlighted bold.

|Technique|Availability|XAML Only|Minimum LoC|Shared Project Only|Brevity|Reuse|
|-|-|-|-|-|-|-|
|Attached Property|**100%+**|No|Medium|Yes|**1 Line**|**Highest**|
|Behavior|86%|No|Medium|**Yes**|3 Lines|Medium|
|Effect|100%|No|High|No|3 Lines|High|
|Event|100%|No|Low|**Yes**|**1 Line**|Lowest|
|Property Trigger|86%|**Yes**|**0**|**Yes**|4 Lines|High|
|Renderer|100%|No|High|No|**1 Line**|Low|
|Tap Gesture Recognizer|72%|No|Low|**Yes**|3 Lines|Medium-Low|

### Conclusion
For the specific use case property trigger seems to be the most suitable approach. With 4 lines of XAML the problem is solved. The approach is not available though for all controls. What makes property triggers also attractive is that they can be defined in styles making them highly reusable. Next in line could be Attached Properties and Behaviors, which essentially are wrappers for Event handlers or even Tap Gesture Recognizers making them independent of the Code Behind. Effects and Renderer seem to be an overkill for this particular use case. The Tap Gesture Recognizer is not suitable in this particular case as tapping on the Entry does not trigger an a tap event.

What do you think?

*Credit*: This page was written using [StackEdit](https://stackedit.io/app) and [Markdown Editor](https://jbt.github.io/markdown-editor/)