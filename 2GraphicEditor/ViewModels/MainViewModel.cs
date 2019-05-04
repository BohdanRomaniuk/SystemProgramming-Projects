using GraphicEditor.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace GraphicEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<object> Figures { get; set; }

        public ICommand DrawClickCommand { get; }
        public ICommand ChangeColorCommand { get; }
        public ICommand SelectFillColorCommand { get; }
        public ICommand ChangeModeCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseWindowCommand { get; }

        private Brush currentColor;
        private Brush fillColor;
        private string mode;
        private double thickness;
        private string text;
        Point currentPoint = new Point();

        public Brush CurrentColor
        {
            get => currentColor;
            set
            {
                currentColor = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }
        public Brush FillColor
        {
            get => fillColor;
            set
            {
                fillColor = value;
                OnPropertyChanged(nameof(FillColor));
            }
        }
        public string Mode
        {
            get => mode;
            set
            {
                mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }
        public double Thickness
        {
            get => thickness;
            set
            {
                thickness = value;
                OnPropertyChanged(nameof(Thickness));
            }
        }
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public MainViewModel()
        {
            Figures = new ObservableCollection<object>();
            Mode = "Pencil";
            CurrentColor = Brushes.Black;
            FillColor = Brushes.White;
            Thickness = 1;
            DrawClickCommand = new Command(DrawClick);
            ChangeColorCommand = new Command(ChangeColor);
            SelectFillColorCommand = new Command(SelectFillColor);
            ChangeModeCommand = new Command(ChangeMode);
            ClearCommand = new Command(Clear);
            UndoCommand = new Command(Undo);
            AboutCommand = new Command(About);
            SaveCommand = new Command(Save);
            CloseWindowCommand = new Command(CloseWindow);
        }

        private void DrawClick(object parameter)
        {
            Canvas plane = (parameter as Canvas);
            switch (Mode)
            {
                case "Pencil":
                    currentPoint = Mouse.GetPosition((IInputElement)parameter);
                    plane.MouseMove += new MouseEventHandler(PencilMouseMove);
                    plane.MouseUp += new MouseButtonEventHandler(PencilMouseUp);
                    break;
                case "Eraser":
                    currentPoint = Mouse.GetPosition((IInputElement)parameter);
                    plane.MouseMove += new MouseEventHandler(EraserMouseMove);
                    plane.MouseUp += new MouseButtonEventHandler(EraserMouseUp);
                    break;
                case "Line":
                    if (plane.CaptureMouse())
                    {
                        var startPoint1 = Mouse.GetPosition((IInputElement)parameter);
                        var line = new Line
                        {
                            Stroke = CurrentColor,
                            StrokeThickness = Thickness,
                            X1 = startPoint1.X,
                            Y1 = startPoint1.Y,
                            X2 = startPoint1.X,
                            Y2 = startPoint1.Y,
                        };
                        Figures.Add(line);
                    }
                    plane.MouseMove += new MouseEventHandler(LineMouseMove);
                    plane.MouseLeftButtonUp += new MouseButtonEventHandler(LineMouseLeftButtonUp);
                    break;
                case "Rectangle":
                    if (plane.CaptureMouse())
                    {
                        var startPoint1 = Mouse.GetPosition((IInputElement)parameter);
                        Rectangle rect = new Rectangle();
                        rect.Stroke = CurrentColor;
                        rect.Fill = FillColor;
                        rect.Width = startPoint1.X - startPoint1.X;
                        rect.Height = startPoint1.Y - startPoint1.Y;
                        Canvas.SetLeft(rect, startPoint1.X);
                        Canvas.SetTop(rect, startPoint1.Y);
                        Figures.Add(rect);
                    }
                    plane.MouseMove += new MouseEventHandler(RectangleMouseMove);
                    plane.MouseLeftButtonUp += new MouseButtonEventHandler(RectangleMouseLeftButtonUp);
                    break;
                case "Ellipse":
                    if (plane.CaptureMouse())
                    {
                        var startPoint1 = Mouse.GetPosition((IInputElement)parameter);
                        Ellipse ellipse = new Ellipse();
                        ellipse.Stroke = CurrentColor;
                        ellipse.Fill = FillColor;
                        ellipse.Width = startPoint1.X - startPoint1.X;
                        ellipse.Height = startPoint1.Y - startPoint1.Y;
                        Canvas.SetLeft(ellipse, startPoint1.X);
                        Canvas.SetTop(ellipse, startPoint1.Y);
                        Figures.Add(ellipse);
                    }
                    plane.MouseMove += new MouseEventHandler(EllipseMouseMove);
                    plane.MouseLeftButtonUp += new MouseButtonEventHandler(EllipseMouseLeftButtonUp);
                    break;
                case "Text":
                    currentPoint = Mouse.GetPosition((IInputElement)parameter);
                    TextBlock textBlock = new TextBlock();
                    textBlock.Foreground = CurrentColor;
                    textBlock.FontSize = Thickness;
                    textBlock.Text = Text;
                    textBlock.Foreground = CurrentColor;
                    Canvas.SetLeft(textBlock, currentPoint.X);
                    Canvas.SetTop(textBlock, currentPoint.Y);
                    Figures.Add(textBlock);
                    break;
                default:
                    currentPoint = Mouse.GetPosition((IInputElement)parameter);
                    plane.MouseMove += new MouseEventHandler(PencilMouseMove);
                    plane.MouseUp += new MouseButtonEventHandler(PencilMouseUp);
                    break;
            }

        }

        private void ChangeColor(object parameter)
        {
            var converter = new BrushConverter();
            CurrentColor = (Brush)converter.ConvertFromString((string)parameter);
        }

        private void SelectFillColor(object parameter)
        {
            var converter = new BrushConverter();
            FillColor = (Brush)converter.ConvertFromString((string)parameter);
        }

        private void ChangeMode(object parameter)
        {
            Mode = (string)parameter;
        }

        private void Clear(object parameter)
        {
            Figures.Clear();
        }

        private void Undo(object parameter)
        {
            if (Figures.Count > 0)
            {
                Figures.Remove(Figures.Last());
            }
        }

        private void Save(object parameter)
        {
            var rtb = new RenderTargetBitmap((int)(parameter as Canvas).RenderSize.Width,
                                             (int)(parameter as Canvas).RenderSize.Height, 96d, 96d, PixelFormats.Default);
            rtb.Render(parameter as Canvas);
            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)(parameter as Canvas).RenderSize.Width,
                                             (int)(parameter as Canvas).RenderSize.Height));
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(.png)|*.png"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                using (var fs = System.IO.File.OpenWrite(fileName))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void CloseWindow(object parameter)
        {
            (parameter as Window).Close();
        }

        private void About(object parameter)
        {
            MessageBox.Show("Програму розробив студент групи ПМі-43 Романюк Богдан", "Про програму", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region pencil

        private void PencilMouseMove(object sender, MouseEventArgs e)
        {
            Line line = new Line();
            line.Stroke = CurrentColor;
            line.StrokeThickness = (Mode == "Brush") ? Thickness : 1;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = e.GetPosition(sender as IInputElement).X;
            line.Y2 = e.GetPosition(sender as IInputElement).Y;
            currentPoint = e.GetPosition(sender as IInputElement);

            Figures.Add(line);
        }

        private void PencilMouseUp(object sender, MouseEventArgs e)
        {
            (sender as Canvas).MouseMove -= new MouseEventHandler(PencilMouseMove);
        }

        #endregion
        #region eraser

        private void EraserMouseMove(object sender, MouseEventArgs e)
        {
            Line line = new Line();
            line.Stroke = Brushes.White;
            line.StrokeThickness = Thickness;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = e.GetPosition(sender as IInputElement).X;
            line.Y2 = e.GetPosition(sender as IInputElement).Y;
            currentPoint = e.GetPosition(sender as IInputElement);

            Figures.Add(line);
        }

        private void EraserMouseUp(object sender, MouseEventArgs e)
        {
            (sender as Canvas).MouseMove -= new MouseEventHandler(EraserMouseMove);
        }

        #endregion
        #region line

        private void LineMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            if (canvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                var line = Figures.OfType<Line>().LastOrDefault();

                if (line != null)
                {
                    var endPoint = e.GetPosition(canvas);
                    line.X2 = endPoint.X;
                    line.Y2 = endPoint.Y;
                }
            }
        }

        private void LineMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Canvas)sender).ReleaseMouseCapture();
            ((Canvas)sender).MouseMove -= new MouseEventHandler(LineMouseMove);
            ((Canvas)sender).MouseLeftButtonUp -= new MouseButtonEventHandler(LineMouseLeftButtonUp);
        }

        #endregion
        #region rectangle

        private void RectangleMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            if (canvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                var rectangle = Figures.OfType<Rectangle>().LastOrDefault();

                if (rectangle != null)
                {
                    var endPoint = e.GetPosition(canvas);
                    var diffX = endPoint.X - Canvas.GetLeft(rectangle);
                    if (diffX < 0)
                    {
                        rectangle.Width = Canvas.GetLeft(rectangle) - endPoint.X;
                        Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) - (Canvas.GetLeft(rectangle) - endPoint.X));
                    }
                    else
                    {
                        rectangle.Width = diffX;
                    }
                    var diffY = endPoint.Y - Canvas.GetTop(rectangle);
                    if (diffY < 0)
                    {
                        rectangle.Height = Canvas.GetTop(rectangle) - endPoint.Y;
                        Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) - (Canvas.GetTop(rectangle) - endPoint.Y));
                    }
                    else
                    {
                        rectangle.Height = diffY;
                    }
                }
            }
        }

        private void RectangleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Canvas)sender).ReleaseMouseCapture();
            ((Canvas)sender).MouseMove -= new MouseEventHandler(RectangleMouseMove);
            ((Canvas)sender).MouseLeftButtonUp -= new MouseButtonEventHandler(RectangleMouseLeftButtonUp);
        }

        #endregion
        #region ellipse
        private void EllipseMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            if (canvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                var ellipse = Figures.OfType<Ellipse>().LastOrDefault();

                if (ellipse != null)
                {
                    var endPoint = e.GetPosition(canvas);
                    var diffX = endPoint.X - Canvas.GetLeft(ellipse);
                    if (diffX < 0)
                    {
                        ellipse.Width = Canvas.GetLeft(ellipse) - endPoint.X;
                        Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) - (Canvas.GetLeft(ellipse) - endPoint.X));
                    }
                    else
                    {
                        ellipse.Width = diffX;
                    }
                    var diffY = endPoint.Y - Canvas.GetTop(ellipse);
                    if (diffY < 0)
                    {
                        ellipse.Height = Canvas.GetTop(ellipse) - endPoint.Y;
                        Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) - (Canvas.GetTop(ellipse) - endPoint.Y));
                    }
                    else
                    {
                        ellipse.Height = diffY;
                    }
                }
            }
        }

        private void EllipseMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Canvas)sender).ReleaseMouseCapture();
            ((Canvas)sender).MouseMove -= new MouseEventHandler(EllipseMouseMove);
            ((Canvas)sender).MouseLeftButtonUp -= new MouseButtonEventHandler(EllipseMouseLeftButtonUp);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
