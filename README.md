
# Welcome to Xamarin Techniques!

This repository is a playground for exploring various ideas and learning more about Xamarin. It is work in progress and your feedback is much appreciated. The purpose is to provide an overview of various techniques and to evaluate how to best use them.

## Techniques in Xamarin
When learning about Xamarin you will come at some point across techniques such as;

 - Attached Properties
 - Behaviors
 - Effects
 - Event Handlers
 - Triggers
	 - Property Triggers
	 - Data Triggers
	 - Event Triggers
 - Implicit Styles (in conjunction with Triggers)
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
No C# code is needed.

### Data Trigger
The data trigger is applied to a target type and observes the value of a property, referred to as `Path`, using data binding. Triggers are available on any class deriving from `VisualElement` and to the `Style` class. If the condition is true one or more setters are applied that can set values for other properties. The `Binding` property requires a `Source`, which is set in this use case to the named `entry`. Use the data trigger like this:
```sh
<Entry x:Name="entry">
    <Entry.Triggers>
        <DataTrigger TargetType="Entry" Binding="{Binding Source={x:Reference entry},Path=IsFocused}" Value="true">
            <Setter Property="BackgroundColor" Value="Yellow"/>
        </DataTrigger>
    </Entry.Triggers>
</Entry>
```
No C# code is needed.

### Event Trigger
The event trigger observes whether an event occurs and registers an action that is executed. Actions derive from the `TriggerAction<T>`. Triggers are available on any class deriving from `VisualElement` and to the `Style` class. Use the event trigger like this:
```sh
<Entry>
    <Entry.Triggers>
        <EventTrigger Event="SomeEvent">
            <local:MyTriggerAction />
        </EventTrigger>
</Entry>
```
The action is implemented in C# but sub classing `TriggerAction<T>`, ex.
```sh
public class MyTriggerAction : TriggerAction<Entry> {
    protected override void Invoke(Entry entry) {
        ....
    }
}
```

### Implicit Styles (in conjunction with Triggers)
All classes that derive from `VisualElement` can have a dictionary of resources. Styles are special type of resources that target a particular type. If the `Style` has a `Key` it must be assigned explicitly to a control, without a `Key` it is assigned to all instances of the targeted type. A Style can be defined as a local resource to a control, at the page level or globally. A Style cannot observe changes to properties. Combine it with triggers to support the use case. Use an implicit `Style` at the `Application` level like this:
```sh
<Application xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" x:Class="mynamespace.App">
    <Application.Resources>
        <Style TargetType="Entry">
            <Style.Triggers>
                <Trigger TargetType="Entry" Property="IsFocused" Value="True">
                    <Setter Property="BackgroundColor" Value="Yellow" />
                </Trigger>
            </Style.Triggers>
        </Style>
    </Application.Resources>
</Application>
```
No C# code is needed.

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

One question relates to better understand when to use what technique. This section compares various techniques applied to a simple use case 
### Scenario used for exploration
The background color of a view changes each the control gains focus.

### Evaluation Criteria
Possible evaluation criteria could be:
 1. **Availability of technique**. How many controls the technique can be applied to.
 2. **XAML-only**. The extent the solution requires C# code or can be purely implemented in XAML
 3. **C# Lines of Code**. The number of lines of code needed to solve the problem
 4. **Shared project only**. The extent the solution can be implemented in the Forms project or platform-specific projects
 5. **Brevity of XAML**.  How readable and brief the XAML code is
 6. **Reusability of approach**.  Can a functionality can be written once and used across all similar controls

### Comparison
The table evaluates the extent each technique contributes to a particular evaluation criteria. The perceived winner is highlighted bold.

|Technique|Availability|XAML Only|C# LoC|Shared Project Only|Brevity|Reuse|Comments
|-|-|-|-|-|-|-|-|
|**Attached Property**|**100%+**|No|Medium (39)|Yes|**1 Line**|**Highest**|Any XAML element, Extending View, Wrapping other techniques
|**Behavior**|86%|No|Medium (23)|**Yes**|3 Lines|Medium|`VisualElement.Behaviors`, Extending the View
|**Effect**|100%|No|High (67)|No|3 Lines|High|`Element.Effects`, Access to native control, events and properties
|**Event**|100%|No|Low (5)|**Yes**|**1 Line**|Lowest|Events available for a view, Limited to specific view used in the XAML
|**Property Trigger**|86%|**Yes**|**0**|**Yes**|8 Lines|High|`VisualElement.Triggers` and `Style.Triggers`, UI centric
|**Data Trigger**|86%|**Yes**|**0**|**Yes**|8 Lines|High|`VisualElement.Triggers` and `Style.Triggers`, UI centric
|**Event Trigger**|86%|No|Low (12)|**Yes**|8 Lines|High|`VisualElement.Triggers` and `Style.Triggers`, Extending View and reuse of event handler
|**Implicit Style**|86%|**Yes**|**0**|**Yes**|8 Lines|High|`VisualElement.Resources`, UI Centric, gobale reuse
|**Renderer**|100%|No|High (72)|No|**1 Line**|Low|40 renderer base classes, Full access to the native control used even allowing it to be replaced
|**Tap Gesture Recognizer**|72%|No|Low (5)|**Yes**|3 Lines|Medium-Low|`View.GestureRecognizers`, Listen to user interaction and extend behavior. Event handler can be in the Code Behind or a command in the view model

### Conclusion
For the specific use case different options can be considered depending on the principle applied:

 - Simple Event handler in C# is easy to understand and very little effort but not very reusable
 - Property and data triggers inside Style offer maximum reuse with only XAML. Triggers are however not 100% available.
 - Event Trigger or Attached Property as wrapper of a Behaviors inside Attached Property , both inside a Style offer maximum reuse with some C#.
- Effects and Renderer seem to be an overkill for this particular use case. 
-  The Tap Gesture Recognizer is not suitable in this particular case as tapping on the Entry does not trigger an a tap event.

What do you think?

*Credit*: This page was written using [StackEdit](https://stackedit.io/app) and [Markdown Editor](https://jbt.github.io/markdown-editor/)