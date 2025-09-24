namespace Rhynia.Baseline;

public static class VirtualizedList
{
    public static void Draw<T>(
        Rect listRect,
        List<T> items,
        Action<Rect, T> drawRow,
        ref Vector2 scrollPosition,
        float rowHeight,
        float padding = 10f,
        float scrollBarWidth = 10f
    )
    {
        var totalHeight = items.Count * rowHeight;
        var scrollRect = new Rect(0f, listRect.y, listRect.width - scrollBarWidth, listRect.height);
        var viewRect = new Rect(0f, 0f, scrollRect.width - 16f, totalHeight);

        Widgets.BeginScrollView(scrollRect, ref scrollPosition, viewRect);

        var visibleTop = scrollPosition.y;
        var visibleBottom = scrollPosition.y + scrollRect.height;

        var firstVisibleIndex = Mathf.Max(0, Mathf.FloorToInt(visibleTop / rowHeight) - 1);
        var lastVisibleIndex = Mathf.Min(
            items.Count - 1,
            Mathf.CeilToInt(visibleBottom / rowHeight) + 1
        );

        for (var i = firstVisibleIndex; i <= lastVisibleIndex; i++)
        {
            var itemY = i * rowHeight;
            var itemRect = new Rect(padding, itemY, viewRect.width - 2 * padding, rowHeight);
            drawRow(itemRect, items[i]);
        }

        Widgets.EndScrollView();
    }
}

/// <summary>
/// Base class for tabs that configure a specific Thing properties.
/// </summary>
public abstract class ITab_ConfigureThing<T> : ITab
    where T : Thing
{
    protected new T? SelThing => base.SelThing as T;

    protected abstract string LabelKey { get; }

    protected virtual Vector2 WinSize { get; } = new(400f, 360f);

    public override bool IsVisible => SelThing is not null;

    public ITab_ConfigureThing()
    {
        size = WinSize;
        labelKey = LabelKey;
    }
}
