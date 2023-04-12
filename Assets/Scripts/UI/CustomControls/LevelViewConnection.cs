using ConnectIt.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectIt.UI.CustomControls
{
    public class LevelViewConnection : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<LevelViewConnection, UxmlTraits> { }

        public const string UssClassName = "select-level-container__connection-line";

        public const string StartColorCustomStyleName = "--start-color";
        public const string EndColorCustomStyleName = "--end-color";
        public const string LineWidthCustomStyleName = "--line-width";
        public const string LineOffsetCustomStyleName = "--line-offset";
        public const string UseViewsColorsCustomStyleName = "--use-views-colors";

        public bool Connected => _first != null && _second != null;

        private static CustomStyleProperty<Color> startColorStyle = new(StartColorCustomStyleName);
        private static CustomStyleProperty<Color> endColorStyle = new(EndColorCustomStyleName);
        private static CustomStyleProperty<float> lineWidthStyle = new(LineWidthCustomStyleName);
        private static CustomStyleProperty<float> lineOffsetStyle = new(LineOffsetCustomStyleName);
        private static CustomStyleProperty<bool> useViewsColorsStyle = new(UseViewsColorsCustomStyleName);

        private Color _startColor;
        private Color _endColor;
        private float _lineWidth;
        private float _lineOffset;
        private bool _useViewsColors;

        private LevelView _first;
        private LevelView _second;

        public LevelViewConnection()
        {
            AddToClassList(UssClassName);
            pickingMode = PickingMode.Ignore;
        }

        public LevelViewConnection(LevelView first, LevelView second) : this()
        {
            Assert.ArgsIsNotNull(first, second);

            Connect(first, second);
        }

        public void Connect(LevelView first, LevelView second)
        {
            Assert.ArgsIsNotNull(first, second);

            if (Connected)
                Disconnect();

            _first = first;
            _second = second;

            SubscribeToCallbacks();
        }

        public void Disconnect()
        {
            Assert.That(Connected);

            UnsubscribeFromCallbacks();

            _first = null;
            _second = null;
        }

        private void SubscribeToCallbacks()
        {
            generateVisualContent += GenerateVisualContent;

            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStylesResolved);

            _first.RegisterCallback<GeometryChangedEvent>(OnLevelViewGeometryChanged);
            _second.RegisterCallback<GeometryChangedEvent>(OnLevelViewGeometryChanged);
        }

        private void UnsubscribeFromCallbacks()
        {
            generateVisualContent -= GenerateVisualContent;

            UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStylesResolved);

            _first.UnregisterCallback<GeometryChangedEvent>(OnLevelViewGeometryChanged);
            _second.UnregisterCallback<GeometryChangedEvent>(OnLevelViewGeometryChanged);
        }

        private void OnLevelViewGeometryChanged(GeometryChangedEvent geometryChangedEvent)
        {
            MarkDirtyRepaint();
        }

        private void OnCustomStylesResolved(CustomStyleResolvedEvent customStyleResolvedEvent)
        {
            bool repaint = false;
            if (customStyle.TryGetValue(startColorStyle, out _startColor))
                repaint = true;

            if (customStyle.TryGetValue(endColorStyle, out  _endColor))
                repaint = true;

            if (customStyle.TryGetValue(lineWidthStyle, out _lineWidth))
                repaint = true;

            if (customStyle.TryGetValue(lineOffsetStyle, out _lineOffset))
                repaint = true;

            if (customStyle.TryGetValue(useViewsColorsStyle, out _useViewsColors))
                repaint = true;

            if (repaint)
                MarkDirtyRepaint();
        }

        private void GenerateVisualContent(MeshGenerationContext context)
        {
            if (!Connected)
                return;

            Painter2D painter = context.painter2D;

            Color startColor = _useViewsColors ? _first.resolvedStyle.backgroundColor : _startColor;
            Color endColor = _useViewsColors ? _second.resolvedStyle.backgroundColor : _endColor;

            Gradient strokeGradient = new();
            strokeGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(startColor, 0),
                    new GradientColorKey(endColor, 1)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(startColor.a, 0),
                    new GradientAlphaKey(endColor.a, 1)
                });

            painter.strokeGradient = strokeGradient;
            painter.lineWidth = _lineWidth;
            painter.lineCap = LineCap.Round;

            Vector2 firstCenterPosition = _first.layout.position + _first.layout.size / 2;
            Vector2 secondCenterPosition = _second.layout.position + _second.layout.size / 2;

            Vector2 fromFirstToSecondDirectionUnit = (firstCenterPosition - secondCenterPosition).normalized;
            Vector2 firstPoint = firstCenterPosition - fromFirstToSecondDirectionUnit * (_lineOffset + _first.contentRect.size.x / 2);
            Vector2 secondPoint = secondCenterPosition + fromFirstToSecondDirectionUnit * (_lineOffset + _second.contentRect.size.x / 2);

            painter.BeginPath();
            painter.MoveTo(firstPoint);
            painter.LineTo(secondPoint);
            painter.Stroke();
        }
    }
}
