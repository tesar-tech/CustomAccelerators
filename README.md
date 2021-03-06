# Custom Accelerators

Library and XAML control for keyboard ⌨ shortcuts customization.

Try it in [Video Detail Player](https://www.microsoft.com/en-us/p/video-detail-player/9p34ghb2h88r) or in the sample app.

<img src="vdp_changing_shortcuts.gif" width="500">

## Usage

(See the sample project in this repo for details)

- Add the package to your UWP project:

    [![CustomAccelerators](https://img.shields.io/nuget/v/CustomAccelerators.svg)](https://www.nuget.org/packages/CustomAccelerators/)
  > Install-Package CustomAccelerators

- Add reference in XAML:

    ``` xml
    <Page
    ...
    xmlns:ca="using:CustomAccelerators">
    ```

- Use attached property to add custom accelerator

    ``` xml
    <Button Content="Hello" ca:Extension.Identity="HelloCa"  />
     <!--this will also update the tooltip of the button-->
    ```

- Add definitions of accelerators somewhere (e.g. in `OnNavigatedTo` of the Page).

```csharp
var acceleratorsList = new List<(string identity, string label, VirtualKey key, VirtualKeyModifiers modifiers)>()
{
    ("HelloCa","I am Hello Button",VirtualKey.H,VirtualKeyModifiers.Control|VirtualKeyModifiers.Shift),
    ("Another Accelerator","",VirtualKey.PageDown,VirtualKeyModifiers.None)
};

AcceleratorsManager.AddDefaultsAndLoadFromStorage(acceleratorsList);
```

- Add `CustomAcceleratorsEditControl` somewhere in your xaml.

```xml
<ca:CustomAcceleratorsEditControl />
<!--this control allows to edit keybord shortcuts-->
```

## Notes

- You can also set accelerator without using attached properties:

   ``` xml
    <Button Content="Hello" >
        <Button.KeyboardAccelerators>
            <ca:CustomAccelerator  Identity="HelloCa"/>
        </Button.KeyboardAccelerators>
    </Button>
    <!--this will NOT automaticaly update the tooltip of the button-->
    ```

- You can enable/disable the accelerator through the attached property:

  ```xml
   <Button ca:Extension.Identity="SimpleAccelerator" ca:Extension.IsEnabled="True"/>
  ```
- Attached property has one great feature  - it changes tooltip of button (and appbarButton) when accelerator is changed (this behaviour is not supported with basic accelerators).
- Why is it neccessary to set accelerators somewhere in C# code?
  - Main reason is necessity to reach all accelerators in one place (for edit control). Accelerators from all pages, event those, that aren't loaded yet. 
- Why set the accelerators in `OnNavigatedTo` method?
  - It's not requisite. You can set this also in `OnLaunched` of `App.xaml.cs`, but note there are some IO operations (loading settings) that may delay the startup.
- It saves the shortcuts persistently (using local settings).
- You can't edit the appearance of `CustomAcceleratorsEditControl` (yet).
- Dont't set `Key`, `Modifiers` or `Label` in xaml. Only within definitions.
- It doesn't check if there are more action with same shortcut. In such case, just one action will be invoked.
- There are cases when you want to use two accelerator that invoke same action. No problem.
  - It's primary made for this special `CommandBar` [bug and its workarround](https://stackoverflow.com/questions/53735503/keyboard-accelerator-stops-working-in-uwp-app/62025749#62025749).
- What about localization?
  - You can do it this way:

  ```csharp
   var rl = ResourceLoader.GetForCurrentView();
   acceleratorsList = acceleratorsList.Select(x => { x.label = rl.GetString(x.identity);return x; }).ToList();
  ```

  - The [ResourceExtractor](https://github.com/tesar-tech/ResourceExtractor#custom-accelerators) is also prepared for this and will automatically modify the `.resw` with Identities and Labels.

- It's not perfect, I know 🙃 . If you have anything to say about it, don't be shy and open an issue. 