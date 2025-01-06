using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Extra.Tests.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins;
using PuppeteerSharp;
using PuppeteerSharp.BrowserData;

namespace Extra.Tests;

public abstract class BrowserDefault : IDisposable
{
    private static readonly SemaphoreSlim InstallSemaphore = new(1, 1);
    private static IConfiguration? Configuration;
    private readonly List<IBrowser> _launchedBrowsers = [];

    protected async Task<IBrowser> LaunchAsync(LaunchOptions? options = null)
    {
        options ??= await CreateDefaultOptionsAsync();

        var browser = await Puppeteer.LaunchAsync(options);
        _launchedBrowsers.Add(browser);
        return browser;
    }

    protected async Task<IBrowser> LaunchWithPluginAsync(
        PuppeteerExtraPlugin plugin,
        LaunchOptions? options = null)
    {
        var extra = new PuppeteerExtra().Use(plugin);
        options ??= await CreateDefaultOptionsAsync();

        var browser = await extra.LaunchAsync(options);
        _launchedBrowsers.Add(browser);
        return browser;
    }

    protected async Task<IBrowser> LaunchWithPluginsAsync(
        LaunchOptions? options = null,
        params PuppeteerExtraPlugin[] plugins)
    {
        var extra = new PuppeteerExtra();

        foreach (var plugin in plugins)
        {
            extra.Use(plugin);
        }
        
        options ??= await CreateDefaultOptionsAsync();

        var browser = await extra.LaunchAsync(options);
        _launchedBrowsers.Add(browser);
        return browser;
    }

    protected async Task<IPage> LaunchAndGetPage(
        PuppeteerExtraPlugin? plugin = null,
        LaunchOptions? options = null)
    {
        IBrowser? browser;

        if (plugin != null)
            browser = await LaunchWithPluginAsync(plugin, options);
        else
            browser = await LaunchAsync(options);

        var page = (await browser.PagesAsync())[0];

        return page;
    }

    protected async Task<LaunchOptions> CreateDefaultOptionsAsync()
    {
        var options = GetOptions();

        const SupportedBrowser browser = SupportedBrowser.Chrome;
        string path;

        if (options.ShouldBeInstall)
        {
            var fetcher = new BrowserFetcher
            {
                Browser = browser,
                CacheDir = options.CacheDirectory
            };

            var browsersQuery = fetcher.GetInstalledBrowsers();

            browsersQuery = browsersQuery.Where(e => e.Browser == browser);

            if (!string.IsNullOrEmpty(options.BuildId))
            {
                browsersQuery = browsersQuery.Where(e => e.BuildId == options.BuildId);
            }

            var infoBrowser = browsersQuery.FirstOrDefault();

            if (infoBrowser == null)
            {
                await InstallSemaphore.WaitAsync();
                try
                {
                    if (infoBrowser == null)
                    {
                        if (string.IsNullOrEmpty(options.BuildId))
                        {
                            infoBrowser = await fetcher.DownloadAsync(BrowserTag.Latest);
                        }
                        else
                        {
                            infoBrowser = await fetcher.DownloadAsync(options.BuildId);
                        }
                    }
                }
                finally
                {
                    InstallSemaphore.Release();
                }
            }

            path = infoBrowser.GetExecutablePath();
        }
        else
        {
            path = options.PathToChrome ?? throw new ArgumentException("Path to Chrome is not set");
        }

        var launchOptions = new LaunchOptions
        {
            Browser = browser,
            ExecutablePath = path,
            Headless = options.Headless,
        };

        return launchOptions;
    }

    private static ConfigurationOptions GetOptions()
    {
        Configuration ??= new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        var opt = Configuration.Get<ConfigurationOptions>() ??
                  throw new InvalidOperationException("Configuration is empty");

        return opt;
    }

    public void Dispose()
    {
        foreach (var launchedBrowser in _launchedBrowsers)
        {
            launchedBrowser.CloseAsync().Wait();
        }
    }
}
