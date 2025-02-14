﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace PuppeteerExtraSharp.Plugins;

public abstract class PuppeteerExtraPlugin
{
    protected PuppeteerExtraPlugin(string pluginName)
    {
        Name = pluginName;
    }

    public string Name { get; }

    public virtual IReadOnlyCollection<PluginRequirements> Requirements { get; } = [];

    public virtual ICollection<PuppeteerExtraPlugin> GetDependencies() => [];

    public virtual void BeforeLaunch(LaunchOptions options)
    {
    }

    public virtual void AfterLaunch(IBrowser browser)
    {
    }

    public virtual void BeforeConnect(ConnectOptions options)
    {
    }

    public virtual void AfterConnect(IBrowser browser)
    {
    }

    public virtual void OnBrowser(IBrowser browser)
    {
    }

    public virtual void OnTargetCreated(Target target)
    {
    }

    public virtual Task OnPageCreatedAsync(IPage page) => Task.CompletedTask;

    public virtual Task OnTargetChangedAsync(Target target) => Task.CompletedTask;

    public virtual Task OnTargetDestroyedAsync(Target target) => Task.CompletedTask;

    public virtual void OnDisconnected()
    {
    }

    public virtual void OnClose()
    {
    }

    public virtual void OnPluginRegistered()
    {
    }
}
