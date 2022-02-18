using UnityEngine;

namespace RishUI.Components
{
    public class Grid : RishComponent<GridProps>
    {
        protected override bool RenderOnResize => true;

        protected override RishElement Render()
        {
            if (Props.children.Count == 0)
            {
                return RishElement.Null;
            }

            var count = Props.children.Count;

            if (count == 0)
            {
                return RishElement.Null;
            }

            var height = Size.y - Props.padding.top - Props.padding.bottom;
            var width = Size.x - Props.padding.left - Props.padding.right;

            if (height <= 0 || width <= 0)
            {
                return RishElement.Null;
            }

            var rows = 0;
            var columns = 0;
            var elementWidth = 0f;
            var elementHeight = 0f;
            if (Props.elementSize.x <= 0 && Props.elementSize.y <= 0)
            {
                var closest = float.MaxValue;
                for (var r = 1; r <= count; r++)
                {
                    var h = (height - Props.spacing.y * (r - 1)) / r;
                    var c = Mathf.CeilToInt(count / (float) r);
                    var w = (width - Props.spacing.x * (c - 1)) / c;

                    var diff = Mathf.Abs(1 - w / h);

                    if (diff >= closest)
                    {
                        break;
                    }

                    closest = diff;

                    rows = r;
                    columns = c;
                    elementWidth = w;
                    elementHeight = h;
                }
            }
            else
                switch (Props.overflow)
                {
                    case GridOverflow.None when Props.elementSize.y > 0 && Props.elementSize.x <= 0:
                    {
                        elementHeight = Props.elementSize.y;

                        rows = (int) ((height + Props.spacing.y) / (elementHeight + Props.spacing.y));
                        if (rows <= 0)
                        {
                            return RishElement.Null;
                        }

                        columns = Mathf.CeilToInt(count / (float) rows);

                        elementWidth = (width - Props.spacing.x * (columns - 1)) / columns;
                        break;
                    }
                    case GridOverflow.None when Props.elementSize.x > 0 && Props.elementSize.y <= 0:
                    {
                        elementWidth = Props.elementSize.x;

                        columns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));
                        if (columns <= 0)
                        {
                            return RishElement.Null;
                        }

                        rows = Mathf.CeilToInt(count / (float) columns);

                        elementHeight = (height - Props.spacing.y * (rows - 1)) / rows;
                        break;
                    }
                    case GridOverflow.None:
                    {
                        elementWidth = Props.elementSize.x;
                        elementHeight = Props.elementSize.y;

                        rows = (int) ((height + Props.spacing.y) / (elementHeight + Props.spacing.y));
                        columns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));

