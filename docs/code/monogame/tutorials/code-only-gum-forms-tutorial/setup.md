# Setup

## Introduction

This tutorial walks you through creating an empty Gum project which acts as a starting point for the rest of the tutorials.&#x20;

This tutorial covers:

* Adding Gum NuGet packages
* Modifying your Game class to support Gum and Gum Forms
* Adding your first Gum control (Button)

## Adding Gum NuGet Packages

Before writing any code, we must add the Gum nuget package. Add the Gum.MonoGame package to your game. For more information see the [Setup page](https://docs.flatredball.com/gum/code/monogame/setup).

Once you are finished, your game project should reference the `Gum.MonoGame` project.

<figure><img src="../../../../.gitbook/assets/NuGetGum.png" alt=""><figcaption><p>Gum.MonoGame NuGet package</p></figcaption></figure>

## Adding Gum to Your Game

Gum requires a few lines of code to get started. A simplified Game class with the required calls would look like the following code:

```csharp
using MonoGameGum.Forms.Controls;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    
    StackPanel mainPanel;
    GumService Gum => GumService.Default;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Gum.Initialize(this);
            
        mainPanel = new StackPanel();
        mainPanel.Visual.AddToRoot();
        
        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        Gum.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Gum.Draw();
        base.Draw(gameTime);
    }
}
```

The code above includes the following sections:

* StackPanel mainPanel - Games using Gum usually have an object which contains all other instances. In this case we create a StackPanel which will hold all of our controls.&#x20;

```csharp
StackPanel mainPanel;
```

* Initialize - The Initialize method prepares Gum for use. It must be called one time for every Gum project. Once Gum is initialized, we can create controls such as the StackPanel.  By calling AddToRoot, the maniPanel will be drawn and will receive input. All items added to the StackPanel will also be drawn and receive input, so we only need to call AddToRoot to the StackPanel.

```csharp
Gum.Initialize(this);
            
mainPanel = new StackPanel();
mainPanel.Visual.AddToRoot();
```

* Update - this updates the internal keyboard, mouse, and gamepad instances and applies default behavior to any components which implement Forms. For example, if a Button is added to the Screen, this code is responsible for checking if the cursor is overlapping the Button and adjusting the highlight/pressed state appropriately.

```csharp
Gum.Update(gameTime);
```

* Draw - this method draws all Gum objects to the screen. This method does not yet perform any drawing since StackPanels are invisible, but we'll be adding controls later in this tutorial.

```csharp
Gum.Draw();
```

We can run our project to see a blank (cornflower blue) screen.

<figure><img src="../../../../.gitbook/assets/image (2) (1).png" alt=""><figcaption><p>Empty MonoGame project</p></figcaption></figure>

### Adding Controls

Now that we have Gum running, we can add controls to our StackPanel (Root). The following code in Initialize adds a button which responds to being clicked by modifying its Text property:

```csharp
protected override void Initialize()
{
    Gum.Initialize(this);

    mainPanel = new StackPanel();
    mainPanel.Visual.AddToRoot();

    // Creates a button instance
    var button = new Button();
    // Adds the button as a child so that it is drawn and has its
    // events raised
    mainPanel.AddChild(button);
    // Initial button text before being clicked
    button.Text = "Click Me";
    // Makes the button wider so the text fits
    button.Visual.Width = 350;
    // Click event can be handled with a lambda
    button.Click += (_, _) =>
        button.Text = $"Clicked at {System.DateTime.Now}";

    base.Initialize();
}
```

<figure><img src="../../../../.gitbook/assets/13_07 53 14.gif" alt=""><figcaption></figcaption></figure>
