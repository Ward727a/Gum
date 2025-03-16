﻿using GameUiSamples.Screens;
using Gum.Wireframe;
using GumRuntime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using RenderingLibrary;
using System;

namespace GameUiSamples;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreparingDeviceSettings += HandlePrepareDeviceSettings;
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 1366;
        _graphics.PreferredBackBufferHeight = 768;
        IsMouseVisible = true;
    }

    private void HandlePrepareDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
        e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
    }

    protected override void Initialize()
    {
        var gumProject = MonoGameGum.GumService.Default.Initialize(this, "GumProject/GameUiSamplesgumProject.gumx");

        var startScreen = new MainMenuRuntime();
        startScreen.AddToRoot();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        MonoGameGum.GumService.Default.Update(this, gameTime);

        foreach(var item in GumService.Default.Root.Children)
        {
            (item as IUpdateScreen)?.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        MonoGameGum.GumService.Default.Draw();

        base.Draw(gameTime);
    }
}