                        break;
                    }
                    case GridOverflow.Vertical when Props.elementSize.y > 0 && Props.elementSize.x <= 0:
                    {
                        elementHeight = Props.elementSize.y;

                        var closest = float.MaxValue;
                        for (var c = 1; c <= count; c++)
                        {
                            var w = (width - Props.spacing.x * (c - 1)) / c;

                            var diff = Mathf.Abs(w - elementHeight);

                            if (diff >= closest)
                            {
                                break;
                            }

                            closest = diff;

                            columns = c;
                            elementWidth = w;
                        }

                        if (columns <= 0)
                        {
                            return RishElement.Null;
                        }

                        rows = Mathf.CeilToInt(count / (float) columns);

                        break;
                    }
                    case GridOverflow.Vertical when Props.elementSize.x > 0 && Props.elementSize.y <= 0:
                    {
                        elementWidth = Props.elementSize.x;
                        elementHeight = elementWidth;

                        columns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));
                        if (columns <= 0)
                        {
                            return RishElement.Null;
                        }

                        rows = Mathf.CeilToInt(count / (float) columns);

                        var h = (height - Props.spacing.y * (rows - 1)) / rows;
                        if (h > elementHeight)
                        {
                            elementHeight = h;
                        }

                        break;
                    }
                    case GridOverflow.Vertical:
                    {
                        elementWidth = Props.elementSize.x;
                        elementHeight = Props.elementSize.y;

                        columns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));
                        if (columns <= 0)
                        {
                            return RishElement.Null;
                        }

                        rows = Mathf.CeilToInt(count / (float) columns);

                        break;
                    }
                    case GridOverflow.Horizontal when Props.elementSize.y > 0 && Props.elementSize.x <= 0:
                    {
                        elementHeight = Props.elementSize.y;
                        elementWidth = elementHeight;

                        var maxRows = (int) ((height + Props.spacing.y) / (elementHeight + Props.spacing.y));
                        var minColumns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));

                        for (var c = count; c >= minColumns; c--)
                        {
                            var r = Mathf.CeilToInt(count / (float) c);

                            if (r > maxRows)
                            {
                                break;
                            }

                            rows = r;
                            columns = c;
                        }

                        var w = (width - Props.spacing.x * (columns - 1)) / columns;
                        if (w > elementWidth)
                        {
                            elementWidth = w;
                        }

                        break;
                    }
                    case GridOverflow.Horizontal when Props.elementSize.x > 0 && Props.elementSize.y <= 0:
                    {
                        elementWidth = Props.elementSize.x;
                        elementHeight = elementWidth;

                        var maxRows = (int) ((height + Props.spacing.y) / (elementHeight + Props.spacing.y));
                        var minColumns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));

                        for (var r = maxRows; r >= 1; r--)
                        {
                            var c = Mathf.CeilToInt(count / (float) r);

                            if (c < minColumns) continue;

                            rows = r;
                            columns = c;
                            break;
                        }

                        var h = (height - Props.spacing.y * (rows - 1)) / rows;
                        if (h > elementHeight)
                        {
                            elementHeight = h;
                        }

                        break;
                    }
                    case GridOverflow.Horizontal:
                    {
                        elementWidth = Props.elementSize.x;
                        elementHeight = Props.elementSize.y;

                        var maxRows = (int) ((height + Props.spacing.y) / (elementHeight + Props.spacing.y));
                        var minColumns = (int) ((width + Props.spacing.x) / (elementWidth + Props.spacing.x));

                        for (var c = count; c >= minColumns; c--)
                        {
                            var r = Mathf.CeilToInt(count / (float) c);

                            if (r > maxRows)
                            {
                                break;
                            }

                            rows = r;
                            columns = c;
                        }

                        break;
                    }
                }

            if (columns <= 0 || rows <= 0 || elementWidth <= 0 || elementHeight <= 0 ||
                Mathf.Approximately(elementWidth, 0) || Mathf.Approximately(elementHeight, 0))
            {
                return RishElement.Null;
            }

            if (rows * columns > count)
            {
                rows = Mathf.CeilToInt(count / (float) columns);
            }

            var rowElements = new LayoutElement[rows];
            for (var i = 0; i < rows; i++)
            {
                var elements = new LayoutElement[columns];
                for (var j = 0; j < columns; j++)
                {
                    var index = columns * i + j;
                    elements[j] = index < count ? Props.children[index] : RishElement.Null;
                }

                rowElements[i] = Rish.Create<DirectionalLayout, DirectionalLayoutProps>(new DirectionalLayoutProps(Direction.LeftRight)
                {
                    spacing = Props.spacing.x,
                    elementSize = elementWidth,
                    overflow = true,
                    center = Props.centerHorizontal,
                    children = elements
                });
            }

            return Rish.Create<Div, DivProps>(new ExpandTransform(Props.padding), new DivProps
            {
                children = Rish.Create<DirectionalLayout, DirectionalLayoutProps>(new DirectionalLayoutProps
                {
                    spacing = Props.spacing.y,
                    elementSize = elementHeight,
                    overflow = true,
                    center = Props.centerVertical,
                    children = rowElements
                })
            });
        }
    }

    public struct GridProps
    {
        public Vector2 spacing;
        public Vector2 elementSize;
        
        public Margins padding;

        public GridOverflow overflow;
        public bool centerVertical;
        public bool centerHorizontal;

        public RishList<RishElement> children;

        [Default]
        public static GridProps Default => new GridProps
        {
            centerVertical = true
        };

        public GridProps(GridProps other)
        {
            spacing = other.spacing;
            elementSize = other.elementSize;
            padding = other.padding;
            overflow = other.overflow;
            centerVertical = other.centerVertical;
            centerHorizontal = other.centerHorizontal;
            children = other.children;
        }

        [Comparer]
        public static bool Equals(GridProps a, GridProps b) =>
            a.overflow == b.overflow && a.centerVertical == b.centerVertical && a.centerHorizontal == b.centerHorizontal &&
            Mathf.Approximately(a.spacing.x, b.spacing.x) && Mathf.Approximately(a.spacing.y, b.spacing.y) &&
            Mathf.Approximately(a.elementSize.x, b.elementSize.x) && Mathf.Approximately(a.elementSize.y, b.elementSize.y) &&
            RishUtils.CompareUnmanaged<Margins>(a.padding, b.padding) && RishUtils.Compare<RishList<RishElement>>(a.children, b.children);
    }
    
    public enum GridOverflow { None, Horizontal, Vertical }
}