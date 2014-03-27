function bijectiv()
{
    // A cross platform hack to get the Linx font size the same as windows
    if (navigator.platform.indexOf("Linux")!=-1)
    {
        $(".highlight").css("font-size", "1.1em");
    }
}

