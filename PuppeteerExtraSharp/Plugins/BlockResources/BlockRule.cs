using System.Collections.Generic;
using System.Text.RegularExpressions;
using PuppeteerSharp;

namespace PuppeteerExtraSharp.Plugins.BlockResources;

public class BlockRule
{
    public string? SitePattern { get; set; }

    public IPage? IPage { get; set; }

    public HashSet<ResourceType> ResourceType = [];

    public bool IsRequestBlocked(IPage fromPage, IRequest request)
    {
        if (!IsResourcesBlocked(request.ResourceType))
            return false;

        return IsSiteBlocked(request.Url) || IsPageBlocked(fromPage);
    }


    public bool IsPageBlocked(IPage page)
    {
        return IPage != null && page.Equals(IPage);
    }

    public bool IsSiteBlocked(string siteUrl)
    {
        return SitePattern != null &&
               !string.IsNullOrWhiteSpace(SitePattern) &&
               Regex.IsMatch(siteUrl, SitePattern);
    }

    public bool IsResourcesBlocked(ResourceType resource)
    {
        return ResourceType.Contains(resource);
    }
}
