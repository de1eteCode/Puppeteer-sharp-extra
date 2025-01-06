# PuppeteerExtraSharp

Puppeteer extra sharp is a .NET port of the [Node.js library](https://github.com/berstend/puppeteer-extra/tree/master/packages/puppeteer-extra)*

## Requirements

- Only support dotnet 8, because in 'SourceUrl' plugin used [Harmony Thin](https://github.com/pardeike/Harmony) library
  to hide '\_\_puppeteer\_evaluation\_script\_\_'. Thanks for [commit](https://github.com/hardkoded/puppeteer-sharp/commit/e8a8133b22f2fe4c754cc8abf5cc6d506f5c9fcc) ( ｡ •̀ ᴖ •́ ｡)

## Quickstart

```c#
// Initialization plugin builder
var extra = new PuppeteerExtra(); 

// Use stealth plugin
extra.Use(new StealthPlugin());   

// Launch the puppeteer browser with plugins
var browser = await extra.LaunchAsync(new LaunchOptions()
{
    Headless = false
});

// Create a new page
var page = await browser.NewPageAsync();

await page.GoToAsync("http://google.com");

// Wait 2 second
await page.WaitForTimeoutAsync(2000);

// Take the screenshot
await page.ScreenshotAsync("extra.png");
```
## Plugin list

🏴 [Puppeteer stealth plugin](https://github.com/Overmiind/PuppeteerExtraSharp/tree/master/Plugins/ExtraStealth)
- Applies various evasion techniques to make detection of headless puppeteer harder.

📃 [Puppeteer anonymize UA plugin](https://github.com/Overmiind/PuppeteerExtraSharp/tree/master/Plugins/AnonymizeUa)
- Anonymizes the user-agent on all pages.


✋**More plugins will be soon**
## API

#### Use(IPuppeteerExtraPlugin)

Adds a new plugin to plugins list and register it.
- Returns the same instance of puppeteer extra
- Parameters: instance of IPuppeteerExtraPlugin interface
```c# 
var puppeteerExtra = new PuppeteerExtra().Use(new AnonymizeUaPlugin()).Use(new StealthPlugin());
```

#### LaunchAsync(LaunchOptions)

- Return the new puppeteer browser instance with launch options

```c#
var browser = new PuppeteerExtra().LaunchAsync(new LaunchOptions());
```

#### ConnectAsync(ConnectOptions)
- Connect to the exiting browser with connect options
```c#
var browser = new PuppeteerExtra().ConnectAsync(new ConnectOptions());
```

#### GetPlugin<T>()
- Get plugin from plugin list by type
```c# 
var stealthPlugin = puppeteerExtra.GetPlugin<StealthPlugin>();
```

<span style="font-size: 10px">*almost</span>