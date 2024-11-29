using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading;
using Xamarin.Forms;

namespace VoiceAnimationXamarin
{
    public class DynamicBarAnimationView : SKCanvasView
    {
        private float _bar1Height = 20;
        private float _bar2Height = 40;
        private float _bar3Height = 20;
        private float _bar4Height = 40;

        private bool _toggle = false; // Used to alternate between height states
        private bool _isAnimating = false; // Tracks animation state
        private CancellationTokenSource _animationCancellationToken;

        // Bindable Property for Animation Speed
        public static readonly BindableProperty AnimationSpeedProperty =
            BindableProperty.Create(
                nameof(AnimationSpeed),
                typeof(double),
                typeof(DynamicBarAnimationView),
                300.0);

        public double AnimationSpeed
        {
            get => (double)GetValue(AnimationSpeedProperty);
            set => SetValue(AnimationSpeedProperty, value);
        }

        // Bindable Property for Corner Radius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(float),
                typeof(DynamicBarAnimationView),
                8.0f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // Bindable Property for Spacing Factor
        public static readonly BindableProperty SpacingFactorProperty =
            BindableProperty.Create(
                nameof(SpacingFactor),
                typeof(double),
                typeof(DynamicBarAnimationView),
                1.5);

        public double SpacingFactor
        {
            get => (double)GetValue(SpacingFactorProperty);
            set => SetValue(SpacingFactorProperty, value);
        }

        // Bindable Property for MaxHeight
        public static readonly BindableProperty MaxHeightProperty =
            BindableProperty.Create(
                nameof(MaxHeight),
                typeof(float),
                typeof(DynamicBarAnimationView),
                40.0f);

        public float MaxHeight
        {
            get => (float)GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }

        // Bindable Property for MinHeight
        public static readonly BindableProperty MinHeightProperty =
            BindableProperty.Create(
                nameof(MinHeight),
                typeof(float),
                typeof(DynamicBarAnimationView),
                20.0f);

        public float MinHeight
        {
            get => (float)GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }

        // Bindable Property for Bar Color
        public static readonly BindableProperty BarColorProperty =
            BindableProperty.Create(
                nameof(BarColor),
                typeof(Color),
                typeof(DynamicBarAnimationView),
                Color.Blue,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((DynamicBarAnimationView)bindable).InvalidateSurface();
                });

        public Color BarColor
        {
            get => (Color)GetValue(BarColorProperty);
            set => SetValue(BarColorProperty, value);
        }

        public DynamicBarAnimationView()
        {
            PaintSurface += OnPaintSurface; // Subscribe to draw event
        }

        public void StartAnimation()
        {
            if (_isAnimating) return;

            _isAnimating = true;
            _animationCancellationToken = new CancellationTokenSource();

            Device.StartTimer(TimeSpan.FromMilliseconds(AnimationSpeed), () =>
            {
                if (_animationCancellationToken.IsCancellationRequested)
                {
                    _isAnimating = false;
                    return false; // Stop timer
                }

                AnimateBars();
                InvalidateSurface(); // Redraw the canvas
                return true; // Continue timer
            });
        }

        public void StopAnimation()
        {
            _animationCancellationToken?.Cancel();
            _isAnimating = false;
        }

        private void AnimateBars()
        {
            if (_toggle)
            {
                _bar1Height = MaxHeight;
                _bar2Height = MinHeight;
                _bar3Height = MaxHeight;
                _bar4Height = MinHeight;
            }
            else
            {
                _bar1Height = MinHeight;
                _bar2Height = MaxHeight;
                _bar3Height = MinHeight;
                _bar4Height = MaxHeight;
            }

            _toggle = !_toggle;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;

            // Clear the canvas
            canvas.Clear(SKColors.Transparent);

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = BarColor.ToSKColor(),
                IsAntialias = true
            })
            {
                // Calculate bar dimensions and spacing
                float barWidth = (float)(info.Width / (8 * SpacingFactor));
                float spacing = barWidth;
                float centerY = info.Height / 2;

                // Draw bar 1
                DrawBar(canvas, paint, spacing, centerY, barWidth, _bar1Height);

                // Draw bar 2
                DrawBar(canvas, paint, spacing * 3, centerY, barWidth, _bar2Height);

                // Draw bar 3
                DrawBar(canvas, paint, spacing * 5, centerY, barWidth, _bar3Height);

                // Draw bar 4
                DrawBar(canvas, paint, spacing * 7, centerY, barWidth, _bar4Height);
            }
        }

        private void DrawBar(SKCanvas canvas, SKPaint paint, float x, float centerY, float width, float height)
        {
            var rect = new SKRect(x, centerY - height / 2, x + width, centerY + height / 2);
            canvas.DrawRoundRect(rect, CornerRadius, CornerRadius, paint);
        }
    }
}
