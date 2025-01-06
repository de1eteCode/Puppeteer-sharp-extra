using System;
using System.Net;
using System.Threading.Tasks;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using Xunit;

namespace Extra.Tests.StealthPluginTests;

public class CloudFlareDoubleVisitsTests : BrowserDefault
{
    [Fact]
    public async Task ShouldBeNotDetected()
    {
        const string url =
            "https://playerok.com/products/8b1f416bc5f0-42-ar4-legibez-privyazokpochta-pereprivyazka";

        var browser =
            await LaunchWithPluginsAsync(null, new StealthPlugin(), new AnonymizeUaPlugin());

        // ---

        var page1 = await browser.NewPageAsync();

        var resp1 = await page1.GoToAsync(url);
        var doc1 = await resp1.TextAsync();

        // ---

        var page2 = await browser.NewPageAsync();

        var resp2 = await page2.GoToAsync(url);
        var doc2 = await resp2.TextAsync();

        // ---

        Assert.Equal(HttpStatusCode.OK, resp1.Status);
        Assert.Equal(HttpStatusCode.OK, resp2.Status);
        Assert.NotEmpty(doc1);
        Assert.NotEmpty(doc2);
        Assert.True(string.Equals(doc1, doc2, StringComparison.InvariantCulture));
    }
}
