using System;
using System.IO;
using System.Threading.Tasks;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using Xunit;

namespace Extra.Tests.StealthPluginTests.EvasionsTests;

public class StealthPluginTest : BrowserDefault
{
    private const string AntiBotDirectory = "AntiBot";

    public StealthPluginTest()
    {
        CreateAntiBotDirectory();
    }

    [Fact]
    public async Task Test_SannySoft()
    {
        var browser = await LaunchWithPluginAsync(new StealthPlugin());
        var page = await browser.NewPageAsync();
        await page.GoToAsync("https://bot.sannysoft.com");
        await page.ScreenshotAsync($"{AntiBotDirectory}/SannySoft.png",
            new ScreenshotOptions()
            {
                FullPage = true,
                Type = ScreenshotType.Png,
            });
    }

    [Fact]
    public async Task Test_Incolumitas()
    {
        var browser = await LaunchWithPluginAsync(new StealthPlugin());
        var page = await browser.NewPageAsync();
        await page.GoToAsync("https://bot.incolumitas.com/");
        await page.ScreenshotAsync($"{AntiBotDirectory}/Incolumitas.png",
            new ScreenshotOptions()
            {
                FullPage = true,
                Type = ScreenshotType.Png,
            });
    }

    [Fact]
    public async Task Test_InfoSimples()
    {
        var browser = await LaunchWithPluginAsync(new StealthPlugin());
        var page = await browser.NewPageAsync();

        page.Dialog += async (_, e) =>
        {
            // human simulate
            await Task.Delay(Random.Shared.Next(200, 500));

            await e.Dialog.Accept();
        };

        await page.GoToAsync("https://infosimples.github.io/detect-headless/");

        await page.ScreenshotAsync($"{AntiBotDirectory}/InfoSimples.png",
            new ScreenshotOptions()
            {
                FullPage = true,
                Type = ScreenshotType.Png,
            });
    }

    private static void CreateAntiBotDirectory()
    {
        if (Directory.Exists(AntiBotDirectory))
            return;

        Directory.CreateDirectory(AntiBotDirectory);
    }
}
